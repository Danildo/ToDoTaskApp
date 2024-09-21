using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoTaskApplication.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the task title.")]
        [MaxLength(50, ErrorMessage = "The title must have a maximum of 50 characters")]
        public string Title { get; set; }

        [MaxLength(250, ErrorMessage = "The description must have a maximum of 250 characters")]
        public string? Description { get; set; }

        [DisplayName("Creation Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Completion Date")]
        public DateTime? CompletedDate { get; set; }

        [ForeignKey("Status")]
        public int StatusId { get; set; }


        [Required(ErrorMessage = "Choose a status.")]
        public Status Status { get; set; }
    }
}
