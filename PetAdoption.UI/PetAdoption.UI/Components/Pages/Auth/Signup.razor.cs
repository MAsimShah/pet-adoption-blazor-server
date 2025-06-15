using BlazorBootstrap;
using Microsoft.AspNetCore.Components.Forms;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.DTOs;
using PetAdoption.UI.Components.Pages.Pets;
using static System.Net.Mime.MediaTypeNames;

namespace PetAdoption.UI.Components.Pages.Auth
{
    public record RegisterDto(string Name, string Email, string Password, string PhoneNumber, Gender Gender, Base64ImageFile? ProfilePhoto);
    public record TokenResponse(string? RefreshToken, DateTime? RefreshTokenExpiryTime);

    public partial class Signup
    {
        private RegisterViewModel register = new RegisterViewModel();
        public IBrowserFile ProfileFile { get; set; }

        private void UploadProfilePhoto(InputFileChangeEventArgs e)
        {
            ProfileFile = e.File;
        }

        private async Task RegisterUser()
        {
            HttpResponseMessage? response = null;
            try
            {
                PreloadService.Show();

                Base64ImageFile profileImage = null;

                if (ProfileFile != null)
                {
                    var fileStream = ProfileFile.OpenReadStream(10 * 1024 * 1024); // max 10MB
                    var streamContent = new StreamContent(fileStream);

                    using var ms = new MemoryStream();
                    await fileStream.CopyToAsync(ms);
                    var bytes = ms.ToArray();

                    var base64 = Convert.ToBase64String(bytes);

                    var base64WithPrefix = $"data:{ProfileFile.ContentType};base64,{base64}";

                    profileImage = new Base64ImageFile(ProfileFile.Name, base64WithPrefix);
                }

                TokenResponse tokens = await petAPI.RegisterUserAsync(new RegisterDto(register.Name, register.Email, register.Password, register.PhoneNumber, register.Gender, profileImage));

                if (tokens != null)
                {
                    ToastService.Notify(new ToastMessage(ToastType.Danger, $"{register.Name} not saved successfully! Please try again"));
                    return;
                }

                ToastService.Notify(new ToastMessage(ToastType.Success, $"{register.Name} user created successfully"));

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
