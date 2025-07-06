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
            }
        }

        private async Task RegisterUser()
        {
            HttpResponseMessage? response = null;
            try
            {
             //   PreloadService.Show();

               // Base64ImageFile profileImage = null;

                //if (ProfileFile != null)
                //{
                //    var fileStream = ProfileFile.OpenReadStream(10 * 1024 * 1024); // max 10MB
                //    var streamContent = new StreamContent(fileStream);

                //    using var ms = new MemoryStream();
                //    await fileStream.CopyToAsync(ms);
                //    var bytes = ms.ToArray();

                //    var base64 = Convert.ToBase64String(bytes);

                //    var base64WithPrefix = $"data:{ProfileFile.ContentType};base64,{base64}";

                //    profileImage = new Base64ImageFile(ProfileFile.Name, base64WithPrefix);
                //}

                AuthToken token = await petAPI.RegisterUserAsync(new RegisterUser(model.Name, model.Email, model.Password, model.PhoneNumber, model.Gender, previewImageUrl));

                if (token is null || string.IsNullOrEmpty(token.RefreshToken))
                {
                   // ToastService.Notify(new ToastMessage(ToastType.Danger, $"{register.Name} not saved successfully! Please try again"));
                    return;
                }

              //  ToastService.Notify(new ToastMessage(ToastType.Success, $"{register.Name} user created successfully"));

                var claims = new List<Claim>{ new Claim(ClaimTypes.Name, "userName")};
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await AuthState.MarkUserAsAuthenticated(token);
                _Naivigation.NavigateTo("/", true);
            }
            catch (Exception ex)
            {
              //  PreloadService.Hide();
              //  ToastService.Notify(new ToastMessage(ToastType.Danger, $"Something went wrong!"));
            }
            finally
            {
               // PreloadService.Hide();
            }
        }
    }
}
