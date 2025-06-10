using BlazorBootstrap;
using PetAdoption.UI.Components.Models;

namespace PetAdoption.UI.Components.Pages.Auth
{
    public partial class Signup
    {
        private RegisterViewModel register = new RegisterViewModel();

        private async Task RegisterUser()
        {
            HttpResponseMessage? response = null;
            try
            {
                PreloadService.Show();

                response = await Http.PostAsJsonAsync("/api/Auth/Register", register);

                if (!response.IsSuccessStatusCode)
                {
                    ToastService.Notify(new ToastMessage(ToastType.Danger, $"{register.Name} not saved successfully! Please try again"));
                    return;
                }

                ToastService.Notify(new ToastMessage(ToastType.Success, $"{register.Name} user created successfully"));

                _Naivigation.NavigateTo("/login", true);
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
