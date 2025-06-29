using BlazorBootstrap;
using PetAdoption.UI.Components.Models;
using System.Security.Claims;
using System.Text.Json;

namespace PetAdoption.UI.Components.Pages.Auth
{
    public partial class Login
    {
        private LoginViewModel modal = new LoginViewModel();
        private string? username;
        private string? role;

        protected override async Task OnInitializedAsync()
        {
            var state = await AuthState.CheckUserAsAuthenticated();

            if (state != null && state.User.Identity != null && state.User.Identity.IsAuthenticated)
            {
                username = state.User.Identity?.Name;
                role = state.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            }
        }

        private async Task AuthUser()
        {
            RegisterViewModel modal2 = new RegisterViewModel() { Email = "test@12s.com", Name = "Authpassword", PhoneNumber = "12232", Password = "password", Gender = Gender.Female };

            HttpResponseMessage? response = await Http.PostAsJsonAsync("/api/Auth/Login", modal2);

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var token = tokenResponse?.AccessToken;

           await cookieService.SetCookieAsync("authToken", token, 1);
            await OnInitializedAsync();
        }

        private async Task LoginUser()
        {
            try
            {
                PreloadService.Show();

                HttpResponseMessage? response = await Http.PostAsJsonAsync("/api/Auth/Login", modal);

                if (!response.IsSuccessStatusCode)
                {
                    ToastService.Notify(new ToastMessage(ToastType.Danger, $"{modal.Email} not login successfully! Please try again"));
                    return;
                }

                ToastService.Notify(new ToastMessage(ToastType.Success, $"{modal.Email} user not login successfully"));

                _Naivigation.NavigateTo("/", true);
            }
            catch (Exception ex)
            {
                PreloadService.Hide();
                ToastService.Notify(new ToastMessage(ToastType.Danger, $"Something went wrong!"));
            }
            finally
            {
                PreloadService.Hide();
            }
        }
    }
}
