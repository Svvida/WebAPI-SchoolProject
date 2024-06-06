using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using University.RazorPages.Models;
using System.Net.Http.Headers;

namespace University.RazorPages.Services
{
    public class GraphQLService
    {
        private readonly GraphQLHttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GraphQLService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _client = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://localhost:5237/graphql")
            }, new SystemTextJsonSerializer(), httpClient);
        }

        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(token))
            {
                _client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // Queries
        public async Task<IList<AccountModel>> GetAccountsAsync()
        {
            AddAuthorizationHeader();
            var query = new GraphQLRequest
            {
                Query = @"
                query {
                    allAccounts {
                        id
                        email
                        login
                        isActive
                        roles {
                            id
                            name
                        }
                    }
                }"
            };

            var response = await _client.SendQueryAsync<Response<IList<AccountModel>>>(query);
            return response.Data.AllAccounts;
        }

        public async Task<IList<AddressModel>> GetAddressesAsync()
        {
            AddAuthorizationHeader();
            var query = new GraphQLRequest
            {
                Query = @"
                query {
                    allAddresses {
                        id
                        country
                        city
                        postalCode
                        street
                        buildingNumber
                        apartmentNumber
                    }
                }"
            };

            var response = await _client.SendQueryAsync<Response<IList<AddressModel>>>(query);
            return response.Data.AllAddresses;
        }

        public async Task<IList<StudentModel>> GetStudentsAsync()
        {
            AddAuthorizationHeader();
            var query = new GraphQLRequest
            {
                Query = @"
                query {
                    allStudents {
                        id
                        name
                        surname
                        dateOfBirth
                        pesel
                        gender
                        address {
                            country
                            city
                            postalCode
                            street
                            buildingNumber
                            apartmentNumber
                        }
                    }
                }"
            };

            var response = await _client.SendQueryAsync<Response<IList<StudentModel>>>(query);
            return response.Data.AllStudents;
        }

        public async Task<IList<RoleModel>> GetRolesAsync()
        {
            AddAuthorizationHeader();
            var query = new GraphQLRequest
            {
                Query = @"
                query {
                    allRoles {
                        id
                        name
                    }
                }"
            };

            var response = await _client.SendQueryAsync<Response<IList<RoleModel>>>(query);
            return response.Data.AllRoles;
        }

        // Mutations
        public async Task<AccountModel> AddAccountAsync(AccountInputModel input)
        {
            AddAuthorizationHeader();
            var mutation = new GraphQLRequest
            {
                Query = @"
                mutation($input: AccountInput!) {
                    addAccountAsync(input: $input) {
                        id
                        email
                        login
                        isActive
                    }
                }",
                Variables = new { input }
            };

            var response = await _client.SendMutationAsync<Response<AccountModel>>(mutation);
            return response.Data.AddAccountAsync;
        }

        public async Task<AccountModel> UpdateAccountAsync(UpdateAccountInputModel input)
        {
            AddAuthorizationHeader();
            var mutation = new GraphQLRequest
            {
                Query = @"
                mutation($input: UpdateAccountInput!) {
                    updateAccountAsync(input: $input) {
                        id
                        email
                        login
                        isActive
                    }
                }",
                Variables = new { input }
            };

            var response = await _client.SendMutationAsync<Response<AccountModel>>(mutation);
            return response.Data.UpdateAccountAsync;
        }

        public async Task<bool> DeleteAccountAsync(Guid id)
        {
            AddAuthorizationHeader();
            var mutation = new GraphQLRequest
            {
                Query = @"
                mutation($id: ID!) {
                    deleteAccountAsync(id: $id)
                }",
                Variables = new { id }
            };

            var response = await _client.SendMutationAsync<Response<bool>>(mutation);
            return response.Data.DeleteAccountAsync;
        }

        public async Task<RoleModel> AddRoleAsync(RoleInputModel input)
        {
            AddAuthorizationHeader();
            var mutation = new GraphQLRequest
            {
                Query = @"
                mutation($input: RoleInput!) {
                    addRoleAsync(input: $input) {
                        id
                        name
                    }
                }",
                Variables = new { input }
            };

            var response = await _client.SendMutationAsync<Response<RoleModel>>(mutation);
            return response.Data.AddRoleAsync;
        }

        public async Task<RoleModel> UpdateRoleAsync(Guid id, string name)
        {
            AddAuthorizationHeader();
            var mutation = new GraphQLRequest
            {
                Query = @"
                mutation($id: ID!, $name: String!) {
                    updateRoleAsync(id: $id, name: $name) {
                        id
                        name
                    }
                }",
                Variables = new { id, name }
            };

            var response = await _client.SendMutationAsync<Response<RoleModel>>(mutation);
            return response.Data.UpdateRoleAsync;
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            AddAuthorizationHeader();
            var mutation = new GraphQLRequest
            {
                Query = @"
                mutation($id: ID!) {
                    deleteRoleAsync(id: $id)
                }",
                Variables = new { id }
            };

            var response = await _client.SendMutationAsync<Response<bool>>(mutation);
            return response.Data.DeleteRoleAsync;
        }

        public async Task<StudentModel> AddStudentAsync(StudentInputModel input)
        {
            AddAuthorizationHeader();
            var mutation = new GraphQLRequest
            {
                Query = @"
                mutation($input: StudentInput!) {
                    addStudentAsync(input: $input) {
                        id
                        name
                        surname
                        dateOfBirth
                        pesel
                        gender
                        address {
                            country
                            city
                            postalCode
                            street
                            buildingNumber
                            apartmentNumber
                        }
                    }
                }",
                Variables = new { input }
            };

            var response = await _client.SendMutationAsync<Response<StudentModel>>(mutation);
            return response.Data.AddStudentAsync;
        }

        public async Task<StudentModel> UpdateStudentAsync(UpdateStudentInputModel input)
        {
            AddAuthorizationHeader();
            var mutation = new GraphQLRequest
            {
                Query = @"
                mutation($input: UpdateStudentInput!) {
                    updateStudentAsync(input: $input) {
                        id
                        name
                        surname
                        dateOfBirth
                        pesel
                        gender
                        address {
                            country
                            city
                            postalCode
                            street
                            buildingNumber
                            apartmentNumber
                        }
                    }
                }",
                Variables = new { input }
            };

            var response = await _client.SendMutationAsync<Response<StudentModel>>(mutation);
            return response.Data.UpdateStudentAsync;
        }

        public async Task<bool> DeleteStudentAsync(Guid id)
        {
            AddAuthorizationHeader();
            var mutation = new GraphQLRequest
            {
                Query = @"
                mutation($id: ID!) {
                    deleteStudentAsync(id: $id)
                }",
                Variables = new { id }
            };

            var response = await _client.SendMutationAsync<Response<bool>>(mutation);
            return response.Data.DeleteStudentAsync;
        }

        private class Response<T>
        {
            public T AllAccounts { get; set; }
            public T AllAddresses { get; set; }
            public T AllStudents { get; set; }
            public T AllRoles { get; set; }
            public T AddAccountAsync { get; set; }
            public T UpdateAccountAsync { get; set; }
            public T DeleteAccountAsync { get; set; }
            public T AddRoleAsync { get; set; }
            public T UpdateRoleAsync { get; set; }
            public T DeleteRoleAsync { get; set; }
            public T AddStudentAsync { get; set; }
            public T UpdateStudentAsync { get; set; }
            public T DeleteStudentAsync { get; set; }
        }
    }
}
