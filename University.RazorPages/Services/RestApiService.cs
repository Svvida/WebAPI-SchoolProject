using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using University.Application.DTOs;

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
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

                if (tokenResponse?.Token != null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Token);
                    return tokenResponse.Token;
                }
            }

            return null;
        }

        public async Task<IList<StudentDto>> GetStudentsAsync()
        {
            var response = await _httpClient.GetAsync("/api/students");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<StudentDto>>(responseContent);
            }

            return new List<StudentDto>();
        }

        public async Task CreateStudentAsync(StudentDto student)
        {
            var content = new StringContent(JsonSerializer.Serialize(student), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/students", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<StudentDto> GetStudentByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/students/{id}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<StudentDto>(responseContent);
            }

            return null;
        }

        public async Task UpdateStudentAsync(StudentDto student)
        {
            var content = new StringContent(JsonSerializer.Serialize(student), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/students/{student.Id}", content);
            response.EnsureSuccessStatusCode();
        }
    }

    public class TokenResponse
    {
        public string Token { get; set; }
    }
}
