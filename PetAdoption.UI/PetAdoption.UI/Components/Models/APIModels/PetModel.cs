namespace PetAdoption.UI.Components.Models.APIModels
{
    public class PetModel : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public int Age { get; set; }
        public int ContactInformation { get; set; }
        public Species Species { get; set; } = Species.Other;
        public AnimalGender Gender { get; set; }
        public AnimalColor Color { get; set; }
        public HealthStatus HealthStatus { get; set; }

        public string Description { get; set; } = string.Empty;
        public bool Microchipped { get; set; } = false;
        public bool GoodWithKids { get; set; } = false;
        public bool GoodWithOtherPets { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public decimal? AdoptionFee { get; set; } = 0;
        public DateTime? AdoptableSince { get; set; }
        public string? Location { get; set; }

        public List<PetPhotoModel> PetPhotos { get; set; } = new List<PetPhotoModel>();
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
