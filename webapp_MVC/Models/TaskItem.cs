using System.ComponentModel.DataAnnotations;

namespace webapp_MVC.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsComplete { get; set; }
    }
}
