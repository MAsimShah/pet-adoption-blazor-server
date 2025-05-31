namespace PetAdoption.UI.Components.Models
{
    public class PetPhotosViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int PetId { get; set; }
        public string PhotoUrl { get; set; }
    }
}
