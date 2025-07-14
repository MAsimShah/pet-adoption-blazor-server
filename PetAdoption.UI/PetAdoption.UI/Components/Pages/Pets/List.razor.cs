using MudBlazor;
using PetAdoption.UI.Components.Models;

namespace PetAdoption.UI.Components.Pages.Pets
{
    public partial class List
    {
        private List<PetViewModel> petsList = new();
        DialogOptions dialogOptions = new DialogOptions
        {
            BackgroundClass = "my-custom-class",
            MaxWidth = MaxWidth.ExtraLarge,
            Position = DialogPosition.Center,
            FullWidth = true,
        };


        protected override async Task OnInitializedAsync()
        {
            await RefreshGrid();
        }

        private async Task AddPetAsync()
        {
            var dialog = await DialogService.ShowAsync<AdddPetModal>("Add new Pet", dialogOptions);
            var result = await dialog.Result;

            if (result != null && !result.Canceled)
            {
                await RefreshGrid();
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
                //var petToRemove = petViewModel.PetPhotos.FirstOrDefault(p => p.Id == photoId);
                //if (petToRemove != null)
                //{
                //  //  petViewModel.PetPhotos = petViewModel.PetPhotos.Where(p => p.Id != photoId).ToList();
                //}

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
                   // petViewModel = await response.Content.ReadFromJsonAsync<PetViewModel>();
                   // EditContext editContext = new EditContext(petViewModel);
                 //   await modal.ShowAsync();
                }

                //var parameters = new DialogParameters<AdddPetModal> { { x => x.EditModel, new PetViewModel() } };
                //var dialog = await DialogService.ShowAsync<AdddPetModal>("Add new Pet", dialogOptions);
                //var result = await dialog.Result;

                //if (!result.Canceled)
                //{
                //    var tt = "";
                //}

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