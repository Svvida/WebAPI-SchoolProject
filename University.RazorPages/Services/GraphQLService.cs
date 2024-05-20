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

        private class Response<T>
        {
            public T AllAccounts { get; set; }
            public T AllAddresses { get; set; }
        }
    }
}
