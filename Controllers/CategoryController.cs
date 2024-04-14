using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PetsHeaven.Context;
using PetsHeaven.DTO;

namespace PetsHeaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        PetsHeavenDatabase db;
        public CategoryController(PetsHeavenDatabase context)
        {
            db = context;
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            var categories = db.Categories.Include(d => d.Products).ToList();
            if (categories == null)
                return NotFound();
            List<CategoryDTO> categoryDTOs = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                NumOfProducts = c.Products.Where(p => p.CategoryId == c.Id).Count()
            }).ToList();
            return Ok(categoryDTOs);
        }
    }
}
