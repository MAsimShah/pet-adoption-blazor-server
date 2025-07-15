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

        #region Pets

        [Post("/api/Pets/Add")]
        Task<PetViewModel> AddPetAsync([Body] PetViewModel model);

        [Post("/api/Pets/Update")]
        Task<PetViewModel> UpdatePetAsync([Body] PetViewModel model);

        [Post("/api/Pets/Upload-pet-files")]
        Task<Task> UploadedPetFilesAsync([Body] Base64UploadRequest model);

        [Get("/api/Pets/get-list")]
        Task<List<PetViewModel>> GetAllPetsAsync();

        [Delete("/api/Pets/Delete/{petId}")]
        Task<Task> DeletePetAsync(int petId);

        [Delete("/api/Pets/DeletePhoto/{photoId}")]
        Task<Task> DeletePetPhotoAsync(int photoId);

        #endregion Pets
    }
}
