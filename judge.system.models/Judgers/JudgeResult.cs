using System.ComponentModel.DataAnnotations;

namespace judge.system.models.Judgers
{
    public class JudgeResult
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public JudgeState State { get; set; }

        [DataType(DataType.Duration)]
        public TimeSpan Time { get; set; }

        public long Memory { get; set; }

        public List<Issue> Issues { get; set; }
    }
}
