using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using StoreApp.WebApi.Simple.Models;
using StoreApp.WebApi.Simple.Repositories;

namespace StoreApp.WebApi.Simple.Controllers
{
    [Authorize]
    public class CommentsController : ApiController
    {
        private readonly ICommentsRepository _repo = Services.Instance.Get<ICommentsRepository>();

        // GET: api/Comments
        //[Authorize(Roles = "admins,users", Comments = "sergey,eugene")]
        //[Authorize(Comments = "admin")]




        public IHttpActionResult GetComments()
        {
            if (_repo.isAdmin(User))
                return Json(_repo.delCrcRef(_repo.GetAll().ToList()));
            else
                return Json(_repo.getCurrentUser(User).Comments);
        }

        // GET: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult GetComment(long id)
        {
            Comment comment = _repo.Get(id);
            if (comment == null)
            {
                return NotFound();
            }
            if (_repo.isAdmin(User))
                return Json(_repo.delCrcRef(comment));
            else
            {
                if (comment.Task.Project.Users.Any(x => x.Id == _repo.getCurrentUser(User).Id))
                    return Json(_repo.delCrcRef(comment));
                else
                    return StatusCode(HttpStatusCode.Forbidden);
            }
        }

        // PUT: api/Comments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComment(long id, Comment comment)
        {
            comment.Task = _repo.Get(id).Task;
            comment.User = _repo.Get(id).User;
            if (comment == null)
            {
                return BadRequest();
            }
            if (_repo.isAdmin(User))
            {
                if (!_repo.Update(id, comment)) return NotFound();
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                var userCommentsId = _repo.getCurrentUser(User).Comments;
                if (userCommentsId.Any(x => x.Id == comment.Id))
                {
                    if (!_repo.Update(id, comment)) return NotFound();
                    return StatusCode(HttpStatusCode.NoContent);
                }
                else
                    return StatusCode(HttpStatusCode.Forbidden);
            }
        }

        // POST: api/Comments
        [ResponseType(typeof(Comment))]
        public IHttpActionResult PostComment(Comment comment)
        {
            ITasksRepository _repoTasks = Services.Instance.Get<ITasksRepository>();
            comment.Task = _repoTasks.Get(comment.Id);
            if (_repo.isAdmin(User))
            {
                _repo.Add(comment);
                return CreatedAtRoute("DefaultApi", new { id = comment.Id }, comment);
            }
            else
            {
                if (comment.Task.Project.Users.Any(x => x.Id == _repo.getCurrentUser(User).Id))
                {
                    comment.User = _repo.getCurrentUser(User);
                    _repo.Add(comment);
                    return CreatedAtRoute("DefaultApi", new { id = comment.Id }, comment);
                }
                else
                    return StatusCode(HttpStatusCode.Forbidden);
            }

        }

        // DELETE: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult DeleteComment(long id)
        {
            Comment comment = _repo.Get(id);
            if (comment == null)
            {
                return NotFound();
            }
            if (_repo.isAdmin(User))
            {
                if (!_repo.Delete(id)) return NotFound();
                return Ok();
            }
            else
            {
                if (comment.Task.Project.Users.Any(x => x.Id == _repo.getCurrentUser(User).Id))
                {
                    if (!_repo.Delete(id)) return NotFound();
                    return Ok();
                }
                else
                    return StatusCode(HttpStatusCode.Forbidden);
            }


        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _repo.Dispose();
            base.Dispose(disposing);
        }

    }
}