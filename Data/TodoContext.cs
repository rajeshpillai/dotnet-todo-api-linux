using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) {}

        public DbSet<TodoItem> Todos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TodoTag> TodoTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoTag>()
                .HasKey(tt => new { tt.TodoItemId, tt.TagId }); // Composite primary key

            modelBuilder.Entity<TodoTag>()
                .HasOne(tt => tt.TodoItem)
                .WithMany(t => t.TodoTags)
                .HasForeignKey(tt => tt.TodoItemId);

            modelBuilder.Entity<TodoTag>()
                .HasOne(tt => tt.Tag)
                .WithMany(t => t.TodoTags)
                .HasForeignKey(tt => tt.TagId);
        }
    }
}
