using StoreApp.WebApi.Simple.Models;

namespace StoreApp.WebApi.Simple.Repositories.Entity
{
    public class ActivitiesRepository : Repository<Activity>, IActivitiesRepository
    {
        public ActivitiesRepository(ApplicationDbContext ctx)
            : base(ctx, ctx.Activities)
        {
        }
    }
}