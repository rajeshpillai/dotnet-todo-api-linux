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
                // Ensure the database is created
                context.Database.Migrate();

                // Check if data already exists
                if (!context.Todos.Any())
                {
                    context.Todos.AddRange(
                        new TodoItem { Title = "Buy groceries", IsCompleted = false },
                        new TodoItem { Title = "Walk the dog", IsCompleted = false },
                        new TodoItem { Title = "Finish C# project", IsCompleted = true },
                        new TodoItem { Title = "Read a book", IsCompleted = false }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
