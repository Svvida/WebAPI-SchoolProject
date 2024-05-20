using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using University.RazorPages.Models;

namespace University.RazorPages.Services
{
    public class RestApiService
    {
        private readonly HttpClient _httpClient;

        public RestApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { Login = username, Password = password }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var token = JsonSerializer.Deserialize<TokenResponse>(responseContent)?.Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return token;
            }

            return null;
        }
    }

    public class TokenResponse
    {
        public string Token { get; set; }
    }
}
