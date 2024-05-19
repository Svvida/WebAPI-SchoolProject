#!/bin/bash

# Base URL of the GraphQL API
BASE_URL="http://192.168.11.247:5237/graphql/"

# Function to execute a cURL command and return the response
execute_curl() {
    echo "Executing: $1"
    response=$(eval $1)
    echo "Response: $response"
    echo -e "\n"
    echo "$response"
}

# Function to extract ID from the JSON response
extract_id() {
    echo "$1" | grep -oP '(?<="id":")[^"]+' | head -n 1
}

# Add a new Role Mutation
add_role_mutation='curl -X POST -H "Content-Type: application/json" -d "{\"query\": \"mutation { addRole(input: { name: \\\"TemporaryRole\\\" }) { id name } }\"}" $BASE_URL'
response=$(execute_curl "$add_role_mutation")

# Extract the newly added role ID for further operations
role_id=$(extract_id "$response")
echo "Extracted Role ID: $role_id"

# Add Role to Account Mutation (using the new role ID)
add_role_to_account_mutation="curl -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { addRoleToAccount(accountId: \\\"00000000-0000-0000-0000-000000000004\\\", roleId: \\\"$role_id\\\") { account_id role_id } }\"}' $BASE_URL"
execute_curl "$add_role_to_account_mutation"

# Update Role Mutation (using the new role ID)
update_role_mutation="curl -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { updateRole(id: \\\"$role_id\\\", name: \\\"UpdatedTemporaryRole\\\") { id name } }\"}' $BASE_URL"
execute_curl "$update_role_mutation"

# Delete Role from Account Mutation (using the new role ID)
delete_role_from_account_mutation="curl -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { deleteRoleFromAccount(accountId: \\\"00000000-0000-0000-0000-000000000004\\\", roleId: \\\"$role_id\\\") }\"}' $BASE_URL"
execute_curl "$delete_role_from_account_mutation"

# Delete the new Role Mutation (using the new role ID)
delete_role_mutation="curl -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { deleteRole(id: \\\"$role_id\\\") }\"}' $BASE_URL"
execute_curl "$delete_role_mutation"

# Add Account Mutation
add_account_mutation='curl -X POST -H "Content-Type: application/json" -d "{\"query\": \"mutation { addAccount(input: { email: \\\"jan.kowalski@example.com\\\", login: \\\"jankowalski\\\", password: \\\"password123\\\", isActive: true }) { id email login is_active } }\"}" $BASE_URL'
response=$(execute_curl "$add_account_mutation")

# Extract the newly added account ID for further operations
account_id=$(extract_id "$response")
echo "Extracted Account ID: $account_id"

# Update Account Mutation
update_account_mutation="curl -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { updateAccount(input: { id: \\\"$account_id\\\", email: \\\"jan.nowak@example.com\\\", login: \\\"jannowak\\\", isActive: false }) { id email login is_active } }\"}' $BASE_URL"
execute_curl "$update_account_mutation"

# Delete Account Mutation
delete_account_mutation="curl -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { deleteAccount(id: \\\"$account_id\\\") }\"}' $BASE_URL"
execute_curl "$delete_account_mutation"

# Add Student Mutation
add_student_mutation="curl -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { addStudent(input: { name: \\\"Adam\\\", surname: \\\"Nowak\\\", dateOfBirth: \\\"2001-05-15\\\", pesel: \\\"32165498701\\\", gender: MALE, country: \\\"Polska\\\", city: \\\"Warszawa\\\", postalCode: \\\"00-001\\\", street: \\\"Glowna\\\", buildingNumber: \\\"1\\\", apartmentNumber: \\\"1\\\", accountId: \\\"$account_id\\\" }) { id name surname date_of_birth pesel gender address { country city postal_code street building_number apartment_number } } }\"}' $BASE_URL"
response=$(execute_curl "$add_student_mutation")

# Extract the newly added student ID for further operations
student_id=$(extract_id "$response")
echo "Extracted Student ID: $student_id"

# Update Student Mutation
update_student_mutation="curl -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { updateStudent(input: { id: \\\"$student_id\\\", name: \\\"Adam\\\", surname: \\\"Nowakowski\\\", dateOfBirth: \\\"2001-05-15\\\", pesel: \\\"32165498701\\\", gender: MALE, country: \\\"Polska\\\", city: \\\"Warszawa\\\", postalCode: \\\"00-001\\\", street: \\\"Glowna\\\", buildingNumber: \\\"2\\\", apartmentNumber: \\\"3\\\", accountId: \\\"$account_id\\\" }) { id name surname date_of_birth pesel gender address { country city postal_code street building_number apartment_number } } }\"}' $BASE_URL"
execute_curl "$update_student_mutation"

# Delete Student Mutation
delete_student_mutation="curl -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { deleteStudent(id: \\\"$student_id\\\") }\"}' $BASE_URL"
execute_curl "$delete_student_mutation"

# Update Address Mutation
update_address_mutation="curl -X POST -H \"Content-Type: application/json\" -d '{\"query\": \"mutation { updateAddress(input: { id: \\\"bb7a5062-dd7a-4295-bb29-05a042638467\\\", country: \\\"Polska\\\", city: \\\"Warszawa\\\", postalCode: \\\"00-002\\\", street: \\\"Poboczna\\\", buildingNumber: \\\"10\\\", apartmentNumber: \\\"20\\\" }) { id country city postal_code street building_number apartment_number } }\"}' $BASE_URL"
execute_curl "$update_address_mutation"

echo "API tests completed."
