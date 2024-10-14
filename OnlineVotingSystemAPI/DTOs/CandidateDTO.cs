namespace OnlineVotingSystemAPI.DTOs
{
    public class CandidateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Party { get; set; }
        public int VotingEventId { get; set; } // Reference only
    }
}
