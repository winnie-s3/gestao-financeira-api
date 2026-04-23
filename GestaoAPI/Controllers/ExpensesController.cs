using GestaoAPI.Models.Dtos;
using GestaoAPI.Models.Entities;
using GestaoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly GestaoAPIDbContext context;

        public ExpensesController(GestaoAPIDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetExpenses(DateTime? startDate, DateTime? endDate)
        {
            var query = context.Expenses.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(e => e.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(e => e.Date <= endDate.Value);

            var result = query
                .OrderByDescending(e => e.Id)
                .ToList();

            return Ok(ApiResponse<object>.Ok(result, "Despesas retornadas com sucesso."));
        }

        [HttpGet("{id}")]
        public IActionResult GetExpense(int id)
        {
            var expense = context.Expenses.Find(id);

            if (expense == null)
                return NotFound(ApiResponse<string>.Fail("Despesa não encontrada."));

            return Ok(ApiResponse<object>.Ok(expense, "Despesa encontrada."));
        }

        [HttpPost]
        public IActionResult CreateExpense([FromBody] ExpenseDto expenseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var categoryExists = context.Categories.Any(c => c.CategoryId == expenseDto.CategoryId);

            if (!categoryExists)
                return BadRequest(ApiResponse<string>.Fail("Categoria inválida."));

            var expense = new Expense
            {
                Description = expenseDto.Description,
                Amount = expenseDto.Amount,
                Date = expenseDto.Date,
                CategoryId = expenseDto.CategoryId
            };

            context.Expenses.Add(expense);
            context.SaveChanges();

            return CreatedAtAction(
                nameof(GetExpense),
                new { id = expense.Id },
                ApiResponse<object>.Ok(expense, "Despesa criada com sucesso.")
            );
        }

        [HttpPut("{id}")]
        public IActionResult UpdateExpense(int id, [FromBody] ExpenseDto expenseDto)
        {
            var expense = context.Expenses.Find(id);

            if (expense == null)
                return NotFound(ApiResponse<string>.Fail("Despesa não encontrada."));

            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var categoryExists = context.Categories.Any(c => c.CategoryId == expenseDto.CategoryId);

            if (!categoryExists)
                return BadRequest(ApiResponse<string>.Fail("Categoria inválida."));

            expense.Description = expenseDto.Description;
            expense.Amount = expenseDto.Amount;
            expense.Date = expenseDto.Date;
            expense.CategoryId = expenseDto.CategoryId;

            context.SaveChanges();

            return Ok(ApiResponse<object>.Ok(new { }, "Despesa atualizada com sucesso."));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteExpense(int id)
        {
            var expense = context.Expenses.Find(id);

            if (expense == null)
                return NotFound(ApiResponse<string>.Fail("Despesa não encontrada."));

            context.Expenses.Remove(expense);
            context.SaveChanges();

            return Ok(ApiResponse<object>.Ok(new { }, "Despesa removida com sucesso."));
        }

        [HttpGet("total")]
        public IActionResult GetTotal()
        {
            var total = context.Expenses.Sum(e => e.Amount);

            return Ok(ApiResponse<object>.Ok(total, "Total de despesas calculado."));
        }

        [HttpGet("by-category")]
        public IActionResult GetByCategory(int? month, int? year)
        {
            var query = context.Expenses.AsQueryable();

            if (month.HasValue && !year.HasValue)
            {
                return BadRequest(ApiResponse<string>.Fail("O ano deve ser informado quando o mês for utilizado."));
            }

            if (month.HasValue && year.HasValue)
            {
                query = query.Where(e => e.Date.Month == month.Value && e.Date.Year == year.Value);
            }
            else if (year.HasValue)
            {
                query = query.Where(e => e.Date.Year == year.Value);
            }

            var result = query
                .GroupBy(e => e.Category.Name)
                .Select(group => new
                {
                    category = group.Key,
                    total = group.Sum(e => e.Amount)
                })
                .OrderByDescending(x => x.total)
                .ToList();

            return Ok(ApiResponse<object>.Ok(result, "Despesas agrupadas por categoria."));
        }
    }
}