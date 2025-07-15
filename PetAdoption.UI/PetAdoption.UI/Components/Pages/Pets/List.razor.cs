using MudBlazor;
using PetAdoption.UI.Components.Models;

namespace PetAdoption.UI.Components.Pages.Pets
{
    public partial class List
    {
        private List<PetViewModel> petsList = new();
        private List<string> _events = new();
        DialogOptions dialogOptions = new DialogOptions
        {
            BackgroundClass = "my-custom-class",
            MaxWidth = MaxWidth.ExtraLarge,
            Position = DialogPosition.Center,
            FullWidth = true,
        };

        #region lifecycles

        protected override async Task OnInitializedAsync()
        {
            await RefreshGrid();
        }

        #endregion lifecycles

        #region methods

        void StartedEditingItem(PetViewModel item)
        {
            _events.Insert(0, $"Event = StartedEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        void CanceledEditingItem(PetViewModel item)
        {
            _events.Insert(0, $"Event = CanceledEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        void CommittedItemChanges(PetViewModel item)
        {
            _events.Insert(0, $"Event = CommittedItemChanges, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
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

        //private async Task DeletePetPhoto(int photoId)
        //{
        //    try
        //    {
        //        Loader.Show();
        //        var response = await petAPI.DeletePetPhotoAsync(photoId);

        //        // grid refresh with db calling
        //        var petToRemove = petsList.PetPhotos.FirstOrDefault(p => p.Id == photoId);
        //        if (petToRemove != null)
        //        {
        //            petViewModel.PetPhotos = petViewModel.PetPhotos.Where(p => p.Id != photoId).ToList();
        //        }

        //        // render UI
        //        StateHasChanged();
        //    }
        //    catch
        //    {
        //        Loader.Hide();
        //        Snackbar.Add($"Something went wrong", Severity.Error);
        //    }
        //    finally
        //    {
        //        Loader.Hide();
        //    }
        //}

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

        //private async Task EditPet(int petId)
        //{
        //    HttpResponseMessage? response = null;

        //    try
        //    {
        //        Loader.Show();
        //        response = await Http.GetAsync($"/api/Pets/Get/{petId}");

        //        if (response.IsSuccessStatusCode)
        //        {
        //            // petViewModel = await response.Content.ReadFromJsonAsync<PetViewModel>();
        //            // EditContext editContext = new EditContext(petViewModel);
        //            //   await modal.ShowAsync();
        //        }

        //        //var parameters = new DialogParameters<AdddPetModal> { { x => x.EditModel, new PetViewModel() } };
        //        //var dialog = await DialogService.ShowAsync<AdddPetModal>("Add new Pet", dialogOptions);
        //        //var result = await dialog.Result;

        //        //if (!result.Canceled)
        //        //{
        //        //    var tt = "";
        //        //}

        //    }
        //    catch
        //    {
        //        Snackbar.Add($"Something went wrong", Severity.Error);
        //    }
        //    finally
        //    {
        //        Loader.Hide();
        //    }
        //}

        private async Task DeletePet(int petId)
        {
            try
            {
                Loader.Show();
                var response = await petAPI.DeletePetAsync(petId);

                // grid refresh with db calling
                var petToRemove = petsList.FirstOrDefault(p => p.Id == petId);
                if (petToRemove != null)
                {
                    petsList = petsList.Where(p => p.Id != petId).ToList();
                }

                StateHasChanged();
            }
            catch
            {
                Loader.Hide();
                Snackbar.Add($"Not able to delete pet info", Severity.Error);
            }
            finally
            {
                Loader.Hide();
            }
        }

        private async Task RefreshGrid()
        {
            Loader.Show();
            try
            {
                var result = await petAPI.GetAllPetsAsync();

                petsList = result is null || !result.Any() ? new List<PetViewModel>() : result;
                StateHasChanged();
            }
            catch
            {
                Loader.Hide();
                Snackbar.Add($"Not fetched all pets information", Severity.Error);
            }
            finally
            {
                Loader.Hide();
            }
        }

        #endregion methods
    }
}