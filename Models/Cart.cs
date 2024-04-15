using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetsHeaven.Models
{
    public class Cart

    {



        [Key]
        [Column(Order = 0)]
        [ForeignKey("User")]
        public string userId { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Product")]
        public int productId { get; set; }

        public int quantity { get; set; }

        public Product Product { get; set; }
        public ApplicationUser User { get; set; }
    }
}
