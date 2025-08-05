using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.APIModels;
using PetAdoption.UI.Components.Models.DTOs;
using Refit;

namespace PetAdoption.UI.Interfaces
{
    public interface IPetAdoptionAPI
    {
        [Post("/api/Auth/Register")]
        Task<AuthToken> RegisterUserAsync([Body] RegisterUserModel model);

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

        [Get("/api/Pets/dropdown")]
        Task<List<DropDownModal>> GetPetDropdownAsync();

        [Get("/api/Pets/Get/{petId}")]
        Task<PetModel> GetPetAsync(int petId);

        [Delete("/api/Pets/Delete/{id}")]
        Task DeletePetAsync(int id);

        [Delete("/api/Pets/DeletePhoto/{id}")]
        Task DeletePetPhotoAsync(int id);

        #endregion Pets

        #region Pet's Requests

        [Get("/api/Requests/get-list")]
        Task<List<PetRequestModel>> GetAllRequestsAsync();

        [Get("/api/Requests/Get/{id}")]
        Task<PetRequestModel> GetRequestAsync(int id);

        [Post("/api/Requests/Add")]
        Task<PetRequestModel> AddRequestAsync([Body] PetRequestModel model);

        [Put("/api/Requests/Update")]
        Task<PetRequestModel> UpdateRequestAsync([Body] PetRequestModel model);

        [Delete("/api/Requests/Delete/{id}")]
        Task DeleteRequestAsync(int id);

        #endregion Pet's Requests

        #region Users

        [Get("/api/Auth/GetAllUsers")]
        Task<List<UserModel>> GetAllUsersAsync();

        [Get("/api/Auth/Get/{id}")]
        Task<UserModel> GetUserAsync(string id);


        [Put("/api/Auth/Update")]
        Task<AuthToken> UpdateUserAsync([Body] UserModel model);

        [Delete("/api/Auth/Delete/{id}")]
        Task DeleteUserAsync(string id);


        #endregion Users

    }
}
