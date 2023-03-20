using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        //1 - Qualquer numero | 2 - qualquer letra minuscula | 3 - qualquer letra maiuscula
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "The password must be complex")]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
