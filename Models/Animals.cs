using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Validators;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace WebApplication1.Models
{
    public class Animals
    {
        [Key]
        public int AnimalID { get; set; } = 0;

        [Required(ErrorMessage = "Please provide the name of the animal!")]
        [StringLength(100)]
        [Display(Name = "Animal Name: ")]
        [OnlyLetters(ErrorMessage = "Only letters here!")]
        public string AnimalName { get; set;} = string.Empty;

        [Required(ErrorMessage = "Please provide the age of the animal!")]
        [Range(0, 150)]
        [Display(Name = "Animal Age: ")]
        [OnlyNumbers(ErrorMessage = "Only numbers here!")]
        public int AnimalAge { get; set; } = 0;

        [Required]
        public byte[]? Image { get; set;}

        [Required(ErrorMessage = "Please provide the description of the animal!")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Animal Description: ")]
        public string Description { get; set;} = string.Empty;

        [ForeignKey("Category")]
        [Required(ErrorMessage = "Please provide the category of the animal!")]
        [Display(Name = "Category:")]
        public int CategoryID { get; set; } = 0;

        public virtual Categories? Category { get; set; } 
    }
}
