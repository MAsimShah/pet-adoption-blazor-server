using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.APIModels;
using PetAdoption.UI.Components.Models.DTOs;

namespace PetAdoption.UI.Components.Pages.Admin.Users
{
    public partial class AddUserModel
    {
        [CascadingParameter]
        private IMudDialogInstance MudDialog { get; set; }

        [Parameter]
        public UserViewModel EditModel { get; set; } = new();

        private string? previewImageUrl;
        private Base64ImageFile? profileIage;
        private MudFileUpload<IBrowserFile>? _fileElement;

        private UserViewModel model = new UserViewModel();


        protected override Task OnInitializedAsync()
        {
            if (EditModel != null && !string.IsNullOrEmpty(EditModel.Id))
            {
                previewImageUrl = Configuration["BlazorApiUrl"] + EditModel.ProfileImage;
                model = EditModel;
            }

            return base.OnInitializedAsync();
        }

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
                AuthToken token = null;

                if (string.IsNullOrEmpty(model.Id))
                {
                    token = await petAPI.RegisterUserAsync(new RegisterUserModel()
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        PhoneNumber = model.PhoneNumber,
                        ProfilePhoto = profileIage
                    });
                }
                else
                {
                    token = await petAPI.UpdateUserAsync(new UserModel()
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        PhoneNumber = model.PhoneNumber,
                        ProfilePhoto = profileIage
                    });
                }


                if (token is null || string.IsNullOrEmpty(token.RefreshToken))
                {
                    Snackbar.Add($"{model.Email} not saved successfully! Please try again", Severity.Error);
                    return;
                }

                Snackbar.Add($"{model.Email} user created successfully", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
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
