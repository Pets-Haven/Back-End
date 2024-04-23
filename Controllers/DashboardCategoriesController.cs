using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetsHeaven.Context;
using PetsHeaven.DTO;
using PetsHeaven.Models;

namespace PetsHeaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class DashboardCategoriesController : ControllerBase
    {
        private readonly PetsHeavenDatabase _db;
        public DashboardCategoriesController(PetsHeavenDatabase db)
        {
            _db = db;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddCategory([FromBody] CategoryDTO category)
        {
            _db.Categories.Add(new Category
            {
                Name = category.Name
            });
            _db.SaveChanges();
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoryById(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }


    }
}
