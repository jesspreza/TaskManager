using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs
{
    public class UserDto
    {
        [Required]
        [MaxLength(250)]
        [MinLength(3)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(512)]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
