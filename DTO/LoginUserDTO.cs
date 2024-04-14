using System.ComponentModel.DataAnnotations;

namespace PetsHeaven.DTO
{
    public class LoginUserDTO
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$")]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9@#$%^&*!]{8,20}$")]
        public string Password { get; set; }
    }
}
