using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Request
{
    public class ProjectRequestDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
