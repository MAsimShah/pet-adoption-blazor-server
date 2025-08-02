using MudBlazor;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.APIModels;
using PetAdoption.UI.Components.Pages.Pets;

namespace PetAdoption.UI.Components.Pages.Admin.Users
{
    public partial class List
    {
        private List<UserViewModel> userList = new();
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

        async Task EditUser(string id)
        {
            Loader.Show();

            try
            {
                UserModel result = await petAPI.GetUserAsync(id);

                if (result is null)
                {
                    Snackbar.Add("Not fetched user info", Severity.Error);
                    return;
                }

                UserViewModel model = new()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Email = result.Email,
                    Password = result.Password,
                    PhoneNumber = result.PhoneNumber
                };

                //var parameters = new DialogParameters<AdddPetModal> { { x => x.EditModel, model } };
                //var dialog = await DialogService.ShowAsync<AdddPetModal>("Edit new Pet", parameters, dialogOptions);
                //var dialogResult = await dialog.Result;

                //if (dialogResult != null && !dialogResult.Canceled)
                //{
                //    await RefreshGrid();
                //}
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

        private async Task AddUserAsync()
        {
            var dialog = await DialogService.ShowAsync<AdddPetModal>("Add new User", dialogOptions);
            var result = await dialog.Result;

            if (result != null && !result.Canceled)
            {
                await RefreshGrid();
            }
        }

        private async Task DeleteUser(string id)
        {
            try
            {
                Loader.Show();
                await petAPI.DeleteUserAsync(id);

                // grid refresh with db calling
                var toRemove = userList.FirstOrDefault(p => p.Id == id);
                if (toRemove != null)
                {
                    userList = userList.Where(p => p.Id != id).ToList();
                }

                Snackbar.Add($"User info deleted successfully", Severity.Success);
                StateHasChanged();
            }
            catch
            {
                Loader.Hide();
                Snackbar.Add($"Not able to delete user info", Severity.Error);
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
                var result = await petAPI.GetAllUsersAsync();
                if (result is null || !result.Any()) userList = new List<UserViewModel>();

                if (result != null)
                {
                    userList = result.Select(x => new UserViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email
                    }).ToList();
                }

                StateHasChanged();
            }
            catch
            {
                Loader.Hide();
                Snackbar.Add($"Not fetched all users information", Severity.Error);
            }
            finally
            {
                Loader.Hide();
            }
        }

        #endregion methods
    }
}
