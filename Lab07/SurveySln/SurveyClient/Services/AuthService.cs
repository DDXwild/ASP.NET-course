using System.Net.Http.Json;
using SurveyClient.Models;

namespace SurveyClient.Services;

public class AuthService(HttpClient httpClient, JwtAuthenticationStateProvider authStateProvider)
{
    public async Task<(bool Succeeded, string Error)> RegisterAsync(RegisterRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("api/account/register", new
        {
            username = request.Username,
            email = request.Email,
            password = request.Password
        });

        if (response.IsSuccessStatusCode)
        {
            return (true, string.Empty);
        }

        var errorText = await response.Content.ReadAsStringAsync();
        return (false, errorText);
    }

    public async Task<(bool Succeeded, string Error)> LoginAsync(LoginRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("api/account/login", request);
        if (!response.IsSuccessStatusCode)
        {
            return (false, "Invalid credentials.");
        }

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (loginResponse is null || string.IsNullOrWhiteSpace(loginResponse.Token))
        {
            return (false, "Token was not returned by API.");
        }

        await authStateProvider.MarkUserAsAuthenticatedAsync(loginResponse.Token);
        return (true, string.Empty);
    }

    public Task LogoutAsync() => authStateProvider.MarkUserAsLoggedOutAsync();
}
