using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using University.Application.DTOs;

namespace University.RazorPages.Services
{
    public class RestApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RestApiService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RestApiService(HttpClient httpClient, ILogger<RestApiService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { login = username, password = password }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API Response: {ResponseContent}", responseContent);

                var tokenJson = JsonDocument.Parse(responseContent);
                var token = tokenJson.RootElement.GetProperty("token").GetString();

                if (!string.IsNullOrEmpty(token))
                {
                    _logger.LogInformation("Token received: {Token}", token);
                    SaveToken(token);
                    return token;
                }
                else
                {
                    _logger.LogError("Token is null or empty");
                }
            }
            else
            {
                _logger.LogWarning("Login failed with status code: {StatusCode}", response.StatusCode);
                _logger.LogWarning("API Response: {ResponseContent}", await response.Content.ReadAsStringAsync());
            }
            return null;
        }

        private void SaveToken(string token)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                _logger.LogError("Session is null. Ensure session middleware is correctly configured.");
                throw new InvalidOperationException("Session is null. Ensure session middleware is correctly configured.");
            }

            _logger.LogInformation("Saving token to session: {Token}", token);
            session.SetString("JWToken", token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private string GetToken()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetString("JWToken");
        }

        private void AddAuthorizationHeader()
        {
            var token = GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private async Task<HttpResponseMessage> TryRequestAsync(Func<Task<HttpResponseMessage>> requestFunc)
        {
            var response = await requestFunc();
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogError("Request failed: {StatusCode}", response?.StatusCode);
            }
            return response;
        }

        // Generic method to handle API requests
        private async Task<T> GetAsync<T>(string endpoint)
        {
            AddAuthorizationHeader();
            var response = await TryRequestAsync(() => _httpClient.GetAsync(endpoint));

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseContent);
            }

            return default;
        }

        public Task<IList<StudentDto>> GetStudentsAsync()
        {
            return GetAsync<IList<StudentDto>>("api/students");
        }

        public Task<StudentDto> GetStudentByIdAsync(Guid id)
        {
            return GetAsync<StudentDto>($"api/students/{id}");
        }

        public async Task<StudentDto> CreateStudentAsync(StudentDto student)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(student), Encoding.UTF8, "application/json");
            var response = await TryRequestAsync(() => _httpClient.PostAsync("api/students", content));

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<StudentDto>(responseContent);
            }

            return null;
        }

        public async Task UpdateStudentAsync(StudentDto student)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(student), Encoding.UTF8, "application/json");
            await TryRequestAsync(() => _httpClient.PutAsync($"api/students/{student.Id}", content));
        }

        public async Task DeleteStudentAsync(Guid id)
        {
            AddAuthorizationHeader();
            await TryRequestAsync(() => _httpClient.DeleteAsync($"api/students/{id}"));
        }
    }
}
