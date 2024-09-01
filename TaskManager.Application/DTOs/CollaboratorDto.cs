using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs
{
    public class CollaboratorDto
    {
        
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(250)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public UserDto? UserDto { get; set; } 
        public IEnumerable<TimeTrackerDto?> TimeTrackersDto { get; set; } = new List<TimeTrackerDto>();
    }
}
