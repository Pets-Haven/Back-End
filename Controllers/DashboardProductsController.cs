using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsHeaven.Context;
using PetsHeaven.DTO;
using PetsHeaven.Models;

namespace PetsHeaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class DashboardProductsController : ControllerBase
    {
        PetsHeavenDatabase db;
        public DashboardProductsController(PetsHeavenDatabase context)
        {
            db = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetAllProducts()
        {
            List<Product> products = db.Products.Include(p => p.Category).ToList();
            if (products == null)
                return NotFound();
            List<ProductDTO> productDTOs = products.Select(p => new ProductDTO
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
            return Ok(productDTOs);
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetProductById(int id)
        {
            Product product = db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult SearchProductByName(string searchValue)
        {
            List<Product> products = db.Products.Include(p => p.Category).Where(p => p.Name.Contains(searchValue)).ToList();
            if (products == null)
                return NotFound();
            List<ProductDTO> productDTOs = products.Select(p => new ProductDTO
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
            return Ok(productDTOs);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult EditProduct(ProductDTO product)
        {
            Product productToEdit = db.Products.FirstOrDefault(p => p.Id == product.Id);
            if (productToEdit == null)
                return NotFound("Product Not Found");
            productToEdit.Name = product.Name;
            productToEdit.Price = product.Price;
            productToEdit.Description = product.Description;
            productToEdit.TotalWeight = product.TotalWeight;
            productToEdit.AnimalType = product.AnimalType;
            productToEdit.Quantity = product.Quantity;
            productToEdit.Image = product.Image;
            productToEdit.CategoryId = product.CategoryId;
            db.SaveChanges();
            return Ok(new
            {
                product,
                Message = "Product Updated Successfully"
            });
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteProduct(int id)
        {
            Product productToDelete = db.Products.FirstOrDefault(p => p.Id == id);
            if (productToDelete == null)
                return NotFound("Product Not Found");
            db.Products.Remove(productToDelete);
            db.SaveChanges();
            return Ok(new
            {
                productToDelete,
                Message = "Product Deleted Successfully"
            });
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddProduct(ProductDTO product)
        {
            Product newProduct = new Product
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                TotalWeight = product.TotalWeight,
                AnimalType = product.AnimalType,
                Quantity = product.Quantity,
                Image = product.Image,
                CategoryId = product.CategoryId
            };
            db.Products.Add(newProduct);
            db.SaveChanges();
            return Created("", new
            {
                newProduct,
                Message = "Product Added Successfully"
            });
        }
    }
}
