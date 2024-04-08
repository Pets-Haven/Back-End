using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsHeaven.Context;
using PetsHeaven.DTO;
using PetsHeaven.Models;

namespace PetsHeaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        PetsHeavenDatabase db;
        public ProductController(PetsHeavenDatabase context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult GetAll() 
        {
            var products = db.Products.Include(d=>d.Category).ToList();
            if (products == null)
                return NotFound();

            List<ProductDTO> productsDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                TotalWeight = p.TotalWeight,
                AnimalType = p.AnimalType,
                Quantity = p.Quantity,
                Image = p.Image,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();
            return Ok(productsDTOs);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id) 
        {
            var product = db.Products.Include(d => d.Category).FirstOrDefault(p=> p.Id==id);
            if (product == null) return NotFound();
            ProductDTO productDTO = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                TotalWeight = product.TotalWeight,
                AnimalType = product.AnimalType,
                Quantity = product.Quantity,
                Image = product.Image,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name
            };
            return Ok(productDTO);
        }
        [HttpGet("{searchValue:alpha}")]
        public IActionResult searchByName(string searchValue) 
        {
            var products = db.Products.Include(d => d.Category).Where(p => p.Name.Contains(searchValue)).ToList();
            if (products == null)
                return NotFound();

            List<ProductDTO> productsDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                TotalWeight = p.TotalWeight,
                AnimalType = p.AnimalType,
                Quantity = p.Quantity,
                Image = p.Image,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();
            return Ok(productsDTOs);

        }
        [HttpGet("category/{categoryId:int}")]
        public IActionResult GetByCategory(int  categoryId)
        {
            
            var products=db.Products.Include(d=>d.Category).Where(p=>p.CategoryId==categoryId).ToList();
            if (products == null) return NotFound();
            List<ProductDTO> productsDto = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                TotalWeight = p.TotalWeight,
                AnimalType = p.AnimalType,
                Quantity = p.Quantity,
                Image = p.Image,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();
            return Ok(productsDto);

        }
        
    }
}
