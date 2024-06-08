using University.RestApi.Controllers;
using University.Application.DTOs;
using University.Application.Services;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Xunit;
using System.Net.Http.Json;
using System.Net;
using University.RestApi;
using Microsoft.AspNetCore.Mvc.Testing;

namespace University.Tests.IntegrationTests.Controllers
{
    public class AuthControllerTests : IntegrationTestBase
    {
        private readonly HttpClient _client;

        public AuthControllerTests()
        {
            _client = new WebApplicationFactory<Program>().CreateClient();
        }

        [Fact]
        public async Task Login_ValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var loginDto = new LoginDto { Login = "admin", Password = "admin123" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            content.Should().ContainKey("token");
        }

        [Fact]
        public async Task Login_InvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto { Login = "admin", Password = "wrongpassword" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
