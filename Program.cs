using Microsoft.EntityFrameworkCore;
using TodoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add database service
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlite("Data Source=todo.db"));

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

// Apply database migrations automatically
using (var scope = app.Services.CreateScope())
{
    // var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
    // dbContext.Database.Migrate();
    var services = scope.ServiceProvider;
    TodoSeeder.SeedData(services);
}

// Enable endpoints
app.MapControllers();
app.Run();
