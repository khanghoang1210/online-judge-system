using System.Collections.Generic;

namespace judge.system.core.DTOs.Responses.Problem
{
    public class GetProblemRes
    {
        public int ProblemId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float TimeLimit { get; set; }
        public int MemoryLimit { get; set; }
    }
}