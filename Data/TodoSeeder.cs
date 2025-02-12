using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Models;

namespace TodoApi.Data
{
    public static class TodoSeeder
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            using (var context = new TodoContext(serviceProvider.GetRequiredService<DbContextOptions<TodoContext>>()))
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Name = "Work" },
                        new Tag { Name = "Personal" },
                        new Tag { Name = "Urgent" },
                        new Tag { Name = "Low Priority" }
                    );
                    context.SaveChanges();
                }

                if (!context.Todos.Any())
                {
                    var workTag = context.Tags.FirstOrDefault(t => t.Name == "Work");
                    var personalTag = context.Tags.FirstOrDefault(t => t.Name == "Personal");
                    var urgentTag = context.Tags.FirstOrDefault(t => t.Name == "Urgent");

                    context.Todos.AddRange(
                        new TodoItem { Title = "Finish project", IsCompleted = false, TodoTags = new List<TodoTag> { new TodoTag { Tag = workTag } } },
                        new TodoItem { Title = "Buy groceries", IsCompleted = false, TodoTags = new List<TodoTag> { new TodoTag { Tag = personalTag } } },
                        new TodoItem { Title = "Reply to emails", IsCompleted = false, TodoTags = new List<TodoTag> { new TodoTag { Tag = workTag }, new TodoTag { Tag = urgentTag } } },
                        new TodoItem { Title = "Schedule doctor appointment", IsCompleted = false, TodoTags = new List<TodoTag> { new TodoTag { Tag = personalTag } } }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}