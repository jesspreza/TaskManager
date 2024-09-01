using System.ComponentModel.DataAnnotations;

namespace TaskManager.Domain.Entities
{
    public partial class Project : BaseEntity
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; private set; }

        public virtual ICollection<Task>? Tasks { get; private set; } = new List<Task>();
    }
}
