using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Request
{
    public class UserRequestDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(250, ErrorMessage = "Username must be less than 250 characters")]
        [MinLength(3, ErrorMessage = "Username must be at leas 3 characters long")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(512, ErrorMessage = "Password must be shorter")]
        [MinLength(6, ErrorMessage = "Password must have at least 6 characters long")]
        public string Password { get; set; }
    }
}
