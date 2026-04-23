using GestaoAPI.Models.Dtos;
using GestaoAPI.Models.Entities;
using GestaoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly GestaoAPIDbContext context;

        public CategoryController(GestaoAPIDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name
                })
                .ToList();

            return Ok(ApiResponse<object>.Ok(categories, "Categorias retornadas com sucesso."));
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = context.Categories.Find(id);

            if (category == null)
            {
                return NotFound(ApiResponse<string>.Fail("Categoria não encontrada."));
            }

            var result = new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };

            return Ok(ApiResponse<object>.Ok(result, "Categoria encontrada."));
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var exists = context.Categories
                .Any(c => c.Name.ToLower() == categoryDto.Name.ToLower());

            if (exists)
            {
                return BadRequest(ApiResponse<string>.Fail("Já existe uma categoria com esse nome."));
            }

            var category = new Category
            {
                Name = categoryDto.Name
            };

            context.Categories.Add(category);
            context.SaveChanges();

            var result = new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = category.CategoryId },
                ApiResponse<object>.Ok(result, "Categoria criada com sucesso.")
            );
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            var category = context.Categories.Find(id);

            if (category == null)
            {
                return NotFound(ApiResponse<string>.Fail("Categoria não encontrada."));
            }

            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Dados inválidos."));

            var exists = context.Categories
                .Any(c => c.Name.ToLower() == categoryDto.Name.ToLower() && c.CategoryId != id);

            if (exists)
            {
                return BadRequest(ApiResponse<string>.Fail("Já existe uma categoria com esse nome."));
            }

            category.Name = categoryDto.Name;

            context.SaveChanges();

            return Ok(ApiResponse<object>.Ok(new { }, "Categoria atualizada com sucesso."));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = context.Categories.Find(id);

            if (category == null)
            {
                return NotFound(ApiResponse<string>.Fail("Categoria não encontrada."));
            }

            context.Categories.Remove(category);
            context.SaveChanges();

            return Ok(ApiResponse<object>.Ok(new { }, "Categoria removida com sucesso."));
        }
    }
}