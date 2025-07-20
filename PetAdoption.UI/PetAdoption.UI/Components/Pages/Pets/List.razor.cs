using MudBlazor;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.APIModels;

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

        #region lifecycles

        protected override async Task OnInitializedAsync()
        {
            await RefreshGrid();
        }

        #endregion lifecycles

        #region methods

        async Task EditPet(int petId)
        {
            Loader.Show();

            try
            {
                var result = await petAPI.GetPetAsync(petId);

                if (result is null)
                {
                    Snackbar.Add("Not fetched pet info", Severity.Error);
                    return;
                }

                PetViewModel model = new(result);

                var parameters = new DialogParameters<AdddPetModal> { { x => x.EditModel, model } };
                var dialog = await DialogService.ShowAsync<AdddPetModal>("Edit new Pet", parameters, dialogOptions);
                var dialogResult = await dialog.Result;

                if (dialogResult != null && !dialogResult.Canceled)
                {
                    await RefreshGrid();
                }
            }
            catch (Exception)
            {
                Loader.Hide();
                Snackbar.Add("Not fetched pet info", Severity.Error);
            }
            finally
            {
                Loader.Hide();
            }
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

        private async Task DeletePet(int id)
        {
            try
            {
                Loader.Show();
                await petAPI.DeletePetAsync(id);

                // grid refresh with db calling
                var petToRemove = petsList.FirstOrDefault(p => p.Id == id);
                if (petToRemove != null)
                {
                    petsList = petsList.Where(p => p.Id != id).ToList();
                }

                Snackbar.Add($"Pet info deleted successfully", Severity.Success);
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