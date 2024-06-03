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
        [ForeignKey("ProblemId")]
        public Problem Problem { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public Account Account { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Language { get; set; }
    }
}
