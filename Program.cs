using Microsoft.EntityFrameworkCore;
using TodoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add database service
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlite("Data Source=todo.db"));

// Add controllers -> handle circular reference
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });


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
