using BlazorBootstrap;
using PetAdoption.UI.Components.Models;

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
