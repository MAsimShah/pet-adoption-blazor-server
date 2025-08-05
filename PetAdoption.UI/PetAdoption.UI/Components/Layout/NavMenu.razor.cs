using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using MudBlazor;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.APIModels;
using PetAdoption.UI.Components.Pages.Admin.Users;

namespace PetAdoption.UI.Components.Layout
{
    public partial class NavMenu
    {
        private string ProfileImage { get; set; }
        private bool IsAdmin { get; set; }
        DialogOptions dialogOptions = new DialogOptions
        {
            BackgroundClass = "my-custom-class",
            MaxWidth = MaxWidth.ExtraLarge,
            Position = DialogPosition.Center,
            FullWidth = true,
        };

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthState.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var imageUrl = user.FindFirst("ProfileImage")?.Value;

                if (string.IsNullOrEmpty(imageUrl))
                {
                    ProfileImage = Configuration["DefaultProfileImage"];
                }
                else
                {
                    ProfileImage = Configuration["BlazorApiUrl"] + imageUrl;
                }

                IsAdmin = user?.IsInRole("Admin") ?? false;
            }
        }

        private void LoginPage()
        {
            _Naivigation.NavigateTo("/", true);
        }

        private async Task Logout()
        {
            await AuthState.MarkUserAsLoggedOut();
            _Naivigation.NavigateTo("/", true);
        }

        async Task EditUser()
        {
            Loader.Show();

            try
            {
                AuthenticationState? authUser = await AuthState.GetAuthenticationStateAsync();
                var user = authUser.User;
                string? userId = user.FindFirst("Id")?.Value;

                UserModel result = await petAPI.GetUserAsync(userId);

                if (result is null)
                {
                    Snackbar.Add("Not fetched user info", Severity.Error);
                    return;
                }

                UserViewModel model = new()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Email = result.Email,
                    Password = result.Password,
                    PhoneNumber = result.PhoneNumber,
                    ProfileImage = result.ProfileImage
                };

                Loader.Hide();
                var parameters = new DialogParameters<AddUserModel> { { x => x.EditModel, model } };
                var dialog = await DialogService.ShowAsync<AddUserModel>("Edit User", parameters, dialogOptions);
                var dialogResult = await dialog.Result;
            }
            catch (Exception)
            {
                Loader.Hide();
                Snackbar.Add("Not fetched pet info", Severity.Error);
            }
            finally
            {
                Loader.Hide();
            }
        }
    }
}
