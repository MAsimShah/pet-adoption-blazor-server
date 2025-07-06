using Microsoft.AspNetCore.Components.Forms;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace PetAdoption.UI.Components.Pages.Pets
{

    public class Base64UploadRequest
    {
        public int PetId { get; set; }
        public List<Base64ImageFile> Images { get; set; }
    }
    public partial class List
    {
        private List<PetViewModel> petsList = new();
        private PetViewModel petViewModel = new PetViewModel();
        private List<IBrowserFile> loadedFiles = [];
       // private Modal modal = new Modal();
        private bool isLoading = false;
        public IBrowserFile[] UploadedImages { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await RefreshGrid();
        }

        private async Task OnShowModal()
        {
            petViewModel = new PetViewModel();
            EditContext editContext = new EditContext(petViewModel);
        //    await modal.ShowAsync();
        }

        private async Task OnHideModal()
        {
           // await modal.HideAsync();
        }

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            try
            {
               // PreloadService.Show();
                loadedFiles.Clear();

                var files = e.GetMultipleFiles();
                UploadedImages = files.ToArray();
            }
            catch
            {
               // PreloadService.Hide();
              //  ToastService.Notify(new ToastMessage(ToastType.Danger, $"Something went wrong!"));
            }
            finally
            {
              //  PreloadService.Hide();
            }
        }

        private async Task DeletePetPhoto(int photoId)
        {
            HttpResponseMessage? response = null;
            try
            {
              //  PreloadService.Show();
                response = await Http.DeleteAsync($"/api/Pets/DeletePhoto/{photoId}");

                // grid refresh with db calling
                var petToRemove = petViewModel.PetPhotos.FirstOrDefault(p => p.Id == photoId);
                if (petToRemove != null)
                {
                    petViewModel.PetPhotos = petViewModel.PetPhotos.Where(p => p.Id != photoId).ToList();
                }

                // render UI
                StateHasChanged();
            }
            catch
            {
              //  PreloadService.Hide();
              //  ToastService.Notify(new ToastMessage(ToastType.Danger, $"Something went wrong"));
            }
            finally
            {
               // PreloadService.Hide();
              //  if (response is null || !response.IsSuccessStatusCode) ToastService.Notify(new ToastMessage(ToastType.Danger, $"Something went wrong"));
              //  else ToastService.Notify(new ToastMessage(ToastType.Success, IconName.Bug, "Success", $"Deleted Successfully"));
            }
        }

        private async Task SavePet()
        {
            HttpResponseMessage? response = null;
            try
            {
              //  PreloadService.Show();

                if (petViewModel.Id > 0)
                     response = await Http.PutAsJsonAsync("/api/Pets/Update", petViewModel);
                else
                    response = await Http.PostAsJsonAsync("/api/Pets/Add", petViewModel);

                if (!response.IsSuccessStatusCode)
                {
                  //  ToastService.Notify(new ToastMessage(ToastType.Danger, $"{petViewModel.Name} not saved successfully! Please try again"));
                    return;
                }

                petViewModel = await response.Content.ReadFromJsonAsync<PetViewModel>();
                if (petViewModel != null && UploadedImages != null && UploadedImages.Any())
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

                    Base64UploadRequest payload = new Base64UploadRequest() { PetId = petViewModel.Id, Images = images };

                    var json = JsonSerializer.Serialize(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var filesResponse = await Http.PostAsJsonAsync("/api/Pets/Upload-pet-files", payload);

                    //if (!filesResponse.IsSuccessStatusCode) ToastService.Notify(new ToastMessage(ToastType.Danger, $"Files not saved successfully!"));

                   // ToastService.Notify(new ToastMessage(ToastType.Success, $"{petViewModel.Name} is saved successfully"));
                }
            }
            catch (Exception ex)
            {
              //  ToastService.Notify(new ToastMessage(ToastType.Danger, $"Something went wrong!"));
              //  PreloadService.Hide();
            }
            finally
            {
               // PreloadService.Hide();
              //  await modal.HideAsync();
                await RefreshGrid();
            }
        }

        private string GetHealthStatusBadgeClass(HealthStatus status)
        {
            return status switch
            {
                HealthStatus.GoodHealth => "bg-success",
                HealthStatus.Vaccinated => "bg-primary",
                HealthStatus.SpecialMedicalNeeds => "bg-warning",
                HealthStatus.UnderTreatment => "bg-danger",
                _ => "bg-secondary"
            };
        }

        private void ViewPet(PetViewModel pet)
        {
            // View pet details logic
        }

        private async Task EditPet(int petId)
        {
            HttpResponseMessage? response = null;

            try
            {
               // PreloadService.Show();
                response = await Http.GetAsync($"/api/Pets/Get/{petId}");

                if (response.IsSuccessStatusCode)
                {
                    petViewModel = await response.Content.ReadFromJsonAsync<PetViewModel>();
                    EditContext editContext = new EditContext(petViewModel);
                 //   await modal.ShowAsync();
                }

            }
            catch
            {
               // ToastService.Notify(new ToastMessage(ToastType.Danger, $"Something went wrong"));
            }
            finally
            {
              //  PreloadService.Hide();
            }
        }

        private async Task DeletePet(int petId)
        {
            HttpResponseMessage? response = null;
            try
            {
               // PreloadService.Show();
                response = await Http.DeleteAsync($"/api/Pets/Delete/{petId}");

                // grid refresh with db calling
                var petToRemove = petsList.FirstOrDefault(p => p.Id == petId);
                if (petToRemove != null)
                {
                    petsList = petsList.Where(p => p.Id != petId).ToList();
                }

                // render UI
                StateHasChanged();
            }
            catch
            {
             //   PreloadService.Hide();
             //   ToastService.Notify(new ToastMessage(ToastType.Danger, $"Something went wrong"));
            }
            finally
            {
               // PreloadService.Hide();
               // if (response is null || !response.IsSuccessStatusCode) ToastService.Notify(new ToastMessage(ToastType.Danger, $"Something went wrong"));
              //  else ToastService.Notify(new ToastMessage(ToastType.Success, IconName.Bug, "Success", $"Deleted Successfully"));
            }
        }

        private async Task RefreshGrid()
        {
          //  PreloadService.Show();
            try
            {
                var response = await Http.GetAsync("/api/Pets/get-list");
               // if (!response.IsSuccessStatusCode) ToastService.Notify(new ToastMessage(ToastType.Danger, $"Something went wrong"));

                petsList = await response.Content.ReadFromJsonAsync<List<PetViewModel>>();
                isLoading = true;
            }
            catch
            {
               // PreloadService.Hide();
              //  ToastService.Notify(new ToastMessage(ToastType.Danger, $"Something went wrong"));
            }
            finally
            {
             //   PreloadService.Hide();
            }
        }
    }
}