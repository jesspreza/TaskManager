using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Models
{
    public class PaginationParams
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        [Range(1, 50, ErrorMessage = "Up to 50 items are allowed per page")]
        public int PageSize { get; set; }
    }
}
