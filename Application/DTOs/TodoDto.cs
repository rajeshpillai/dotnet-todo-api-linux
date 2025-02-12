using System;
using System.Collections.Generic;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Todos.DTOs
{
    public class TodoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TodoTagDto> TodoTags { get; set; } = new List<TodoTagDto>();

        // ✅ Constructor to Convert `TodoItem` to `TodoDto`
        public TodoDto(TodoItem todo)
        {
            Id = todo.Id;
            Title = todo.Title;
            IsCompleted = todo.IsCompleted;
            CreatedAt = todo.CreatedAt;

            // Convert TodoTags to DTO format
            if (todo.TodoTags != null)
            {
                foreach (var todoTag in todo.TodoTags)
                {
                    TodoTags.Add(new TodoTagDto
                    {
                        TagId = todoTag.TagId,
                        TagName = todoTag.Tag.Name
                    });
                }
            }
        }
    }

    public class TodoTagDto
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
    }
}
