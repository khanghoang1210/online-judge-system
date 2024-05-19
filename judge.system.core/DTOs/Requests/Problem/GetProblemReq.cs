namespace judge.system.core.DTOs.Requests.Problem
{
    // Get by ID or Title
    // Priority given to ID
    public class GetProblemReq
    {
        public int? ProblemId { get; set; }
        public string Title { get; set; }
    }
}
