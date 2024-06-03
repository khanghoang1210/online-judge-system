namespace judge.system.core.DTOs.Responses.Problem
{
    public class GetProblemRes
    {
        public int ProblemId { get; set; }
        public string Title { get; set; }
        public string TitleSlug { get; set; }
        public string Difficulty { get; set; }
        public List<string> TagId { get; set; }

    }
}