namespace ToDoTaskApplication.Migrations
{
    public static class MigrationMiddlewareExtensions
    {
        public static IApplicationBuilder UseMigrationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MigrationMiddleware>();
        }
    }

}
