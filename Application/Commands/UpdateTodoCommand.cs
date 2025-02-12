namespace TodoApi.Application.Todos.Commands
{
    public class UpdateTodoCommand
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public List<int>? TagIds { get; set; }
    }
}
