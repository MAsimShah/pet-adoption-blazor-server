using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.APIModels;
using PetAdoption.UI.Components.Pages.Admin.Users;
using PetAdoption.UI.Components.Pages.PetRequests;

namespace PetAdoption.UI.Components.Layout
{
    public class RequestMessages
    {
        public int RequestId { get; set; }
        public string Message { get; set; }
    }

    public partial class NavMenu
    {
        List<PetRequestModel> Requests = new();
        private string ProfileImage { get; set; }
        private bool IsAdmin { get; set; }
        private HubConnection? hubConnection;

        public bool IsConnected =>
    hubConnection?.State == HubConnectionState.Connected;

        DialogOptions dialogOptions = new DialogOptions
        {
            BackgroundClass = "my-custom-class",
            MaxWidth = MaxWidth.ExtraLarge,
            Position = DialogPosition.Center,
            FullWidth = true,
        };

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthState.GetAuthenticationStateAsync();
            var user = authState.User;
            string loggedInUserId = user?.FindFirst("Id")?.Value ?? "";

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var imageUrl = user.FindFirst("ProfileImage")?.Value;

                if (string.IsNullOrEmpty(imageUrl))
                {
                    ProfileImage = Configuration["DefaultProfileImage"];
                }
                else
                {
                    ProfileImage = Configuration["BlazorApiUrl"] + imageUrl;
                }

                IsAdmin = user?.IsInRole("Admin") ?? false;
            }

            hubConnection = new HubConnectionBuilder()
                                .WithUrl(_Naivigation.ToAbsoluteUri("/requestHub"))
                                .Build();

            hubConnection.On<string, string, int, string>("PetRequest", (userId, userName, requestId, petName) =>
            {
                if (!loggedInUserId.Equals(userId))
                {
                    var encodedMsg = $"{userName} request for this pet {petName}";
                    Requests.Add(new PetRequestModel() { Id = requestId, PetName = encodedMsg });
                    InvokeAsync(StateHasChanged);
                }
            });

            await hubConnection.StartAsync();

            await GetUserRequests();
        }

        private async Task GetUserRequests()
        {
            try
            {
                Loader.Show();
                Requests = await petAPI.GetAllUserRequestsAsync();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                Loader.Hide();
                var errorMessage = ((Refit.ApiException)ex).Content ?? $"Status not Updated Successfully.";
                Snackbar.Add(errorMessage, Severity.Error);
            }
            finally
            {
                Loader.Hide();
            }
        }

        private async Task UpdateStatus(int requestId, RequestStatus status)
        {
            Loader.Show();

            try
            {
                var apiModel = await petAPI.UpdateRequestStatusAsync(requestId, status);

                if (apiModel != null)
                {
                    Snackbar.Add("Status Updated Successfully.", Severity.Success);
                }
            }
            catch (Exception ex)
            {
                Loader.Hide();
                var errorMessage = ((Refit.ApiException)ex).Content ?? $"Status not Updated Successfully.";
                Snackbar.Add(errorMessage, Severity.Error);
            }
            finally
            {
                Loader.Hide();
            }
        }

        private void LoginPage()
        {
            _Naivigation.NavigateTo("/", true);
        }

        private async Task Logout()
        {
            await AuthState.MarkUserAsLoggedOut();
            _Naivigation.NavigateTo("/", true);
        }

        async Task EditUser()
        {
            Loader.Show();

            try
            {
                AuthenticationState? authUser = await AuthState.GetAuthenticationStateAsync();
                var user = authUser.User;
                string? userId = user.FindFirst("Id")?.Value;

                UserModel result = await petAPI.GetUserAsync(userId);

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
                    PhoneNumber = result.PhoneNumber,
                    ProfileImage = result.ProfileImage
                };

                Loader.Hide();
                var parameters = new DialogParameters<AddUserModel> { { x => x.EditModel, model } };
                var dialog = await DialogService.ShowAsync<AddUserModel>("Edit User", parameters, dialogOptions);
                var dialogResult = await dialog.Result;
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
    }
}
