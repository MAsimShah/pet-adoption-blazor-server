using Microsoft.AspNetCore.Components.Authorization;
using PetAdoption.UI.Components.Models.DTOs;
using PetAdoption.UI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace PetAdoption.UI.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly string _TokenKey = "authToken";
        private readonly HttpClient _httpClient;
        private readonly CookieStorageService _cookieStorageService;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        public CustomAuthStateProvider(HttpClient httpClient, CookieStorageService cookieStorageService)
        {
            _httpClient = httpClient;
            _cookieStorageService = cookieStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await _cookieStorageService.GetCookieAsync(_TokenKey);

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                // Optionally validate expiration, audience, issuer, signature, etc.

                if (jwt.ValidTo < DateTime.UtcNow)
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

                var identity = new ClaimsIdentity(jwt.Claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async Task<AuthenticationState> MarkUserAsAuthenticated(AuthToken token)
        {
            if (token is null || string.IsNullOrEmpty(token.AccessToken)) return null;

            await _cookieStorageService.SetCookieAsync(_TokenKey, token.AccessToken, 1);
            var user = GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(user);
            return await user;
        }

        public async Task<AuthenticationState> CheckUserAsAuthenticated()
        {
            var state = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _cookieStorageService.DeleteCookieAsync(_TokenKey);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string token)
        {
            var jwt = _tokenHandler.ReadJwtToken(token);
            return jwt.Claims;
        }
    }
}