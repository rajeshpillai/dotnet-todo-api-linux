using Microsoft.AspNetCore.Mvc;
using TodoApi.Application.Todos.Commands;
using TodoApi.Application.Todos.DTOs;
using TodoApi.Domain.Entities;
using TodoApi.Infrastructure;

namespace TodoApi.API.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TodoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// ✅ Get All Todos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetTodos()
        {
            var todos = await _unitOfWork.Todos.GetAllAsync();
            var todoDtos = todos.Select(todo => new TodoDto(todo)).ToList();
            return Ok(todoDtos);
        }

        /// <summary>
        /// ✅ Get a Single TodoItem by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDto>> GetTodoById(int id)
        {
            var todo = await _unitOfWork.Todos.GetByIdAsync(id);
            if (todo == null) return NotFound(new { message = "TodoItem not found" });

            return Ok(new TodoDto(todo));
        }

        /// <summary>
        /// ✅ Create a New TodoItem
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TodoDto>> CreateTodo([FromBody] CreateTodoCommand command)
        {
            var todo = new TodoItem
            {
                Title = command.Title
            };

            if (command.TagIds != null)
            {
                todo.AssignTags(command.TagIds);
            }

            await _unitOfWork.Todos.AddAsync(todo);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, new TodoDto(todo));
        }

        /// <summary>
        /// ✅ Update an Existing TodoItem
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] UpdateTodoCommand command)
        {
            var todo = await _unitOfWork.Todos.GetByIdAsync(id);
            if (todo == null) return NotFound(new { message = "TodoItem not found" });

            todo.Title = command.Title;
            todo.IsCompleted = command.IsCompleted;

            if (command.TagIds != null)
            {
                todo.TodoTags.Clear();
                todo.AssignTags(command.TagIds);
            }

            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        /// <summary>
        /// ✅ Delete a TodoItem
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _unitOfWork.Todos.GetByIdAsync(id);
            if (todo == null) return NotFound(new { message = "TodoItem not found" });

            _unitOfWork.Todos.Remove(todo);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        /// <summary>
        /// ✅ Toggle TodoItem Completion Status
        /// </summary>
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleTodoCompletion(int id)
        {
            var todo = await _unitOfWork.Todos.GetByIdAsync(id);
            if (todo == null) return NotFound(new { message = "TodoItem not found" });

            todo.IsCompleted = !todo.IsCompleted;
            await _unitOfWork.CompleteAsync();
            return Ok(new { message = "TodoItem status updated", isCompleted = todo.IsCompleted });
        }
    }
}
