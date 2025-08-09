using MudBlazor;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.APIModels;
using PetAdoption.UI.Components.Pages.Pets;

namespace PetAdoption.UI.Components.Pages.PetRequests
{
    public partial class List
    {
        private List<PetRequestViewModel> requestList = new();
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

        async Task EditRequest(int id)
        {
            Loader.Show();

            try
            {
                var result = await petAPI.GetRequestAsync(id);

                if (result is null)
                {
                    Snackbar.Add("Not fetched Request", Severity.Error);
                    return;
                }

                PetRequestViewModel model = new(result);

                var parameters = new DialogParameters<AddRequestModal> { { x => x.EditModel, model } };

                Loader.Hide();

                var dialog = await DialogService.ShowAsync<AddRequestModal>("Edit new Request", parameters, dialogOptions);
                var dialogResult = await dialog.Result;

                if (dialogResult != null && !dialogResult.Canceled)
                {
                    await RefreshGrid();
                }
            }
            catch (Exception ex)
            {
                Loader.Hide();
                var errorMessage = ((Refit.ApiException)ex).Content ?? "Something went wrong!";
                Snackbar.Add(errorMessage, Severity.Error);
            }
            finally
            {
                Loader.Hide();
            }
        }

        private async Task AddRequestAsync()
        {
            var dialog = await DialogService.ShowAsync<AddRequestModal>("Add new Request", dialogOptions);
            var result = await dialog.Result;

            if (result != null && !result.Canceled)
            {
                await RefreshGrid();
            }
        }

        private async Task DeleteRequest(int id)
        {
            try
            {
                Loader.Show();
                await petAPI.DeleteRequestAsync(id);

                // grid refresh with db calling
                var toRemove = requestList.FirstOrDefault(p => p.Id == id);
                if (toRemove != null)
                {
                    requestList = requestList.Where(p => p.Id != id).ToList();
                }

                Snackbar.Add($"Request deleted successfully", Severity.Success);
                StateHasChanged();
            }
            catch
            {
                Loader.Hide();
                Snackbar.Add($"Not able to delete Request.", Severity.Error);
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
                var result = await petAPI.GetAllRequestsAsync();

                result = result is null || !result.Any() ? new List<PetRequestModel>() : result;
                requestList = new List<PetRequestViewModel>();

                foreach (var entity in result)
                {
                    requestList.Add(new PetRequestViewModel()
                    {
                        Id = entity.Id,
                        PetId = entity.PetId ?? 0,
                        PetName = entity.PetName,
                        UserId = entity.UserId,
                        UserName = entity.UserName,
                        RequestDate = entity.RequestDate,
                        Message = entity.Message,
                        Status = entity.Status
                    });
                }

                StateHasChanged();
            }
            catch(Exception ex)
            {
                Loader.Hide();
                var errorMessage = ((Refit.ApiException)ex).Content ?? $"Not fetched all requests";
                Snackbar.Add(errorMessage, Severity.Error);
            }
            finally
            {
                Loader.Hide();
            }
        }

        #endregion methods
    }
}