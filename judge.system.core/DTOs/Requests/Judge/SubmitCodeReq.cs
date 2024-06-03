namespace judge.system.core.DTOs.Requests.Judge
{
    public class SubmitCodeReq
    {
        public int ProblemId { get; set; }
        public string SourceCode { get; set; }
        public string Language { get; set; }
    }
}
