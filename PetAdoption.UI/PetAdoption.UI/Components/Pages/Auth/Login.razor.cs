using BlazorBootstrap;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.DTOs;

namespace PetAdoption.UI.Components.Pages.Auth
{
    public partial class Login
    {
        private LoginViewModel modal = new LoginViewModel();

        private async Task LoginUser()
        {
            try
            {
                PreloadService.Show();

                AuthToken response = await petAPI.LoginUserAsync(modal);

                if (response is null || string.IsNullOrEmpty(response.AccessToken))
                {
                    ToastService.Notify(new ToastMessage(ToastType.Danger, $"{modal.Email} not login successfully! Please try again"));
                    return;
                }

                ToastService.Notify(new ToastMessage(ToastType.Success, $"{modal.Email} user not login successfully"));
                await AuthState.MarkUserAsAuthenticated(response);

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
