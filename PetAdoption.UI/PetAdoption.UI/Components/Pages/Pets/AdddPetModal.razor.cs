using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting.Server;
using MudBlazor;
using PetAdoption.UI.Components.Models;
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

        private void Submit() => MudDialog.Close(DialogResult.Ok(true));

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

                if (model.Id > 0)
                    model = await petAPI.UpdatePetAsync(model);
                else
                    model = await petAPI.AddPetAsync(model);

                if (model is null)
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

                    Base64UploadRequest payload = new Base64UploadRequest() { PetId = model.Id, Images = images };

                    var json = JsonSerializer.Serialize(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var filesResponse = await petAPI.UploadedPetFilesAsync(payload);

                    if (filesResponse is null) Snackbar.Add($"Files not saved successfully!", Severity.Error);
                    else Snackbar.Add($"{model.Name} is saved successfully", Severity.Success);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Something went wrong!", Severity.Error);
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
