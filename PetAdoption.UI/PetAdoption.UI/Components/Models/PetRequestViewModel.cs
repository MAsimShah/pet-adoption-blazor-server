using PetAdoption.UI.Components.Models.APIModels;
using System.ComponentModel.DataAnnotations;

namespace PetAdoption.UI.Components.Models
{
    public class PetRequestViewModel
    {
        public PetRequestViewModel()
        {

        }

        public PetRequestViewModel(PetRequestModel model)
        {
            Id = model.Id;
            PetId = model.PetId ?? 0;
            PetName = model.PetName;
            UserId = model.UserId;
            UserName = model.UserName;
            RequestDate = model.RequestDate;
            Message = model.Message;
            Status = model.Status;
            SpeciesId = model.Specie is null ? (int)Species.Bird : (int)model.Specie;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a pet.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a pet.")]
        public int PetId { get; set; }

        public string? PetName { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Please select a species.")]
        public int SpeciesId { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public string? Message { get; set; }    // Optional message by user

        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public List<DropDownModal> PetList { get; set; } = new List<DropDownModal>();
    }
}