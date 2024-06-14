using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace judge.system.core.Models
{
    [Table("post")]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[Required]
        public string Title { get; set; }
        //[Required]
        public string Introduction { get; set; }
        //[Required]
        public string Content { get; set; }
        public string Image { get; set; }
    }
}
