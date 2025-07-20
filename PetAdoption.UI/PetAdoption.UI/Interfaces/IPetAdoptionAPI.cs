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
        Task<PetModel> AddPetAsync([Body] PetModel model);

        [Put("/api/Pets/Update")]
        Task<PetModel> UpdatePetAsync([Body] PetModel model);

        [Post("/api/Pets/Upload-pet-files")]
        Task<List<string>> UploadedPetFilesAsync([Body] Base64UploadRequest model);

        [Get("/api/Pets/get-list")]
        Task<List<PetViewModel>> GetAllPetsAsync();

        [Get("/api/Pets/Get/{petId}")]
        Task<PetModel> GetPetAsync(int petId);

        [Delete("/api/Pets/Delete/{id}")]
        Task DeletePetAsync(int id);

        [Delete("/api/Pets/DeletePhoto/{id}")]
        Task DeletePetPhotoAsync(int id);

        #endregion Pets
    }
}
