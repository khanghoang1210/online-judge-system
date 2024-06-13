namespace judge.system.core.DTOs.Requests.Account
{
    public class CreateSubmissionReq
    {
        public int ProblemId { get; set; }
        public string Language { get; set; }
        public bool IsAccepted { get; set; }
        public int NumCasesPassed { get; set; }
    }
}
