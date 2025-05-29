using BlazorBootstrap;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using PetAdoption.UI.Components.Models;
using System.Net.Http.Headers;

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

        private List<IBrowserFile> loadedFiles = [];
        private long maxFileSize = 1024 * 15;
        private int maxAllowedFiles = 3;
        private bool isLoading;


        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            try
            {
                isLoading = true;
                loadedFiles.Clear();
                int petId = 2;

                var files = e.GetMultipleFiles();
                MultipartFormDataContent content = new MultipartFormDataContent();
                foreach (var file in files)
                {
                    var fileStream = file.OpenReadStream(10 * 1024 * 1024); // max 10MB
                    var streamContent = new StreamContent(fileStream);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    content.Add(streamContent, "Files", file.Name);
                }

                // Add petId
                content.Add(new StringContent(petId.ToString()), "PetId");

                var response = await Http.PostAsync("/api/Pets/upload-pet-files", content);

                isLoading = false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task HandleValidSubmit()
        {
            var response = await Http.PostAsJsonAsync("AddPet", petViewModel);

            if (response.IsSuccessStatusCode)
            {
                // Optionally show a success message, close modal, etc.
                Console.WriteLine("Pet added successfully.");
                modal.Hide();
            }
            else
            {
                // Handle errors (e.g., log or show to user)
                Console.WriteLine("Error: " + response.ReasonPhrase);
            }
        }
    }
}
