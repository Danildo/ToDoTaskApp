using ToDoTaskApplication.Models;

namespace ToDoTaskApplication.Repositories
{
    public interface IToDoTaskRepository
    {
        Task<IEnumerable<ToDoTask>> GetAllTasksAsync();
        Task<ToDoTask> GetTaskByIdAsync(int id);
        Task AddTaskAsync(ToDoTask toDoTask);
        Task UpdateTaskAsync(ToDoTask toDoTask);
        Task DeleteTaskAsync(int id);
        Task<bool> TaskExistsAsync(int id);
        Task<Status> GetDefaultStatusAsync();
        Task SetTaskStatusAsync(int id, string statusName);
        Task ClearDoneTasksAsync();
    }
}
