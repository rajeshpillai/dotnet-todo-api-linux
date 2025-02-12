using Microsoft.EntityFrameworkCore;
using TodoApi.Domain.Entities;

namespace TodoApi.Infrastructure.Persistence
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        public DbSet<TodoItem> Todos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TodoTag> TodoTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoTag>()
                .HasKey(tt => new { tt.TodoItemId, tt.TagId });

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
