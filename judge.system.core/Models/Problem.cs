using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace judge.system.core.Models
{
    [Table("problem")]
    public class Problem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProblemId { get; set; }
        public string Title { get; set; }

        public string TitleSlug { get; set; }
        public string Difficulty { get; set; }
        public List<string> TagId { get; set; }
        //public float TimeLimit { get; set; }
        //public int MemoryLimit { get; set; }
        //public List<TestCase> TestCases { get; set; }
        //public string Hint { get; set; }
        //public string Description { get; set; }
    }


}
