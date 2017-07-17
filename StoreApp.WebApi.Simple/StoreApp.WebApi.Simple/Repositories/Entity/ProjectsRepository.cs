using StoreApp.WebApi.Simple.Models;

namespace StoreApp.WebApi.Simple.Repositories.Entity
{
    public class ProjectsRepository : Repository<Project>, IProjectsRepository
    {
        public ProjectsRepository(ApplicationDbContext ctx)
            : base(ctx, ctx.Projects)
        {
        }
    }
}