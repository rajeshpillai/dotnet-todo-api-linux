using Microsoft.EntityFrameworkCore;
using TodoApi.Infrastructure;
using TodoApi.Infrastructure.Persistence;
using TodoApi.Infrastructure.Seeding;

var builder = WebApplication.CreateBuilder(args);

// ✅ Allow ALL Origins, Methods, and Headers (for dev/testing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Add database service
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlite("Data Source=todo.db"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>(); // Register TagRepository

// Add controllers -> handle circular reference
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });


// ✅ Force Kestrel to use port 5555
builder.WebHost.UseKestrel()
    .ConfigureKestrel((context, options) =>
    {
        options.ListenAnyIP(5555); // ✅ Runs on port 5555
    });

var app = builder.Build();


// ✅ Enable CORS Middleware for All Requests
app.UseCors("AllowAll");

// ✅ Manually handle OPTIONS requests before controllers
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        context.Response.StatusCode = 204; // ✅ No Content
        return;
    }
    await next();
});

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
