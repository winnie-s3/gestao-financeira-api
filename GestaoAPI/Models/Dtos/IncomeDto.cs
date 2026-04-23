using System.ComponentModel.DataAnnotations;

namespace GestaoAPI.Models.Dtos
{
    public class IncomeDto
    {
        [MaxLength(500)]
        public string Description { get; set; } = "";
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
