using Moq;
using Microsoft.AspNetCore.Mvc;
using ToDoTaskApplication.Controllers;
using ToDoTaskApplication.Models;
using ToDoTaskApplication.Repositories;

namespace ToDoTaskApplication.Tests
{
    public class ToDoTaskControllerTests
    {
        #region Properties

        private readonly Mock<IToDoTaskRepository> _mockRepo;
        private readonly ToDoTaskController _controller;

        public ToDoTaskControllerTests()
        {
            _mockRepo = new Mock<IToDoTaskRepository>();
            _controller = new ToDoTaskController(_mockRepo.Object);
        }

        #endregion

        #region Index_Test

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfTasks()
        {
            // Arrange
            var tasks = new List<ToDoTask>
            {
                new ToDoTask { Id = 1, Title = "Task 1", Description = "Description 1" },
                new ToDoTask { Id = 2, Title = "Task 2", Description = "Description 2" }
            };
            _mockRepo.Setup(repo => repo.GetAllTasksAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ToDoTask>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        #endregion

        #region Create_Tests

        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_Post_ReturnsRedirectAndAddsTask()
        {
            // Arrange
            var task = new ToDoTask { Title = "Task 1", Description = "Description 1" };
            _mockRepo.Setup(repo => repo.GetDefaultStatusAsync()).ReturnsAsync(new Status { Id = 1, StatusName = "Pending" });
            _mockRepo.Setup(repo => repo.AddTaskAsync(It.IsAny<ToDoTask>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(task);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _mockRepo.Verify(repo => repo.AddTaskAsync(It.IsAny<ToDoTask>()), Times.Once);
        }

        [Fact]
        public async Task Create_Post_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");
            var task = new ToDoTask { Description = "Description 1" };

            // Act
            var result = await _controller.Create(task);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ToDoTask>(viewResult.Model);
            Assert.Equal(task, model);
        }

        [Fact]
        public async Task Create_Post_ReturnsView_WhenDefaultStatusNotFound()
        {
            // Arrange
            var task = new ToDoTask { Title = "Task 1", Description = "Description 1" };
            _mockRepo.Setup(repo => repo.GetDefaultStatusAsync()).ReturnsAsync((Status)null);

            // Act
            var result = await _controller.Create(task);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ToDoTask>(viewResult.Model);
            Assert.Equal(task, model);
            Assert.True(_controller.ModelState.ContainsKey("Status"));
        }

        #endregion

        #region Edit_Tests

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenTaskNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetTaskByIdAsync(It.IsAny<int>())).ReturnsAsync((ToDoTask)null);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithTask()
        {
            // Arrange
            var task = new ToDoTask { Id = 1, Title = "Task 1", Description = "Description 1" };
            _mockRepo.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(task);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ToDoTask>(viewResult.Model);
            Assert.Equal(task, model);
        }

        [Fact]
        public async Task Edit_Post_ReturnsNotFound_WhenIdMismatch()
        {
            // Arrange
            var task = new ToDoTask { Id = 1, Title = "Task 1", Description = "Description 1" };

            // Act
            var result = await _controller.Edit(2, task);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ReturnsNotFound_WhenTaskNotFound()
        {
            // Arrange
            var task = new ToDoTask { Id = 1, Title = "Task 1", Description = "Description 1" };
            _mockRepo.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync((ToDoTask)null);

            // Act
            var result = await _controller.Edit(1, task);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ReturnsRedirect_WhenModelStateIsValid()
        {
            // Arrange
            var task = new ToDoTask { Id = 1, Title = "Task 1", Description = "Description 1" };
            _mockRepo.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(task);
            _mockRepo.Setup(repo => repo.UpdateTaskAsync(It.IsAny<ToDoTask>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(1, task);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _mockRepo.Verify(repo => repo.UpdateTaskAsync(It.IsAny<ToDoTask>()), Times.Once);
        }

        #endregion

        #region Delete_Tests

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenTaskNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetTaskByIdAsync(It.IsAny<int>())).ReturnsAsync((ToDoTask)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithTask()
        {
            // Arrange
            var task = new ToDoTask { Id = 1, Title = "Task 1", Description = "Description 1" };
            _mockRepo.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(task);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ToDoTask>(viewResult.Model);
            Assert.Equal(task, model);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectAndDeletesTask()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteTaskAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            _mockRepo.Verify(repo => repo.DeleteTaskAsync(1), Times.Once);
        }

        #endregion

        #region SetStatus_Tests

        [Fact]
        public async Task SetInProgress_Should_SetStatusToInProgress_AndRedirectToIndex()
        {
            // Arrange
            int taskId = 1;

            // Act
            var result = await _controller.SetInProgress(taskId);

            // Assert
            _mockRepo.Verify(r => r.SetTaskStatusAsync(taskId, "In Progress"), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task SetPending_Should_SetStatusToPending_AndRedirectToIndex()
        {
            // Arrange
            int taskId = 1;

            // Act
            var result = await _controller.SetPending(taskId);

            // Assert
            _mockRepo.Verify(r => r.SetTaskStatusAsync(taskId, "Pending"), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task SetDone_Should_SetStatusToDone_AndRedirectToIndex()
        {
            // Arrange
            int taskId = 1;

            // Act
            var result = await _controller.SetDone(taskId);

            // Assert
            _mockRepo.Verify(r => r.SetTaskStatusAsync(taskId, "Done"), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        #endregion

        #region ClearDoneTasks_Test

        [Fact]
        public async Task ClearDoneTasks_Should_ClearDoneTasks_AndRedirectToIndex()
        {
            // Act
            var result = await _controller.ClearDoneTasks();

            // Assert
            _mockRepo.Verify(r => r.ClearDoneTasksAsync(), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        #endregion
    }
}
