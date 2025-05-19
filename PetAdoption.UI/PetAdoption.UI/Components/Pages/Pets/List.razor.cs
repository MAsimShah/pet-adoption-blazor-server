using BlazorBootstrap;
using PetAdoption.UI.Components.Models;

namespace PetAdoption.UI.Components.Pages.Pets
{
    public partial class List
    {
        private List<PetViewModel> pets = new()
    {
        new PetViewModel { Name = "Buddy", Age = 12 },
        new PetViewModel { Name = "Luna", Age = 09},
        new PetViewModel { Name = "Max", Age = 56 }
    };

        private Modal modal;
        private PetViewModel petViewModel = new PetViewModel();

        private async Task OnShowModal()
        {
            await modal.ShowAsync();
        }

        private async Task OnHideModal()
        {
            await modal.HideAsync();
        }

        private void HandleValidSubmit()
        {
          //  var response = await Http.PostAsJsonAsync("AddPet", petViewModel);
            var response = petViewModel;

            //if (response.IsSuccessStatusCode)
            //{
            //    // Optionally show a success message, close modal, etc.
            //    Console.WriteLine("Pet added successfully.");
            //    modal.Hide();
            //}
            //else
            //{
            //    // Handle errors (e.g., log or show to user)
            //    Console.WriteLine("Error: " + response.ReasonPhrase);
            //}
        }
    }
}
