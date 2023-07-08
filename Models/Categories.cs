using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Validators;

namespace WebApplication1.Models
{
    public class Categories
    {
        [Key]
        public int CategoryID { get; set; } = 0;

        [Required(ErrorMessage = "Please provide the name of the category!")]
        [StringLength(100)]
        [Column(TypeName = "Category Names")]
        [OnlyLetters(ErrorMessage = "Only letters here!")]
        public string CategoryName { get; set; } = string.Empty;

    }
}
