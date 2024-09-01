using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Request
{
    public class CollaboratorRequestDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User is required")]
        public Guid UserId { get; set; }
    }
}
