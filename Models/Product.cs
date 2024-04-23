using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetsHeaven.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Double Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public Double TotalWeight { get; set; }
        public string AnimalType { get; set; }
        public string Image { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Cart>? Cart { get; set; }
    }
}
