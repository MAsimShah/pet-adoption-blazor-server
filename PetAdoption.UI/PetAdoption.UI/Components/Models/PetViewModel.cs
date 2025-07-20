using PetAdoption.UI.Components.Models.APIModels;
using System.ComponentModel.DataAnnotations;

namespace PetAdoption.UI.Components.Models
{
    public class PetViewModel
    {
        public PetViewModel()
        {
            
        }
        public PetViewModel(PetModel model)
        {
            Id = model.Id;
            Name = model.Name;
            Breed = model.Breed;
            Age = model.Age;
            ContactInformation = model.ContactInformation;
            Species = model.Species;
            Gender = model.Gender;
            Color = model.Color;
            HealthStatus = model.HealthStatus;
            Description = model.Description;
            Microchipped = model.Microchipped;
            GoodWithKids = model.GoodWithKids;
            GoodWithOtherPets = model.GoodWithOtherPets;
            AdoptionFee = model.AdoptionFee;
            AdoptableSince = model.AdoptableSince;
            Location = model.Location;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Breed can't be longer than 100 characters.")]
        public string Breed { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "Age must be between 0 and 100.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Contact information is required.")]
        public int ContactInformation { get; set; }

        [Required]
        public Species Species { get; set; } = Species.Other;

        [Required]
        public AnimalGender Gender { get; set; } = AnimalGender.Male;

        [Required]
        public AnimalColor Color { get; set; } = AnimalColor.Black;

        [Required]
        public HealthStatus HealthStatus { get; set; } = HealthStatus.GoodHealth;

        public string Description { get; set; } = string.Empty;
        public bool Microchipped { get; set; } = false;
        public bool GoodWithKids { get; set; } = false;
        public bool GoodWithOtherPets { get; set; } = false;
        public decimal? AdoptionFee { get; set; } = 0;
        public DateTime? AdoptableSince { get; set; }
        public string? Location { get; set; }

        public List<PetPhotosViewModel> PetPhotos { get; set; }
    }
}