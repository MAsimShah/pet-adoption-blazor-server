using PetAdoption.UI.Components.Models.DTOs;

namespace PetAdoption.UI.Components.Models.APIModels
{
    public record RegisterUser(string Name, string Email, string Password, string PhoneNumber, Gender Gender, string? ProfilePhoto);

    public record TokenResponse(string AccessToken, string RefreshToken);
}
