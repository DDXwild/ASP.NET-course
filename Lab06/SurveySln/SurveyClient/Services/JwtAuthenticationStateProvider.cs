using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace SurveyClient.Services;

public class JwtAuthenticationStateProvider(HttpClient httpClient, IJSRuntime jsRuntime) : AuthenticationStateProvider
{
    private const string TokenStorageKey = "authToken";
    private static readonly AuthenticationState AnonymousState = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await GetStoredTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            httpClient.DefaultRequestHeaders.Authorization = null;
            return AnonymousState;
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var identity = new ClaimsIdentity(ParseClaims(token), "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task MarkUserAsAuthenticatedAsync(string token)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenStorageKey, token);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var identity = new ClaimsIdentity(ParseClaims(token), "jwt");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
    }

    public async Task MarkUserAsLoggedOutAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenStorageKey);
        httpClient.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(Task.FromResult(AnonymousState));
    }

    private async Task<string?> GetStoredTokenAsync()
    {
        try
        {
            return await jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenStorageKey);
        }
        catch
        {
            return null;
        }
    }

    private static IEnumerable<Claim> ParseClaims(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonBytes) ?? [];

        var claims = new List<Claim>();
        foreach (var pair in keyValuePairs)
        {
            if (pair.Key == ClaimTypes.Role || pair.Key == "role")
            {
                if (pair.Value.ValueKind == JsonValueKind.Array)
                {
                    claims.AddRange(pair.Value.EnumerateArray().Select(role => new Claim(ClaimTypes.Role, role.GetString() ?? string.Empty)));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, pair.Value.GetString() ?? string.Empty));
                }
            }
            else
            {
                claims.Add(new Claim(pair.Key, pair.Value.ToString()));
            }
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        var output = base64.Replace('-', '+').Replace('_', '/');
        switch (output.Length % 4)
        {
            case 2:
                output += "==";
                break;
            case 3:
                output += "=";
                break;
        }

        return Convert.FromBase64String(output);
    }
}
