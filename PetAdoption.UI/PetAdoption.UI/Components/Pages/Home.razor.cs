using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using PetAdoption.UI.Components.Models.APIModels;

namespace PetAdoption.UI.Components.Pages
{
    public partial class Home
    {
        DashboardStatsModel dashboardStats = new();
        private bool IsAdmin { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            // show above count section
            await ShowStats();

            // check user is admin some features show/hide
            await CheckUserIsAdmin();



            await base.OnInitializedAsync();
        }

        private async Task CheckUserIsAdmin()
        {
            var authState = await AuthState.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                IsAdmin = user?.IsInRole("Admin") ?? false;
                StateHasChanged();
            }
        }

        private async Task ShowStats()
        {
            try
            {
                Loader.Show();
                dashboardStats = await petAPI.GetStatsAsync();

                StateHasChanged();
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
    }
}