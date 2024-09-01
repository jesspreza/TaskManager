using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.DTOs
{
    public class TaskDto
    {
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        public virtual ProjectDto? ProjectDto { get; set; }

        public virtual IEnumerable<TimeTrackerDto?> TimeTrackersDto { get; set; }
    }
}
