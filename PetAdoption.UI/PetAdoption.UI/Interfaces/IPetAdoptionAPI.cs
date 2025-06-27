using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Models.DTOs;
using PetAdoption.UI.Components.Pages.Auth;
using Refit;
using static PetAdoption.UI.Components.Pages.Auth.Signup;

namespace PetAdoption.UI.Interfaces
{
    public interface IPetAdoptionAPI
    {
        [Post("/api/Auth/Register")]
        Task<AuthToken> RegisterUserAsync([Body] RegisterDto model);
    }
}
