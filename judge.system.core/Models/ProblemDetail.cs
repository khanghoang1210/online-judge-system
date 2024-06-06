using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace judge.system.core.Models
{
    [Table("problem_detail")]
    public class ProblemDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProblemDetailId { get; set; }
        public int ProblemId { get; set; }
        [ForeignKey("ProblemId")]
        public Problem Problem { get; set; }
        public string Title { get; set; }

        public float TimeLimit { get; set; }
        public int MemoryLimit { get; set; }
        public List<TestCase> TestCases { get; set; }
        public string Hint { get; set; }
        public string Description { get; set; }
        public string ReturnType { get; set; }
    }
    public class TestCase
    {
        public JToken Input { get; set; }
        public JToken Output { get; set; }

    }
}
