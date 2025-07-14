using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.APIModels;
using PetAdoption.UI.Components.Models.DTOs;
using Refit;

namespace PetAdoption.UI.Interfaces
{
    public interface IPetAdoptionAPI
    {
        [Post("/api/Auth/Register")]
        Task<AuthToken> RegisterUserAsync([Body] RegisterUser model);

        [Post("/api/Auth/Login")]
        Task<AuthToken> LoginUserAsync([Body] LoginViewModel model);

        [Post("/api/Auth/RefreshToken")]
        Task<AuthToken> RefreshTokenAsync([Body] string token);

        [Post("/api/Pets/Add")]
        Task<PetViewModel> AddPetAsync([Body] PetViewModel model);

        [Post("/api/Pets/Update")]
        Task<PetViewModel> UpdatePetAsync([Body] PetViewModel model);

        [Post("/api/Pets/Upload-pet-files")]
        Task<Task> UploadedPetFilesAsync([Body] Base64UploadRequest model);
    }
}
