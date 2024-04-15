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
    public class CartController : ControllerBase
    {
        PetsHeavenDatabase db;
        public CartController(PetsHeavenDatabase context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult getCart(string UserId)
        {
            var userCart = db.Cart.Include(d => d.Product).Where(c => c.userId == UserId).ToList();
            if (userCart == null || userCart.Count() == 0)
            {
                return NotFound();
            }
            List<CartDTO> cart = userCart.Select(c=> new CartDTO
            {
               productId = c.productId,
               productName=c.Product.Name,
               productPrice=c.Product.Price,
               productImage=c.Product.Image,
               productQuantity=c.Product.Quantity,
                cartQuantity = c.quantity

            }).ToList();
            return Ok(cart);

        }
        [HttpPost]
        public IActionResult addToCart(string userId,CartDTO cart)
        {
            if (cart == null)
            {
                return BadRequest();
            }

            Cart newCart = new Cart()
            {
                productId = cart.productId,
                userId = userId,
                quantity = cart.cartQuantity
            };
            db.Cart.Add(newCart);
            db.SaveChanges();
            return Ok();
        }
        [HttpDelete]
        public IActionResult deleteItem(string userId, int productId) {
            var cart=db.Cart.Where(c=>c.userId==userId&& c.productId==productId).FirstOrDefault();
        if(cart == null)
            {
                return NotFound();
            }
          
        db.Cart.Remove(cart);
            db.SaveChanges();
            return Ok();
        
        }
        [HttpPut]
        public IActionResult editItem(string userId, CartDTO cart)
        {
            if (cart == null)
            {
                return BadRequest();
            }
            Cart updatedCart = new Cart()
            {
                productId = cart.productId,
                userId = userId,
                quantity = cart.cartQuantity

            };
            db.Cart.Update(updatedCart);
            db.SaveChanges();
            return Ok();
            }

    }
}
