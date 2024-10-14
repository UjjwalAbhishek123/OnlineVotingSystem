namespace OnlineVotingSystemAPI.DTOs
{
    public class VotingEventDTO
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }

        // Optionally, include only candidate IDs or minimal info if needed
        //public List<int> CandidateIds { get; set; } = new List<int>();
    }
}
