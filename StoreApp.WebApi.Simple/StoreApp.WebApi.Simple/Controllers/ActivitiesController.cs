using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using StoreApp.WebApi.Simple.Models;
using StoreApp.WebApi.Simple.Repositories;

namespace StoreApp.WebApi.Simple.Controllers
{
    [Authorize]
    [RoutePrefix("api/Activities")]
    public class ActivitiesController : ApiController
    {
        private readonly IActivitiesRepository _repo = Services.Instance.Get<IActivitiesRepository>();

        // GET: api/Actions
        //[Authorize(Roles = "admins,users", Actions = "sergey,eugene")]
        //[Authorize(Actions = "admin")]




        public IHttpActionResult GetActivities()
        {
            if (_repo.isAdmin(User))
                return Json(_repo.delCrcRef(_repo.GetAll().ToList()));
            else
                return Json(_repo.getCurrentUser(User).Comments);
        }

        // GET: api/Actions/5
        [ResponseType(typeof(Activity))]
        public IHttpActionResult GetActivities(long id)
        {
            Activity activity = _repo.Get(id);
            if (activity == null)
            {
                return NotFound();
            }
            if (_repo.isAdmin(User))
                return Json(_repo.delCrcRef(activity));
            else
            {
                if (activity.Task.Project.Users.Any(x => x.Id == _repo.getCurrentUser(User).Id))
                    return Json(_repo.delCrcRef(activity));
                else
                    return StatusCode(HttpStatusCode.Forbidden);
            }

        }


        // PUT: api/Actions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutActivities(long id, Activity activity)
        {
            if (activity == null || !isValid(activity))
            {
                return BadRequest();
            }
            else
            {
                var actOld= _repo.Get(id);
                if (actOld == null)
                    return NotFound();
                activity.Task = actOld.Task;
                activity.User = actOld.User;

                if (_repo.isAdmin(User))
                {
                    if (!_repo.Update(id, activity)) return NotFound();
                    return StatusCode(HttpStatusCode.NoContent);
                }
                else
                {
                    var userCommentsId = _repo.getCurrentUser(User).Comments;
                    if (userCommentsId.Any(x => x.Id == activity.Id))
                    {
                        if (!_repo.Update(id, activity)) return NotFound();
                        return StatusCode(HttpStatusCode.NoContent);
                    }
                    else
                        return StatusCode(HttpStatusCode.Forbidden);
                }
            }
        }

        // POST: api/Actions
        [ResponseType(typeof(Activity))]
        public IHttpActionResult PostActivities(Activity activity)
        {
            if (activity == null || !isValid(activity))
            {
                return BadRequest();
            }
            else
            {
                ITasksRepository _repoTasks = Services.Instance.Get<ITasksRepository>();
                activity.Task = _repoTasks.Get(activity.Id);
                if (_repo.isAdmin(User))
                {
                    _repo.Add(activity);
                    return Ok();
                   // return Created("/api/Users/" + activity.Id, activity);
                }
                else
                {
                    if (activity.Task.Project.Users.Any(x => x.Id == _repo.getCurrentUser(User).Id))
                    {
                        activity.User = _repo.getCurrentUser(User);
                        _repo.Add(activity);

                        return Ok();
                       // return Created("/api/Users/" + activity.Id, activity);
                    }
                    else
                        return StatusCode(HttpStatusCode.Forbidden);
                }
            }
        }

        // DELETE: api/Actions/5
        [ResponseType(typeof(Activity))]
        public IHttpActionResult DeleteActivities(long id)
        {
            Activity activity = _repo.Get(id);
            if (activity == null)
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
                if (activity.Task.Project.Users.Any(x => x.Id == _repo.getCurrentUser(User).Id))
                {
                    if (!_repo.Delete(id)) return NotFound();
                    return Ok();
                }
                else
                    return StatusCode(HttpStatusCode.Forbidden);
            }
        }


        public bool isValid(Activity act)
        {
            if (act.Level > 1 || act.Level < 0)
                return false;
            else if (System.DateTime.Compare(act.End, act.Begin.AddMinutes(30)) >0)
                return false;
            else if (System.DateTime.Compare(System.DateTime.Now, act.Begin.AddDays(1)) >0)
                return false;
            else
                return true;
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