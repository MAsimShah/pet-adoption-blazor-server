using PetAdoption.UI.Components.Models.DTOs;

namespace PetAdoption.UI.Components.Models.APIModels
{
    public class RegisterUserModel
    {
        public string Name { get; set; } 
        public string Email { get; set; }
        public string Password { get; set; } 
        public string PhoneNumber { get; set; }
        public Base64ImageFile? ProfilePhoto { get; set; }
    }

    public record TokenResponse(string AccessToken, string RefreshToken);
}
