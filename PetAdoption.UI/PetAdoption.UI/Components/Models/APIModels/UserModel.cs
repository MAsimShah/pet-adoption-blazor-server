using Microsoft.AspNetCore.Identity.Data;

namespace PetAdoption.UI.Components.Models.APIModels
{
    public class UserModel : RegisterUserModel
    {
        public string Id { get; set; }
    }
}
