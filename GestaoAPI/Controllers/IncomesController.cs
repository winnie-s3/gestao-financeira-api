using GestaoAPI.Models.Dtos;
using GestaoAPI.Models.Entities;
using GestaoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomesController : ControllerBase
    {
        private readonly GestaoAPIDbContext context;

        public IncomesController(GestaoAPIDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetIncomes(DateTime? startDate, DateTime? endDate)
        {
            var filteredIncomes = context.Incomes.AsQueryable();

            if (startDate.HasValue)
                filteredIncomes = filteredIncomes.Where(e => e.Date >= startDate.Value);

            if (endDate.HasValue)
                filteredIncomes = filteredIncomes.Where(e => e.Date <= endDate.Value);

            var result = filteredIncomes
                .OrderByDescending(e => e.Id)
                .ToList();

            return Ok(ApiResponse<object>.Ok(result, "Receitas retornadas com sucesso."));
        }

        [HttpGet("{id}")]
        public IActionResult GetIncome(int id)
        {
            var income = context.Incomes.Find(id);

            if (income == null)
                return NotFound(ApiResponse<string>.Fail("Receita não encontrada."));

            return Ok(ApiResponse<object>.Ok(income, "Receita encontrada."));
        }

        [HttpPost]
        public IActionResult CreateIncome([FromBody] IncomeDto incomeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var categoryExists = context.Categories.Any(c => c.CategoryId == incomeDto.CategoryId);

            if (!categoryExists)
                return BadRequest(ApiResponse<string>.Fail("Categoria inválida."));

            var income = new Income
            {
                Description = incomeDto.Description,
                Amount = incomeDto.Amount,
                Date = incomeDto.Date,
                CategoryId = incomeDto.CategoryId
            };

            context.Incomes.Add(income);
            context.SaveChanges();

            return CreatedAtAction(
                nameof(GetIncome),
                new { id = income.Id },
                ApiResponse<object>.Ok(income, "Receita criada com sucesso.")
            );
        }

        [HttpPut("{id}")]
        public IActionResult UpdateIncome(int id, [FromBody] IncomeDto incomeDto)
        {
            var income = context.Incomes.Find(id);

            if (income == null)
                return NotFound(ApiResponse<string>.Fail("Receita não encontrada."));

            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var categoryExists = context.Categories.Any(c => c.CategoryId == incomeDto.CategoryId);

            if (!categoryExists)
                return BadRequest(ApiResponse<string>.Fail("Categoria inválida."));

            income.Description = incomeDto.Description;
            income.Amount = incomeDto.Amount;
            income.Date = incomeDto.Date;
            income.CategoryId = incomeDto.CategoryId;

            context.SaveChanges();

            return Ok(ApiResponse<object>.Ok(new { }, "Receita atualizada com sucesso."));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteIncome(int id)
        {
            var income = context.Incomes.Find(id);

            if (income == null)
                return NotFound(ApiResponse<string>.Fail("Receita não encontrada."));

            context.Incomes.Remove(income);
            context.SaveChanges();

            return Ok(ApiResponse<object>.Ok(new { }, "Receita removida com sucesso."));
        }

        [HttpGet("total")]
        public IActionResult GetTotal()
        {
            var total = context.Incomes.Sum(e => e.Amount);

            return Ok(ApiResponse<object>.Ok(total, "Total de receitas calculado."));
        }
    }
}