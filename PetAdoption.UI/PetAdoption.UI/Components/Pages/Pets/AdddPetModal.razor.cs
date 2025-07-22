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
        private string? apiUrl = "";


        #region methods

        protected override Task OnInitializedAsync()
        {
            apiUrl = Configuration["BlazorApiUrl"];
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
            try
            {
                Loader.Show();

                if (model is null)
                {
                    Snackbar.Add($"Please fill all the required fields", Severity.Error);
                    return;
                }

                PetModel apiResult = new()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Breed = model.Breed,
                    Age = model.Age,
                    ContactInformation = model.ContactInformation,
                    Species = model.Species,
                    AdoptableSince = model.AdoptableSince,
                    AdoptionFee = model.AdoptionFee,
                    Color = model.Color,
                    Description = model.Description,
                    Gender = model.Gender,
                    GoodWithKids = model.GoodWithKids,
                    GoodWithOtherPets = model.GoodWithOtherPets,
                    HealthStatus = model.HealthStatus,
                    Location = model.Location,
                    Microchipped = model.Microchipped
                };

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
                MudDialog.Close(DialogResult.Ok(true));
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Not able to save info of Pet.", Severity.Error);
                Loader.Hide();
            }
            finally
            {
                Loader.Hide();
            }
        }

        private async Task DeletePhoto(int id)
        {
            try
            {
                Loader.Show();
                await petAPI.DeletePetPhotoAsync(id);

                // grid refresh with db calling
                var petToRemove = model.PetPhotos.FirstOrDefault(p => p.Id == id);
                if (petToRemove != null)
                {
                    model.PetPhotos = model.PetPhotos.Where(p => p.Id != id).ToList();
                }

                // render UI
                StateHasChanged();
            }
            catch
            {
                Loader.Hide();
                Snackbar.Add($"Something went wrong", Severity.Error);
            }
            finally
            {
                Loader.Hide();
            }
        }

        #endregion methods
    }
}
