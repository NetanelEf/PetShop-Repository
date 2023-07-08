using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Comments
    {
        [Key]
        public int CommentID { get; set; }

        [ForeignKey("Animal")]
        [Required(ErrorMessage = "Please provide the animal of the comment!")]
        public int AnimalID { get; set; }

        public virtual Animals? Animal { get; set; }

        [Required(ErrorMessage = "Please provide the text of the comment!")]
        [StringLength(255)]
        [Column(TypeName = "Comments")]
        public string Comment { get; set; } = string.Empty;

        [Required]
        public string UserNamePosted { get; set; } = string.Empty;
    }
}
