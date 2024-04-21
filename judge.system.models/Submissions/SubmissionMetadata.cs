using judge.system.models.Judgers;
using System.ComponentModel.DataAnnotations;

namespace judge.system.models.Submissions
{
    public class SubmissionMetadata : IHasId<string>
    {
        public string Id { get; set; }

        [Required]
        public string ProblemId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public ProgrammingLanguage Language { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTimeOffset Time { get; set; }

        public uint CodeLength { get; set; }
    }
}
