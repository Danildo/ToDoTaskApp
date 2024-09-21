using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoTaskApplication.Models
{
    public class Status
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the status name.")]
        [MaxLength(50, ErrorMessage = "The status name must have a maximum of 50 characters")]
        [DisplayName("Status")]
        public required string StatusName { get; set; }
    }
}
