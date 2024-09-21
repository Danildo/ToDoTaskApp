using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoTaskApplication.Models;
using ToDoTaskApplication.Repositories;

namespace ToDoTaskApplication.Controllers
{
    /// <summary>
    /// Controller for managing to-do tasks.
    /// </summary>
    /// <remarks>
    /// This controller handles the CRUD operations for to-do tasks, including creating, reading, updating, and deleting tasks.
    /// It interacts with the repository to perform these operations.
    /// </remarks>
    public class ToDoTaskController : Controller
    {
        #region Properties

        private readonly IToDoTaskRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoTaskController"/> class.
        /// </summary>
        /// <param name="repository">The repository used to manage to-do tasks.</param>
        /// <remarks>
        /// The repository is injected via dependency injection to facilitate the separation of concerns and to make the controller more testable.
        /// </remarks>
        public ToDoTaskController(IToDoTaskRepository repository)
        {
            _repository = repository;
        }

        #endregion

        #region Index

        /// <summary>
        /// Retrieves all to-do tasks from the repository and passes them to the view.
        /// </summary>
        /// <returns>
        /// An asynchronous task that, when completed, returns an <see cref="IActionResult"/> 
        /// containing the view with the list of to-do tasks.
        /// </returns>
        public async Task<IActionResult> Index()
        {
            var tasks = await _repository.GetAllTasksAsync();
            return View(tasks);
        }

        #endregion

        #region Create

        /// <summary>
        /// Displays the form to create a new to-do task.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the view for creating a new to-do task.
        /// </returns>
        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Handles the creation of a new to-do task.
        /// </summary>
        /// <param name="toDoTask">The to-do task to create.</param>
        /// <returns>
        /// An asynchronous task that, when completed, returns an <see cref="IActionResult"/> 
        /// containing the view for the newly created to-do task or redirects to the index view.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] ToDoTask toDoTask)
        {
            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                toDoTask.CreatedDate = DateTime.Now;
                var defaultStatus = await _repository.GetDefaultStatusAsync();
                if (defaultStatus == null)
                {
                    ModelState.AddModelError("Status", "Default status not found.");
                    return View(toDoTask);
                }
                toDoTask.Status = defaultStatus;
                await _repository.AddTaskAsync(toDoTask);
                return RedirectToAction(nameof(Index));
            }
            return View(toDoTask);
        }

        #endregion
            
        #region Edit

        /// <summary>
        /// Displays the form to edit an existing to-do task.
        /// </summary>
        /// <param name="id">The ID of the to-do task to edit.</param>
        /// <returns>
        /// An asynchronous task that, when completed, returns an <see cref="IActionResult"/> 
        /// containing the view for editing the to-do task.
        /// </returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoTask = await _repository.GetTaskByIdAsync(id.Value);
            if (toDoTask == null)
            {
                return NotFound();
            }
            return View(toDoTask);
        }


        /// <summary>
        /// Handles the update of an existing to-do task.
        /// </summary>
        /// <param name="id">The ID of the to-do task to update.</param>
        /// <param name="toDoTask">The updated to-do task.</param>
        /// <returns>
        /// An asynchronous task that, when completed, returns an <see cref="IActionResult"/> 
        /// containing the view for the updated to-do task or redirects to the index view.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] ToDoTask toDoTask)
        {
            if (id != toDoTask.Id)
            {
                return NotFound();
            }

            var existingTask = await _repository.GetTaskByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = toDoTask.Title;
            existingTask.Description = toDoTask.Description;

            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.UpdateTaskAsync(existingTask);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _repository.TaskExistsAsync(toDoTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(toDoTask);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Retrieves the details of a specific to-do task by its ID for deletion confirmation.
        /// </summary>
        /// <param name="id">The ID of the to-do task to delete.</param>
        /// <returns>
        /// An asynchronous task that, when completed, returns an <see cref="IActionResult"/> 
        /// containing the view with the details of the to-do task for deletion confirmation.
        /// </returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoTask = await _repository.GetTaskByIdAsync(id.Value);
            if (toDoTask == null)
            {
                return NotFound();
            }

            return View(toDoTask);
        }


        /// <summary>
        /// Confirms and handles the deletion of a specific to-do task by its ID.
        /// </summary>
        /// <param name="id">The ID of the to-do task to delete.</param>
        /// <returns>
        /// An asynchronous task that, when completed, returns an <see cref="IActionResult"/> 
        /// that redirects to the index view.
        /// </returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// Checks if a specific to-do task exists by its ID.
        /// </summary>
        /// <param name="id">The ID of the to-do task to check.</param>
        /// <returns>
        /// An asynchronous task that, when completed, returns a boolean indicating whether the task exists.
        /// </returns>
        private async Task<bool> ToDoTaskExists(int id)
        {
            return await _repository.TaskExistsAsync(id);
        }

        #endregion

        #region SetInProgress

        /// <summary>
        /// Sets the status of a specific to-do task to "In Progress".
        /// </summary>
        /// <param name="id">The ID of the to-do task to update.</param>
        /// <returns>
        /// An asynchronous task that, when completed, returns an <see cref="IActionResult"/> 
        /// that redirects to the index view.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> SetInProgress(int id)
        {
            await _repository.SetTaskStatusAsync(id, "In Progress");
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region SetPending

        /// <summary>
        /// Sets the status of a specific to-do task to "Pending".
        /// </summary>
        /// <param name="id">The ID of the to-do task to update.</param>
        /// <returns>
        /// An asynchronous task that, when completed, returns an <see cref="IActionResult"/> 
        /// that redirects to the index view.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> SetPending(int id)
        {
            await _repository.SetTaskStatusAsync(id, "Pending");
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region SetDone

        /// <summary>
        /// Sets the status of a specific to-do task to "Done".
        /// </summary>
        /// <param name="id">The ID of the to-do task to update.</param>
        /// <returns>
        /// An asynchronous task that, when completed, returns an <see cref="IActionResult"/> 
        /// that redirects to the index view.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> SetDone(int id)
        {
            await _repository.SetTaskStatusAsync(id, "Done");
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ClearDoneTasks

        /// <summary>
        /// Clears all to-do tasks with the status "Done".
        /// </summary>
        /// <returns>
        /// An asynchronous task that, when completed, returns an <see cref="IActionResult"/> 
        /// that redirects to the index view.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> ClearDoneTasks()
        {
            await _repository.ClearDoneTasksAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
