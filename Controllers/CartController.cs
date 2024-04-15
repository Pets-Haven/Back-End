using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsHeaven.Context;
using PetsHeaven.Models;

namespace PetsHeaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        PetsHeavenDatabase db;
        public CartController(PetsHeavenDatabase context)
        {
            db = context;
        }
        [HttpGet("{UserId:alpha}")]
        public IActionResult getCart(string UserId)
        {
            var userCart=db.Cart.Where(c => c.userId == UserId).Include<Product>

        }

    }
}
