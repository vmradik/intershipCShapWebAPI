using StoreApp.WebApi.Simple.Models;

namespace StoreApp.WebApi.Simple.Repositories.Entity
{
    public class TasksRepository : Repository<Task>, ITasksRepository
    {
        public TasksRepository(ApplicationDbContext ctx)
            : base(ctx, ctx.Tasks)
        {
        }
    }
}