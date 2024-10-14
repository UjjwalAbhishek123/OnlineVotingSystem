using System.ComponentModel.DataAnnotations;

namespace OnlineVotingSystemAPI.Models
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Party name is required.")]
        [StringLength(100, ErrorMessage = "Party Name can't be longer than 100 characters.")]
        public string Party { get; set; }
        public int VotingEventId { get; set; } // Foreign key to VotingEvent
        public VotingEvent VotingEvent { get; set; } // Navigation property
    }
}
