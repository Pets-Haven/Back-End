using System.ComponentModel.DataAnnotations;

namespace PetsHeaven.DTO
{
    public class RegisterUserDTO
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
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9_-]{4,20}$")]
        public string UserName { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$")]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9@#$%^&*!]{8,20}$")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public bool isAdmin { get; set; }
    }
}
