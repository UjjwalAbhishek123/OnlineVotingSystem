using System.ComponentModel.DataAnnotations;

namespace OnlineVotingSystemAPI.Models
{
    public class VotingEvent
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Event Name is required.")]
        [StringLength(100, ErrorMessage = "Event Name can't be longer than 100 characters.")]
        public string EventName { get; set; }

        [Required]
        public DateTime EventDate { get; set; }
        public List<Candidate> Candidates { get; set; } = new List<Candidate>(); // Navigation property
    }
}
