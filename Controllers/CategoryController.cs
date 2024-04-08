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
        public IActionResult GetAll() 
        {
            var categories = db.Categories.Include(d=>d.Products).ToList();
            if(categories == null)
                return NotFound();
            List<CategoryDTO> categoryDTOs = categories.Select(c=> new CategoryDTO { 
                Id = c.Id,
                Name = c.Name,
                NumOfProducts = c.Products.Where(p=>p.CategoryId==c.Id).Count()
            }).ToList();
            return Ok(categoryDTOs);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetByCategory(int id) 
        {
            var category = db.Categories.Include(d=>d.Products).FirstOrDefault(c=>c.Id==id);
            if(category == null) return NotFound();
            List<ProductDTO> productDTOs = category.Products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                AnimalType = p.AnimalType,
                CategoryId = category.Id,   
                CategoryName = category.Name,
                Image = p.Image,
                Price = p.Price,
                Quantity = p.Quantity,
                TotalWeight = p.TotalWeight

            }).ToList();
            return Ok(productDTOs);
        }

    }
}
