#!/bin/bash

# Function to get the local IP address from Windows
get_local_ip() {
  local_ip=$(powershell.exe -Command "
    Get-NetIPAddress -AddressFamily IPv4 |
    Where-Object { \$_.InterfaceAlias -eq 'Ethernet' -and \$_.IPAddress -match '^[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+$' } |
    Select-Object -ExpandProperty IPAddress
  ")
  echo $(echo $local_ip | tr -d '\r')
}

# Get the local IP address dynamically
LOCAL_IP=$(get_local_ip)
echo "Local IP address: $LOCAL_IP"

if [ -z "$LOCAL_IP" ]; then
  echo "Failed to retrieve local IP address."
  exit 1
fi

# Base URL
BASE_URL="https://$LOCAL_IP:7143/graphql"

# Function to execute a cURL command and check the response
execute_curl() {
    echo -e "\nExecuting: $2"
    response=$(eval "$1")
    http_status=$(echo "$response" | jq -r '.errors')
    if [ "$http_status" != "null" ]; then
        echo "Error: $response"
    else
        echo "$response" | jq .
    fi
}

# Add a new Role Mutation and extract role ID
add_role_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { addRole(input: { name: \\\"RolaTestowa\\\" }) { id name } }\"}' $BASE_URL"
echo "Adding new role and extracting role ID"
response=$(eval "$add_role_mutation")
http_status=$(echo "$response" | jq -r '.errors')
if [ "$http_status" != "null" ]; then
    echo "Error: $response"
    echo ""
    exit 1
else
    role_id=$(echo "$response" | jq -r '.data.addRole.id')
    echo "$response" | jq .
fi

# Check if role_id is extracted correctly
if [ -z "$role_id" ]; then
  echo "Failed to extract role ID."
  exit 1
else
  echo "Extracted Role ID: $role_id"
fi

# Role Mutations with extracted role ID
update_role_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { updateRole(id: \\\"$role_id\\\", name: \\\"ZmienionaRola\\\") { id name } }\"}' $BASE_URL"
execute_curl "$update_role_mutation" "Updating Role Mutation"

add_role_to_account_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { addRoleToAccount(accountId: \\\"00000000-0000-0000-0000-000000000004\\\", roleId: \\\"$role_id\\\") { accountId roleId } }\"}' $BASE_URL"
execute_curl "$add_role_to_account_mutation" "Adding Role to Account Mutation"

delete_role_from_account_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { deleteRoleFromAccount(accountId: \\\"00000000-0000-0000-0000-000000000004\\\", roleId: \\\"$role_id\\\") }\"}' $BASE_URL"
execute_curl "$delete_role_from_account_mutation" "Deleting Role from Account Mutation"

delete_role_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { deleteRole(id: \\\"$role_id\\\") }\"}' $BASE_URL"
execute_curl "$delete_role_mutation" "Deleting Role Mutation"

# Add a new Account Mutation and extract account ID
add_account_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { addAccount(input: { email: \\\"masakra.z.tymi.komendami@wsei.pl\\\", login: \\\"aleDalemRade\\\", password: \\\"testPassword\\\", isActive: true }) { id email login isActive } }\"}' $BASE_URL"
echo "Adding new account and extracting account ID"
response=$(eval "$add_account_mutation")
http_status=$(echo "$response" | jq -r '.errors')
if [ "$http_status" != "null" ]; then
    echo "Error: $response"
    echo ""
    exit 1
else
    account_id=$(echo "$response" | jq -r '.data.addAccount.id')
    echo "$response" | jq .
fi

# Check if account_id is extracted correctly
if [ -z "$account_id" ]; then
  echo "Failed to extract account ID."
  exit 1
else
  echo "Extracted Account ID: $account_id"
fi

update_account_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { updateAccount(input: { id: \\\"$account_id\\\", email: \\\"zmienione.konto@wsei.pl\\\", login: \\\"ZmienionyLogin\\\", isActive: false }) { id email login isActive } }\"}' $BASE_URL"
execute_curl "$update_account_mutation" "Updating Account Mutation"

delete_account_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { deleteAccount(id: \\\"$account_id\\\") }\"}' $BASE_URL"
execute_curl "$delete_account_mutation" "Deleting Account Mutation"

# Add a new Student Mutation and extract student ID
add_student_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{
  \"query\": \"mutation { addStudent(input: { name: \\\"Jan\\\", surname: \\\"Kowalski\\\", dateOfBirth: \\\"1990-01-01\\\", pesel: \\\"12345678901\\\", gender: MALE, country: \\\"Polska\\\", city: \\\"Warszawa\\\", postalCode: \\\"00-001\\\", street: \\\"Krakowskie Przedmieœcie\\\", buildingNumber: \\\"1\\\", apartmentNumber: \\\"1\\\", accountId: \\\"$account_id\\\" }) { id name surname dateOfBirth pesel gender } }\"}' $BASE_URL"
echo "Adding new student and extracting student ID"
response=$(eval "$add_student_mutation")
http_status=$(echo "$response" | jq -r '.errors')
if [ "$http_status" != "null" ]; then
    echo "Error: $response"
    echo ""
    exit 1
else
    student_id=$(echo "$response" | jq -r '.data.addStudent.id')
    echo "$response" | jq .
fi

# Check if student_id is extracted correctly
if [ -z "$student_id" ]; then
  echo "Failed to extract student ID."
  exit 1
else
  echo "Extracted Student ID: $student_id"
fi

update_student_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{
  \"query\": \"mutation { updateStudent(input: { id: \\\"$student_id\\\", name: \\\"JanUpdated\\\", surname: \\\"KowalskiUpdated\\\", dateOfBirth: \\\"1991-01-01\\\", pesel: \\\"12345678902\\\", gender: FEMALE, country: \\\"Polska\\\", city: \\\"Warszawa\\\", postalCode: \\\"00-001\\\", street: \\\"Krakowskie Przedmieœcie\\\", buildingNumber: \\\"1\\\", apartmentNumber: \\\"1\\\", accountId: \\\"$account_id\\\" }) { id name surname dateOfBirth pesel gender } }\"}' $BASE_URL"
execute_curl "$update_student_mutation" "Updating Student Mutation"

delete_student_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { deleteStudent(id: \\\"$student_id\\\") }\"}' $BASE_URL"
execute_curl "$delete_student_mutation" "Deleting Student Mutation"

# Address Mutations

# Add a new Address Mutation and extract address ID
add_address_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { addAddress(input: { country: \\\"Polska\\\", city: \\\"Warszawa\\\", postalCode: \\\"00-001\\\", street: \\\"Krakowskie Przedmieœcie\\\", buildingNumber: \\\"1\\\", apartmentNumber: \\\"1\\\" }) { id country city postalCode street buildingNumber apartmentNumber } }\"}' $BASE_URL"
echo "Adding new address and extracting address ID"
response=$(eval "$add_address_mutation")
http_status=$(echo "$response" | jq -r '.errors')
if [ "$http_status" != "null" ]; then
    echo "Error: $response"
    echo ""
    exit 1
else
    address_id=$(echo "$response" | jq -r '.data.addAddress.id')
    echo "$response" | jq .
fi

# Check if address_id is extracted correctly
if [ -z "$address_id" ]; then
  echo "Failed to extract address ID."
  exit 1
else
  echo "Extracted Address ID: $address_id"
fi
update_address_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { updateAddress(input: { id: \\\"$address_id\\\", country: \\\"Humpolec\\\", city: \\\"A nie, Humpolec to miasto\\\", postalCode: \\\"77-777\\\", street: \\\"Czatu\\\", buildingNumber: \\\"GPT\\\", apartmentNumber: \\\"4o\\\" }) { id country city postalCode street buildingNumber apartmentNumber } }\"}' $BASE_URL"
execute_curl "$update_address_mutation" "Updating Address Mutation"

delete_address_mutation="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { deleteAddress(id: \\\"$address_id\\\") }\"}' $BASE_URL"
execute_curl "$delete_address_mutation" "Deleting Address Mutation"
# Role Queries
retrieve_roles_query="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"query { allRoles { id name } }\"}' $BASE_URL"
execute_curl "$retrieve_roles_query" "Retrieving All Roles Query"

# Student Queries
retrieve_students_query="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"query { allStudents { id name surname dateOfBirth pesel gender } }\"}' $BASE_URL"
execute_curl "$retrieve_students_query" "Retrieving All Students Query"

retrieve_student_by_field_query="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"query { studentByField(field: \\\"name\\\", value: \\\"Jan\\\") { id name surname dateOfBirth pesel gender } }\"}' $BASE_URL"
execute_curl "$retrieve_student_by_field_query" "Retrieving Student by Field Query"

# Address Queries
retrieve_addresses_query="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"query { allAddresses { id country city postalCode street buildingNumber apartmentNumber } }\"}' $BASE_URL"
execute_curl "$retrieve_addresses_query" "Retrieving All Addresses Query"

retrieve_address_by_field_query="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"query { addressByField(field: \\\"city\\\", value: \\\"Warszawa\\\") { id country city postalCode street buildingNumber apartmentNumber } }\"}' $BASE_URL"
execute_curl "$retrieve_address_by_field_query" "Retrieving Address by Field Query"

# Account Queries
retrieve_accounts_query="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"query { allAccounts { id email login password isActive deactivationDate } }\"}' $BASE_URL"
execute_curl "$retrieve_accounts_query" "Retrieving All Accounts Query"

retrieve_account_by_field_query="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"query { accountByField(field: \\\"email\\\", value: \\\"marta.radzka@wsei.pl\\\") { id email login password isActive deactivationDate } }\"}' $BASE_URL"
execute_curl "$retrieve_account_by_field_query" "Retrieving Account by Field Query"

# AccountRole Queries
retrieve_accounts_with_roles_query="curl -k -s -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"query { allAccountsWithRoles { accountId roleId account { id email login password isActive deactivationDate } role { id name } } }\"}' $BASE_URL"
execute_curl "$retrieve_accounts_with_roles_query" "Retrieving All Accounts with Roles Query"