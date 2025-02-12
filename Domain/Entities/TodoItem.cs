using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Domain.Entities
{
    public class TodoItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Many-to-many relationship
        public ICollection<TodoTag> TodoTags { get; set; } = new List<TodoTag>();

        public void AssignTags(List<int> tagIds)
        {
            foreach (var tagId in tagIds)
            {
                TodoTags.Add(new TodoTag { TagId = tagId });
            }
        }
    }
}
