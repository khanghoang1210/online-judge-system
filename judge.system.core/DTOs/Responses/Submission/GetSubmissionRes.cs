namespace judge.system.core.DTOs.Responses.Submission
{
    public class GetSubmissionRes
    {
        public string ProblemTitle { get; set; }
        public TimeSpan Time { get; set; }
        public bool IsAccepted { get; set; }
        public int NumCasesPassed { get; set; }
        public string Language { get; set; }
    }
}
