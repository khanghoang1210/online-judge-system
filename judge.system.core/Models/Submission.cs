using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace judge.system.core.Models
{
    [Table("submission")]
    public class Submission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubmissionId { get; set; }
        public bool IsAccepted { get; set; }
        public int NumCasesPassed { get; set; }
        public int ProblemId { get; set; }
        public int UserName { get; set; }
    }
}
