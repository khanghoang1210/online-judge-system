using System.ComponentModel.DataAnnotations;

namespace judge.system.models.Submissions
{
    public class SubmissionResult
    {
        [Required]
        public JudgeState State { get; set; }

        public List<JudgeResult> Samples { get; set; }

        public List<JudgeResult> Tests { get; set; }

        public List<Issue> Issues { get; set; }

        public bool HasIssue { get; set; }

        public TimeSpan? MaximumTime { get; set; }

        public long? MaximumMemory { get; set; }

        public int? TotalCase { get; set; }

        public int? AcceptedCase { get; set; }
    }
}
