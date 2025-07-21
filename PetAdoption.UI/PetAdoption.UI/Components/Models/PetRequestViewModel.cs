namespace PetAdoption.UI.Components.Models
{
    public class PetRequestViewModel
    {
        public int Id { get; set; }

        public int PetId { get; set; }         // FK to Pet
        public int UserId { get; set; }        // FK to User making the request

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public string Message { get; set; }    // Optional message by user

        public RequestStatus Status { get; set; } = RequestStatus.Pending;
    }

    public enum RequestStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }
}
