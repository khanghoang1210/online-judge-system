using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace judge.system.core.Models
{
    [Table("tag")]
    public class Tag
    {
        [Key]
        public string TagId { get; set; }
        public string TagName { get; set; }
        public string TagSlug { get; set; }
        public List<ProblemTag> ProblemTags { get; set; }
    }
}
