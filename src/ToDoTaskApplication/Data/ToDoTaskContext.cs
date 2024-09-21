using Microsoft.EntityFrameworkCore;
using ToDoTaskApplication.Models;

namespace ToDoTaskApplication.Data
{
    /// <summary>
    /// Represents the database context for the ToDoTask application.
    /// </summary>
    /// <remarks>
    /// This context is used to interact with the database using Entity Framework Core.
    /// It includes DbSet properties for the ToDoTask and Status entities.
    /// </remarks>
    public class ToDoTaskContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoTaskContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public ToDoTaskContext(DbContextOptions<ToDoTaskContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the DbSet for the ToDoTask entities.
        /// </summary>
        /// <value>
        /// A DbSet representing the collection of ToDoTask entities in the database.
        /// </value>
        public DbSet<ToDoTask> ToDoTask { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for the Status entities.
        /// </summary>
        /// <value>
        /// A DbSet representing the collection of Status entities in the database.
        /// </value>
        public DbSet<Status> Status { get; set; }
    }
}
