using PetAdoption.UI.Components.Models;
using PetAdoption.UI.Components.Pages.Auth;
using Refit;
using static PetAdoption.UI.Components.Pages.Auth.Signup;

namespace PetAdoption.UI.Helpers
{
    public interface IPetAdoptionAPI
    {
        [Post("/api/Auth/Register")]
        Task<TokenResponse> RegisterUserAsync([Body] RegisterDto model);
    }
}
