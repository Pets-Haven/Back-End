using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PetsHeaven.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z]{3,20}$")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z]{3,20}$")]
        public string LastName { get; set; }
        public virtual ICollection<Cart>? Cart { get; set; }

    }
}
