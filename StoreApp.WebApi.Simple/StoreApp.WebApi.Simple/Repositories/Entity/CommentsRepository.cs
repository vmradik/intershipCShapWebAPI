using StoreApp.WebApi.Simple.Models;

namespace StoreApp.WebApi.Simple.Repositories.Entity
{
    public class CommentsRepository : Repository<Comment>, ICommentsRepository
    {
        public CommentsRepository(ApplicationDbContext ctx)
            : base(ctx, ctx.Comments)
        {
        }
    }
}