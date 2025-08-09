using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.APIModels;
using System.Security.Claims;

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
        private HubConnection? hubConnection;

        #region methods

        private async Task OnValueChanged(int value)
        {
            model.SpeciesId = value;
            model.PetList = await petAPI.GetPetDropdownAsync((Species)value);

            if (model.PetList is null)
                model.PetList = new List<DropDownModal>();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                hubConnection = new HubConnectionBuilder()
                          .WithUrl(_Naivigation.ToAbsoluteUri("/requestHub"))
                          .Build();
                await hubConnection.StartAsync();

                Loader.Show();
                if (EditModel != null && EditModel.Id > 0)
                {
                    model = EditModel;
                }


                Loader.Hide();

                await base.OnInitializedAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task Send(string userId, string userName, int requestId, string petName = "")
        {
            if (hubConnection is not null)
            {
                await hubConnection.SendAsync("SendRequest", userId, userName, requestId, petName);
            }
        }
        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
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
                string? userName = "";
                PetRequestModel? apiResult = null;

                if (user.Identity != null && user.Identity.IsAuthenticated)
                {
                    userId = user.FindFirst("Id")?.Value;
                    userName = user.FindFirst(ClaimTypes.Name)?.Value;

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

                // send notification to owner of pet
                await Send(userId, userName, apiResult.Id, model.PetList.FirstOrDefault(x => x.Id == model.PetId)?.Name);

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
