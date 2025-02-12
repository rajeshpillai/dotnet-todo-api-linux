using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Domain.Entities
{
    public class TodoTag
    {
        public int TodoItemId { get; set; }
        public TodoItem TodoItem { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
