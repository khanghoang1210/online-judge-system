namespace judge.system.core.DTOs.Responses.Submission
{
    public class GetSubmissionRes
    {
        public int SubmissionId { get; set; }
        public bool IsAccepted { get; set; }
        public int NumCasesPassed { get; set; }
        public int ProblemId { get; set; }

        // public int UserId { get; set; }
    }
}
