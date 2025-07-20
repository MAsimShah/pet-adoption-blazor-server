using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.APIModels;
using PetAdoption.UI.Components.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace PetAdoption.UI.Components.Pages.Pets
{
    public partial class AdddPetModal
    {
        [CascadingParameter]
        private IMudDialogInstance MudDialog { get; set; }

        [Parameter]
        public PetViewModel EditModel { get; set; } = new();

        public List<IBrowserFile> UploadedImages { get; set; } = new List<IBrowserFile>();
        private PetViewModel model = new PetViewModel();


        #region methods

        protected override Task OnInitializedAsync()
        {
            if (EditModel != null && EditModel.Id > 0)
            {
                model = EditModel;
            }

            return base.OnInitializedAsync();
        }

        private void Cancel() => MudDialog.Cancel();

        private async Task UploadProfilePhoto(InputFileChangeEventArgs e)
        {
            UploadedImages = e.GetMultipleFiles().ToList();
        }

        private async Task SavePet()
        {

            PetViewModel response = null;
            try
            {
                Loader.Show();

                if (model is null)
                {
                    Snackbar.Add($"Please fill all the required fields", Severity.Error);
                    return;
                }

                PetModel apiResult = new(model);

                if (apiResult.Id > 0)
                    apiResult = await petAPI.UpdatePetAsync(apiResult);
                else
                    apiResult = await petAPI.AddPetAsync(apiResult);

                if (apiResult is null)
                {
                    Snackbar.Add($"{model.Name} not saved successfully! Please try again", Severity.Error);
                    return;
                }

                if (UploadedImages != null && UploadedImages.Any())
                {
                    List<Base64ImageFile> images = new List<Base64ImageFile>();

                    foreach (var file in UploadedImages)
                    {
                        var fileStream = file.OpenReadStream(10 * 1024 * 1024); // max 10MB
                        var streamContent = new StreamContent(fileStream);

                        using var ms = new MemoryStream();
                        await fileStream.CopyToAsync(ms);
                        var bytes = ms.ToArray();

                        var base64 = Convert.ToBase64String(bytes);

                        var base64WithPrefix = $"data:{file.ContentType};base64,{base64}";

                        images.Add(new Base64ImageFile(file.Name, base64WithPrefix));
                    }

                    Base64UploadRequest payload = new Base64UploadRequest() { PetId = apiResult.Id, Images = images };

                    var json = JsonSerializer.Serialize(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var files = await petAPI.UploadedPetFilesAsync(payload);

                    if (files is null || !files.Any()) 
                    {
                        Snackbar.Add($"Uploaded files not saved successfully!", Severity.Error);
                        return;
                    }
                }

                Snackbar.Add($"{model.Name} is saved successfully", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Not able to save info of Pet.", Severity.Error);
                Loader.Hide();
            }
            finally
            {
                Loader.Hide();
                MudDialog.Close(DialogResult.Ok(true));
            }
        }

        #endregion methods
    }
}
