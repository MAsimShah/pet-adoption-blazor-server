using PetAdoption.UI.Components.Models.DTOs;

namespace PetAdoption.UI.Components.Models
{
    public class Base64UploadRequest
    {
        public int PetId { get; set; }
        public List<Base64ImageFile> Images { get; set; }
    }
}
