using System.ComponentModel.DataAnnotations;

namespace GestaoAPI.Models.Dtos
{
    public class CategoryDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "";
    }
}
