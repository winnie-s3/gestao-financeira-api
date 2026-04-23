using GestaoAPI.Models.Dtos;
using GestaoAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace GestaoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly GestaoAPIDbContext context;

        public DashboardController(GestaoAPIDbContext context)
        {
            this.context = context;
        }

        [HttpGet("summary")]
        public IActionResult GetSummary(int? month, int? year)
        {
            var incomesQuery = context.Incomes.AsQueryable();
            var expensesQuery = context.Expenses.AsQueryable();

            if (month.HasValue && !year.HasValue)
            {
                return BadRequest(ApiResponse<string>.Fail("O ano deve ser informado quando o mês for utilizado."));
            }

            if (month.HasValue && year.HasValue)
            {
                incomesQuery = incomesQuery.Where(i => i.Date.Month == month.Value && i.Date.Year == year.Value);
                expensesQuery = expensesQuery.Where(e => e.Date.Month == month.Value && e.Date.Year == year.Value);
            }
            else if (year.HasValue)
            {
                incomesQuery = incomesQuery.Where(i => i.Date.Year == year.Value);
                expensesQuery = expensesQuery.Where(e => e.Date.Year == year.Value);
            }

            var totalIncome = incomesQuery.Sum(i => i.Amount);
            var totalExpense = expensesQuery.Sum(e => e.Amount);
            var balance = totalIncome - totalExpense;

            var result = new
            {
                totalIncome,
                totalExpense,
                balance
            };

            return Ok(ApiResponse<object>.Ok(result, "Resumo financeiro retornado com sucesso."));
        }

        [HttpGet("monthly")]
        public IActionResult GetSummaryMonthly(int? month, int? year)
        {
            if (month.HasValue && !year.HasValue)
            {
                return BadRequest(ApiResponse<string>.Fail("O ano deve ser informado quando o mês for utilizado."));
            }

            var incomesQuery = context.Incomes.AsQueryable();
            var expensesQuery = context.Expenses.AsQueryable();

            if (year.HasValue)
            {
                incomesQuery = incomesQuery.Where(i => i.Date.Year == year.Value);
                expensesQuery = expensesQuery.Where(e => e.Date.Year == year.Value);
            }

            var incomes = incomesQuery.ToList();
            var expenses = expensesQuery.ToList();

            var result = new List<object>();

            for (int m = 1; m <= 12; m++)
            {
                if (month.HasValue && m != month.Value)
                    continue;

                var totalIncome = incomes
                    .Where(i => i.Date.Month == m)
                    .Sum(i => i.Amount);

                var totalExpense = expenses
                    .Where(e => e.Date.Month == m)
                    .Sum(e => e.Amount);

                var monthName = CultureInfo
                    .GetCultureInfo("pt-BR")
                    .DateTimeFormat
                    .GetAbbreviatedMonthName(m);

                monthName = char.ToUpper(monthName[0]) + monthName.Substring(1);

                result.Add(new
                {
                    month = m,
                    monthName = monthName,
                    income = totalIncome,
                    expense = totalExpense
                });
            }

            return Ok(ApiResponse<object>.Ok(result, "Dados mensais retornados com sucesso."));
        }

        [HttpGet("full")]
        public IActionResult GetDashboardFull(int? month, int? year)
        {
            var incomesQuery = context.Incomes.AsQueryable();
            var expensesQuery = context.Expenses.AsQueryable();

            if (month.HasValue && !year.HasValue)
            {
                return BadRequest(ApiResponse<string>.Fail("O ano deve ser informado quando o mês for utilizado."));
            }

            if (month.HasValue && year.HasValue)
            {
                incomesQuery = incomesQuery.Where(i => i.Date.Month == month && i.Date.Year == year);
                expensesQuery = expensesQuery.Where(e => e.Date.Month == month && e.Date.Year == year);
            }
            else if (year.HasValue)
            {
                incomesQuery = incomesQuery.Where(i => i.Date.Year == year);
                expensesQuery = expensesQuery.Where(e => e.Date.Year == year);
            }

            // SUMMARY
            var totalIncome = incomesQuery.Sum(i => i.Amount);
            var totalExpense = expensesQuery.Sum(e => e.Amount);
            var balance = totalIncome - totalExpense;

            var summary = new
            {
                totalIncome,
                totalExpense,
                balance
            };

            // MONTHLY
            var monthly = Enumerable.Range(1, 12)
                .Select(m => new
                {
                    month = m,
                    income = context.Incomes
                        .Where(i => i.Date.Month == m && (!year.HasValue || i.Date.Year == year))
                        .Sum(i => i.Amount),

                    expense = context.Expenses
                        .Where(e => e.Date.Month == m && (!year.HasValue || e.Date.Year == year))
                        .Sum(e => e.Amount)
                })
                .ToList();

            // EXPENSES BY CATEGORY
            var expensesByCategory = expensesQuery
                .GroupBy(e => e.Category.Name)
                .Select(g => new
                {
                    category = g.Key,
                    total = g.Sum(e => e.Amount)
                })
                .OrderByDescending(x => x.total)
                .ToList();

            // INCOMES BY CATEGORY
            var incomesByCategory = incomesQuery
                .GroupBy(i => i.Category.Name)
                .Select(g => new
                {
                    category = g.Key,
                    total = g.Sum(i => i.Amount)
                })
                .OrderByDescending(x => x.total)
                .ToList();

            var result = new
            {
                summary,
                monthly,
                expensesByCategory,
                incomesByCategory
            };

            return Ok(ApiResponse<object>.Ok(result, "Dashboard completo carregado com sucesso."));
        }
    }
}