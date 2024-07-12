using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Models.User
{
    public class UserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
}
