using judge.system.core.DTOs.Responses.Judge;

namespace judge.system.core.DTOs.Responses.Problem
{
    public class GetProblemDetailRes
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<TestCaseRes> TestCases { get; set; }
    }
}
