using System.Collections.Generic;

namespace TodoApi.Application.Todos.Commands
{
    public class CreateTodoCommand
    {
        public string Title { get; set; }
        public List<int>? TagIds { get; set; }
    }
}
