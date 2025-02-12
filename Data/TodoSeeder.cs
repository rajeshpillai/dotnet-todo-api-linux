using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Domain.Entities;
using TodoApi.Infrastructure.Persistence;

namespace TodoApi.Infrastructure.Seeding
{
    public static class TodoSeeder
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            using (var context = new TodoContext(serviceProvider.GetRequiredService<DbContextOptions<TodoContext>>()))
            {
                // Apply any pending migrations
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                // Seed Tags if they don’t exist
                if (!context.Tags.Any())
                {
                    var tags = new List<Tag>
                    {
                        new Tag { Name = "Work" },
                        new Tag { Name = "Personal" },
                        new Tag { Name = "Urgent" },
                        new Tag { Name = "Low Priority" }
                    };

                    context.Tags.AddRange(tags);
                    context.SaveChanges();
                }

                // Seed Todos if they don’t exist
                if (!context.Todos.Any())
                {
                    var workTag = context.Tags.FirstOrDefault(t => t.Name == "Work");
                    var personalTag = context.Tags.FirstOrDefault(t => t.Name == "Personal");
                    var urgentTag = context.Tags.FirstOrDefault(t => t.Name == "Urgent");

                    var todos = new List<TodoItem>
                    {
                        new TodoItem
                        {
                            Title = "Finish project",
                            IsCompleted = false,
                            TodoTags = new List<TodoTag> { new TodoTag { TagId = workTag.Id } }
                        },
                        new TodoItem
                        {
                            Title = "Buy groceries",
                            IsCompleted = false,
                            TodoTags = new List<TodoTag> { new TodoTag { TagId = personalTag.Id } }
                        },
                        new TodoItem
                        {
                            Title = "Reply to emails",
                            IsCompleted = false,
                            TodoTags = new List<TodoTag>
                            {
                                new TodoTag { TagId = workTag.Id },
                                new TodoTag { TagId = urgentTag.Id }
                            }
                        },
                        new TodoItem
                        {
                            Title = "Schedule doctor appointment",
                            IsCompleted = false,
                            TodoTags = new List<TodoTag> { new TodoTag { TagId = personalTag.Id } }
                        }
                    };

                    context.Todos.AddRange(todos);
                    context.SaveChanges();
                }
            }
        }
    }
}
