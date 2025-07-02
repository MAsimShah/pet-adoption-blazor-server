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
    }
}
