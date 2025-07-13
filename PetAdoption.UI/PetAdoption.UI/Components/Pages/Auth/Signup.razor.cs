using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.APIModels;
using PetAdoption.UI.Components.Models.DTOs;
using System.Security.Claims;

namespace PetAdoption.UI.Components.Pages.Auth
{
    public partial class Signup
    {
        private string? previewImageUrl;
        private Base64ImageFile? profileIage;
        private MudFileUpload<IBrowserFile>? _fileElement;

        private RegisterViewModel model = new RegisterViewModel();
       // public IBrowserFile ProfileFile { get; set; } = null;


        private async Task AskImageUpload()
        {
            await _fileElement?.OpenFilePickerAsync();
        }
        private async Task UploadProfilePhoto(InputFileChangeEventArgs e)
        {
            var profileFile = e.File;

            if (profileFile != null)
            {
                var resizedImageFile = await profileFile.RequestImageFileAsync("image/png", 300, 300);
                using var stream = resizedImageFile.OpenReadStream(maxAllowedSize: 1024 * 1024 * 5); // 5 MB max
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var bytes = memoryStream.ToArray();
                previewImageUrl = $"data:image/png;base64,{Convert.ToBase64String(bytes)}";
                profileIage = new Base64ImageFile(e.File.Name, previewImageUrl);
            }
        }

        private async Task RegisterUser()
        {
            try
            {
                Loader.Show();

                AuthToken token = await petAPI.RegisterUserAsync(new RegisterUser(model.Name, model.Email, model.Password, model.PhoneNumber, model.Gender, profileIage));

                if (token is null || string.IsNullOrEmpty(token.RefreshToken))
                {
                    Snackbar.Add($"{model.Email} not saved successfully! Please try again", Severity.Error);
                    return;
                }

                Snackbar.Add($"{model.Email} user created successfully", Severity.Success);

                var claims = new List<Claim>{ new Claim(ClaimTypes.Name, "userName")};
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await AuthState.MarkUserAsAuthenticated(token);
                _Naivigation.NavigateTo("/", true);
            }
            catch (Exception ex)
            {
                  Loader.Hide();
                Snackbar.Add("Something went wrong!", Severity.Error);
            }
            finally
            {
                 Loader.Hide();
            }
        }
    }
}
