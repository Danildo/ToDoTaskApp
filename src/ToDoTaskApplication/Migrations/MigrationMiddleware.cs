using Microsoft.EntityFrameworkCore;
using ToDoTaskApplication.Data;

namespace ToDoTaskApplication.Migrations
{
    public class MigrationMiddleware
    {
        private readonly RequestDelegate _next;

        public MigrationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ToDoTaskContext dbContext)
        {
            await dbContext.Database.MigrateAsync();

            await _next(context);
        }
    }
}
