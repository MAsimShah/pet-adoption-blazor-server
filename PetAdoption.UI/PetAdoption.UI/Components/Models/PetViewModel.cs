using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PetAdoption.UI.Components.Models
{
    public class PetViewModel
    {
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

    public enum AnimalGender
    {
        Male = 1,
        Female = 2
    }

    public enum Species
    {
        Dog,
        Cat,
        Rabbit,
        Bird,
        Hamster,
        Other
    }

    public enum WeightCategory
    {
        [Description("Small ( < 15kg)")]
        Small,  // For pets less than 15kg

        [Description("Medium (15 - 30kg)")]
        Medium, // For pets between 15-30kg

        [Description("Large ( > 30kg)")]
        Large   // For pets above 30kg
    }

    public enum AnimalColor
    {
        Black,
        White,
        Brown,
        Grey,
        Tan,
        MultiColor,
        Other
    }

    public enum HealthStatus
    {
        Vaccinated,
        Neutered,
        SpecialMedicalNeeds,
        GoodHealth,
        UnderTreatment,
        Other
    }
}