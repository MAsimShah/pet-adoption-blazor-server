using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PetAdoption.UI.Components.Pages.PetRequests;
using PetAdoption.UI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace PetAdoption.UI.Auth
{
    public class CustomJwtAuthenticateHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly CookieStorageService _cookieStorageService;
        public CustomJwtAuthenticateHandler(
            CookieStorageService cookieStorageService,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _cookieStorageService = cookieStorageService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string? token = null;

            if (Request.Headers.ContainsKey("Authorization"))
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = authHeader.Substring("Bearer ".Length).Trim();
                }
            }

            if (string.IsNullOrEmpty(token))
            {
                token = Request.Cookies["authToken"];
            }

            if (string.IsNullOrWhiteSpace(token))
                return AuthenticateResult.Fail("No token found.");

            try
            {
                // Instead of assigning an AuthenticationHeaderValue, assign the token string directly
                Request.Headers["Authorization"] = $"Bearer {token}";

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                
                if (jwt.ValidTo < DateTime.UtcNow)
                    return AuthenticateResult.Fail("Token expired");

                var identity = new ClaimsIdentity(jwt.Claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Token validation failed: {ex.Message}");
            }
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Redirect("/login");
            return Task.CompletedTask;
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.Redirect("/access-denied");
            return Task.CompletedTask;
        }
    }
}
