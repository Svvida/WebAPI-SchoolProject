#!/bin/bash

# Function to get the local IP address from Windows
get_local_ip() {
  # Use PowerShell to get the IP address
  local_ip=$(powershell.exe -Command "
    Get-NetIPAddress -AddressFamily IPv4 |
    Where-Object { \$_.InterfaceAlias -eq 'Ethernet' -and \$_.IPAddress -match '^[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+$' } |
    Select-Object -ExpandProperty IPAddress
  ")

  # Trim any trailing whitespace (if any)
  echo $(echo $local_ip | tr -d '\r')
}

# Get the local IP address dynamically
LOCAL_IP=$(get_local_ip)
echo "Local IP address: $LOCAL_IP"

# Check if the IP address was successfully retrieved
if [ -z "$LOCAL_IP" ]; then
  echo "Failed to retrieve local IP address."
  exit 1
fi

# Base URL
BASE_URL="http://$LOCAL_IP:5217/api"

# Login and get JWT token
echo "Logging in to get JWT token..."
LOGIN_RESPONSE=$(curl -s -X POST "$BASE_URL/auth/login" -H "Content-Type: application/json" -d '{
    "login": "admin",
    "password": "admin123"
}')
TOKEN=$(echo $LOGIN_RESPONSE | jq -r '.token')

if [ "$TOKEN" == "null" ]; then
  echo "Login failed: $(echo $LOGIN_RESPONSE | jq -r '.message')"
  exit 1
fi

echo "Login successful, JWT token obtained."

# Set Authorization header
AUTH_HEADER="Authorization: Bearer $TOKEN"

# Function to make authenticated GET request
function get_request() {
  URL=$1
  echo "GET $URL"
  RESPONSE=$(curl -s -w "\n%{http_code}" -X GET "$URL" -H "$AUTH_HEADER")
  HTTP_CODE=$(echo "$RESPONSE" | tail -n1)
  RESPONSE_BODY=$(echo "$RESPONSE" | sed '$d')
  if [ "$HTTP_CODE" -eq 200 ]; then
    echo "GET request to $URL succeeded."
  else
    echo "GET request to $URL failed with status code $HTTP_CODE."
    echo "Response: $RESPONSE_BODY"
  fi
}

# Function to make authenticated POST request
function post_request() {
  URL=$1
  DATA=$2
  echo "POST $URL"
  RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$URL" -H "Content-Type: application/json" -H "$AUTH_HEADER" -d "$DATA")
  HTTP_CODE=$(echo "$RESPONSE" | tail -n1)
  RESPONSE_BODY=$(echo "$RESPONSE" | sed '$d')
  if [ "$HTTP_CODE" -eq 201 ]; then
    echo "POST request to $URL succeeded."
  else
    echo "POST request to $URL failed with status code $HTTP_CODE."
    echo "Response: $RESPONSE_BODY"
  fi
}

# Function to make authenticated PUT request
function put_request() {
  URL=$1
  DATA=$2
  echo "PUT $URL"
  RESPONSE=$(curl -s -w "\n%{http_code}" -X PUT "$URL" -H "Content-Type: application/json" -H "$AUTH_HEADER" -d "$DATA")
  HTTP_CODE=$(echo "$RESPONSE" | tail -n1)
  RESPONSE_BODY=$(echo "$RESPONSE" | sed '$d')
  if [ "$HTTP_CODE" -eq 204 ]; then
    echo "PUT request to $URL succeeded."
  else
    echo "PUT request to $URL failed with status code $HTTP_CODE."
    echo "Response: $RESPONSE_BODY"
  fi
}

# Function to make authenticated DELETE request
function delete_request() {
  URL=$1
  echo "DELETE $URL"
  RESPONSE=$(curl -s -w "\n%{http_code}" -X DELETE "$URL" -H "$AUTH_HEADER")
  HTTP_CODE=$(echo "$RESPONSE" | tail -n1)
  RESPONSE_BODY=$(echo "$RESPONSE" | sed '$d')
  if [ "$HTTP_CODE" -eq 204 ]; then
    echo "DELETE request to $URL succeeded."
  else
    echo "DELETE request to $URL failed with status code $HTTP_CODE."
    echo "Response: $RESPONSE_BODY"
  fi
}

# Test Accounts endpoints
echo "Testing Accounts endpoints..."
get_request "$BASE_URL/accounts"
post_request "$BASE_URL/accounts" '{
  "email": "nowy.uzytkownik@przyklad.pl",
  "login": "nowyuzytkownik",
  "password": "nowyuzytkownik123",
  "isActive": true
}'
ACCOUNT_ID=$(curl -s -X GET "$BASE_URL/accounts" -H "$AUTH_HEADER" | jq -r '.[-1].id')
put_request "$BASE_URL/accounts/$ACCOUNT_ID" '{
  "id": "'$ACCOUNT_ID'",
  "email": "zaktualizowany.uzytkownik@przyklad.pl",
  "login": "zaktualizowanyuzytkownik",
  "password": "nowyuzytkownik123",
  "isActive": true
}'
delete_request "$BASE_URL/accounts/$ACCOUNT_ID"

# Test Addresses endpoints
echo "Testing Addresses endpoints..."
get_request "$BASE_URL/addresses"

# Test Roles endpoints
echo "Testing Roles endpoints..."
get_request "$BASE_URL/roles"
post_request "$BASE_URL/roles" '{
  "name": "NowaRola"
}'
ROLE_ID=$(curl -s -X GET "$BASE_URL/roles" -H "$AUTH_HEADER" | jq -r '.[-1].id')
put_request "$BASE_URL/roles/$ROLE_ID" '{
  "id": "'$ROLE_ID'",
  "name": "ZaktualizowanaRola"
}'
delete_request "$BASE_URL/roles/$ROLE_ID"

# Test Students endpoints
echo "Testing Students endpoints..."
get_request "$BASE_URL/students"