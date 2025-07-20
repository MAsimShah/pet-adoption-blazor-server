namespace PetAdoption.UI.Components.Models.APIModels
{
    public class PetPhotoModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int PetId { get; set; }
        public string PhotoUrl { get; set; }
    }
}
