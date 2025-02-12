using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/todos
       [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
        {
            var todos = await _context.Todos
                .Include(t => t.TodoTags)       // Load the relationship table
                .ThenInclude(tt => tt.Tag)       // Load the actual Tag data
                .ToListAsync();

            return todos;
        }


        // GET: api/todos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoById(int id)
        {
            var todo = await _context.Todos
                .Include(t => t.TodoTags)
                .ThenInclude(tt => tt.Tag)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null) return NotFound();
            return todo;
        }


        // POST: api/todos
        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodo([FromBody]TodoItem todo, [FromQuery]List<int> tagIds)
        {
            foreach (var tagId in tagIds)
            {
                var tag = await _context.Tags.FindAsync(tagId);
                if (tag != null)
                {
                    todo.TodoTags.Add(new TodoTag { TagId = tagId });
                }
            }

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todo);
        }


        // PUT: api/todos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, TodoItem todo)
        {
            if (id != todo.Id) return BadRequest();

            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/tags")]
        public async Task<IActionResult> AssignTags(int id, List<int> tagIds)
        {
            var todo = await _context.Todos
                .Include(t => t.TodoTags)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null) return NotFound();

            foreach (var tagId in tagIds)
            {
                if (!todo.TodoTags.Any(tt => tt.TagId == tagId))
                {
                    todo.TodoTags.Add(new TodoTag { TodoItemId = id, TagId = tagId });
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/todos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return NotFound();

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/todos/{id}/toggle
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleTodoStatus(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return NotFound();

            todo.IsCompleted = !todo.IsCompleted;
            await _context.SaveChangesAsync();

            return Ok(todo);
        }
    }
}
