using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using StoreApp.WebApi.Simple.Models;
using StoreApp.WebApi.Simple.Repositories;

namespace StoreApp.WebApi.Simple.Controllers
{
    [Authorize]
    [RoutePrefix("api/Tasks")]
    public class TasksController : ApiController
    {
        private readonly ITasksRepository _repo = Services.Instance.Get<ITasksRepository>();
        private readonly IProjectsRepository _repoProj = Services.Instance.Get<IProjectsRepository>();

        // GET: api/Tasks
        //[Authorize(Roles = "admins,users", Users = "sergey,eugene")]
        [ResponseType(typeof(Task))]
        public IHttpActionResult GetTasks()
        {
            var tasks = _repo.GetAll().ToList();
            
            if (_repo.isAdmin(User))
                return Json(_repo.delCrcRef(tasks));
            else
            {
                var userProjectsId = _repo.getCurrentUser(User).Projects.Select(x=>x.Id);
                var taskUser = tasks.Where(x=>userProjectsId.Any(y=>y==x.ProjectId));
                return Json(_repo.delCrcRef(taskUser));
            }
        }

        // GET: api/Tasks/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult GetTask(long id)
        {
            Task task = _repo.Get(id);
            if (task == null)
                return NotFound();
            else if (_repo.isAdmin(User))
                return Json(_repo.delCrcRef(task));
            else
            {
                var userProjectsId = _repo.getCurrentUser(User).Projects.Select(x => x.Id);
                if (userProjectsId.Any(x => x == task.ProjectId))
                    return Json(_repo.delCrcRef(task));
                else
                    return StatusCode(HttpStatusCode.Forbidden);
            }
        }
        // GET: api/Tasks/5
        [ResponseType(typeof(Task))]
        [Route("{id}/comments")]
        public IHttpActionResult GetAllComments(long id)
        {
            Task task = _repo.Get(id);
            if (task == null)
                return NotFound();
            else if (_repo.isAdmin(User))
                return Json(Services.Instance.Get<ICommentsRepository>().delCrcRef(task.Comments));
            else
            {
                var userProjectsId = _repo.getCurrentUser(User).Projects.Select(x => x.Id);
                if (userProjectsId.Any(x => x == task.ProjectId))
                    return Json(Services.Instance.Get<ICommentsRepository>().delCrcRef(task.Comments));
                else
                    return StatusCode(HttpStatusCode.Forbidden);
            }
        }
        [ResponseType(typeof(Task))]
        [Route("{id}/activities")]
        public IHttpActionResult GetAllActive(long id)
        {
            Task task = _repo.Get(id);
            if (task == null)
                return NotFound();
            else if (_repo.isAdmin(User))
                return Json(Services.Instance.Get<IActivitiesRepository>().delCrcRef(task.Activities));
            else
            {
                var userProjectsId = _repo.getCurrentUser(User).Projects.Select(x => x.Id);
                if (userProjectsId.Any(x => x == task.ProjectId))
                    return Json(Services.Instance.Get<IActivitiesRepository>().delCrcRef(task.Activities));
                else
                    return StatusCode(HttpStatusCode.Forbidden);
            }
        }

        // PUT: api/Tasks/5
        [Authorize(Roles = "admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTask(long id, Task task)
        {
            if (!_repo.Update(id, task)) return NotFound();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tasks
        [Authorize(Roles = "admin")]
        [ResponseType(typeof(Task))]
        public IHttpActionResult PostTask(Task task)
        {
            _repo.Add(task);

            return CreatedAtRoute("DefaultApi", new { id = task.Id }, task);
        }

        // DELETE: api/Tasks/5
        [Authorize(Roles = "admin")]
        [ResponseType(typeof(Task))]
        public IHttpActionResult DeleteTask(long id)
        {
            if (!_repo.Delete(id)) return NotFound();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}