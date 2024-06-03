using System.ComponentModel.DataAnnotations.Schema;

namespace judge.system.core.Models
{
    [Table("problem_tag")]
    public class ProblemTag
    {
        public int ProblemId { get; set; }
        public Problem Problem { get; set; }
        public string TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
