using Microsoft.EntityFrameworkCore;
using ToDoTaskApplication.Data;
using ToDoTaskApplication.Models;

namespace ToDoTaskApplication.Repositories
{
    /// <summary>
    /// Repository for managing to-do tasks.
    /// </summary>
    /// <remarks>
    /// This repository provides methods to perform CRUD operations on to-do tasks and manage their statuses.
    /// </remarks>
    public class ToDoTaskRepository : IToDoTaskRepository
    {
        #region Properties

        private readonly ToDoTaskContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoTaskRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to interact with the database.</param>
        public ToDoTaskRepository(ToDoTaskContext context)
        {
            _context = context;
        }

        #endregion

        #region GetAllTasksAsync

        /// <summary>
        /// Retrieves all to-do tasks from the database.
        /// </summary>
        /// <returns>
        /// An asynchronous task that, when completed, returns an <see cref="IEnumerable{ToDoTask}"/> 
        /// containing all to-do tasks.
        /// </returns>
        public async Task<IEnumerable<ToDoTask>> GetAllTasksAsync()
        {
            return await _context.ToDoTask.Include(t => t.Status).ToListAsync();
        }

        #endregion

        #region GetTaskByIdAsync

        /// <summary>
        /// Retrieves a specific to-do task by its ID.
        /// </summary>
        /// <param name="id">The ID of the to-do task to retrieve.</param>
        /// <returns>
        /// An asynchronous task that, when completed, returns a <see cref="ToDoTask"/> 
        /// if found, or null if not found.
        /// </returns>
        public async Task<ToDoTask> GetTaskByIdAsync(int id)
        {
            return await _context.ToDoTask.Include(t => t.Status).FirstOrDefaultAsync(t => t.Id == id);
        }

        #endregion

        #region AddTaskAsync

        /// <summary>
        /// Adds a new to-do task to the database.
        /// </summary>
        /// <param name="toDoTask">The to-do task to add.</param>
        /// <returns>
        /// An asynchronous task that, when completed, saves the new task to the database.
        /// </returns>
        public async Task AddTaskAsync(ToDoTask toDoTask)
        {
            _context.ToDoTask.Add(toDoTask);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region UpdateTaskAsync

        /// <summary>
        /// Updates an existing to-do task in the database.
        /// </summary>
        /// <param name="toDoTask">The to-do task to update.</param>
        /// <returns>
        /// An asynchronous task that, when completed, saves the updated task to the database.
        /// </returns>
        public async Task UpdateTaskAsync(ToDoTask toDoTask)
        {
            _context.ToDoTask.Update(toDoTask);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region DeleteTaskAsync

        /// <summary>
        /// Deletes a specific to-do task by its ID.
        /// </summary>
        /// <param name="id">The ID of the to-do task to delete.</param>
        /// <returns>
        /// An asynchronous task that, when completed, removes the task from the database.
        /// </returns>
        public async Task DeleteTaskAsync(int id)
        {
            var toDoTask = await _context.ToDoTask.FindAsync(id);
            if (toDoTask != null)
            {
                _context.ToDoTask.Remove(toDoTask);
                await _context.SaveChangesAsync();
            }
        }

        #endregion

        #region TaskExistsAsync

        /// <summary>
        /// Checks if a specific to-do task exists by its ID.
        /// </summary>
        /// <param name="id">The ID of the to-do task to check.</param>
        /// <returns>
        /// An asynchronous task that, when completed, returns a boolean indicating whether the task exists.
        /// </returns>
        public async Task<bool> TaskExistsAsync(int id)
        {
            return await _context.ToDoTask.AnyAsync(t => t.Id == id);
        }

        #endregion

        #region TaskExistsAsync

        /// <summary>
        /// Retrieves the default status from the database.
        /// </summary>
        /// <returns>
        /// An asynchronous task that, when completed, returns the default <see cref="Status"/>.
        /// </returns>
        public async Task<Status> GetDefaultStatusAsync()
        {
            return await _context.Status.FirstOrDefaultAsync();
        }

        #endregion

        #region SetTaskStatusAsync

        /// <summary>
        /// Sets the status of a specific to-do task.
        /// </summary>
        /// <param name="id">The ID of the to-do task to update.</param>
        /// <param name="statusName">The name of the status to set.</param>
        /// <returns>
        /// An asynchronous task that, when completed, updates the status of the task in the database.
        /// </returns>
        public async Task SetTaskStatusAsync(int id, string statusName)
        {
            var task = await _context.ToDoTask.FindAsync(id);
            if (task != null)
            {
                var status = await _context.Status.FirstOrDefaultAsync(s => s.StatusName == statusName);
                if (status != null)
                {
                    task.StatusId = status.Id;
                    if (statusName == "Done")
                    {
                        task.CompletedDate = DateTime.Now;
                    }
                    else if (statusName == "Pending")
                    {
                        task.CompletedDate = null;
                    }
                    await _context.SaveChangesAsync();
                }
            }
        }

        #endregion

        #region ClearDoneTasksAsync

        /// <summary>
        /// Clears all to-do tasks with the status "Done".
        /// </summary>
        /// <returns>
        /// An asynchronous task that, when completed, removes all tasks marked as "Done" from the database.
        /// </returns>
        public async Task ClearDoneTasksAsync()
        {
            var doneTasks = _context.ToDoTask.Where(t => t.Status.StatusName == "Done");
            _context.ToDoTask.RemoveRange(doneTasks);
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
