using Microsoft.AspNetCore.Components;
using MudBlazor;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.APIModels;

namespace PetAdoption.UI.Components.Pages.PetRequests
{
    public partial class AddRequestModal
    {
        [CascadingParameter]
        private IMudDialogInstance MudDialog { get; set; }

        [Parameter]
        public PetRequestViewModel EditModel { get; set; } = new();

        private PetRequestViewModel model = new PetRequestViewModel();

        public List<DropDownModal> PetList { get; set; } = new List<DropDownModal>();

        #region methods

        private async Task OnValueChanged(int value)
        {
            model.SpeciesId = value;
            model.PetList = await petAPI.GetPetDropdownAsync();
            //StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (EditModel != null && EditModel.Id > 0)
                {
                    model = EditModel;
                }

                Loader.Show();

                //model.PetList = await petAPI.GetPetDropdownAsync();
                //model.PetIdt = new DropDownModal()
                //{
                //    Id = 2
                //};

                Loader.Hide();
               // StateHasChanged();

                await base.OnInitializedAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Cancel() => MudDialog.Cancel();

        private async Task SaveRequest()
        {
            try
            {
                Loader.Show();

                if (model is null)
                {
                    Snackbar.Add($"Please fill all the required fields", Severity.Error);
                    return;
                }

                var authUser = await AuthState.GetAuthenticationStateAsync();
                var user = authUser.User;
                string? userId = "";
                PetRequestModel? apiResult = null;

                if (user.Identity != null && user.Identity.IsAuthenticated)
                {
                    userId = user.FindFirst("Id")?.Value;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        apiResult = new()
                        {
                            Id = model.Id,
                            PetId = model.PetId,
                            UserId = userId,
                            RequestDate = model.RequestDate,
                            Message = model.Message,
                            Status = model.Status
                        };

                        if (apiResult.Id > 0)
                            apiResult = await petAPI.UpdateRequestAsync(apiResult);
                        else
                            apiResult = await petAPI.AddRequestAsync(apiResult);
                    }
                }

                if (apiResult is null)
                {
                    Snackbar.Add($"Request not saved successfully! Please try again!", Severity.Error);
                    return;
                }

                Snackbar.Add($"Request is saved successfully.", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Not able to save Request.", Severity.Error);
                Loader.Hide();
            }
            finally
            {
                Loader.Hide();
            }
        }

        #endregion methods
    }
}
