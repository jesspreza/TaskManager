using System.ComponentModel.DataAnnotations;

namespace TaskManager.Domain.Entities
{
    public partial class User : BaseEntity
    {
        [Required]
        [MaxLength(250)]
        [MinLength(3)]
        public string UserName { get; private set; }
        
        [Required]
        [MaxLength(512)]
        public string Password { get; private set; }
    }
}
