using MudBlazor;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.DTOs;

namespace PetAdoption.UI.Components.Pages.Auth
{
    public partial class Login
    {
        private LoginViewModel model = new LoginViewModel();

        private async Task LoginUser()
        {
            try
            {
                Loader.Show();

                // auth user
                AuthToken response = await petAPI.LoginUserAsync(model);

                if (response is null || string.IsNullOrEmpty(response.AccessToken))
                {
                    Snackbar.Add($"{model.Email} not login successfully! Please try again", Severity.Error);
                    return;
                }

                Snackbar.Add($"{model.Email} login successfully", Severity.Success);
                await AuthState.MarkUserAsAuthenticated(response);

                _Naivigation.NavigateTo("/", true);
            }
            catch (Exception ex)
            {
                Loader.Hide();
                Snackbar.Add("Something went wrong!", Severity.Success);
            }
            finally
            {
                Loader.Hide();
            }
        }
    }
}