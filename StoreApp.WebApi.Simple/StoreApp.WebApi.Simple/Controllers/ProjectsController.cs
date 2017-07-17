using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity.Owin;
using StoreApp.WebApi.Simple.Models;
using StoreApp.WebApi.Simple.Repositories;

namespace StoreApp.WebApi.Simple.Controllers
{
    [Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : ApiController
    {
        private readonly IProjectsRepository _repo = Services.Instance.Get<IProjectsRepository>();

        // GET: api/Projects
        public IHttpActionResult GetProjects()
        {
            var projects = _repo.GetAll().ToList();
            if(_repo.isAdmin(User))
            return Json(_repo.delCrcRef((projects)));

            else {
                return Json(_repo.getCurrentUser(User).Projects);
            }
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult GetProject(long id)
        {
            Project project = _repo.Get(id);
            
            if (project == null)
            {
                return NotFound();
            }
            if (_repo.isAdmin(User) || project.Users.Any(y => y.Id == _repo.getCurrentUser(User).Id))
            {
                return Json(_repo.delCrcRef(project));
            }
            else
                return StatusCode(HttpStatusCode.Forbidden);
        }

        // GET: api/Projects/{id}/tasks
        [ResponseType(typeof(Project))]
        [Route("{id}/tasks")]
        public IHttpActionResult GetProjectTasks(long id)
        {
            Project project = _repo.Get(id);

            if (project == null)
            {
                return NotFound();
            }
            if (_repo.isAdmin(User) || project.Users.Any(y => y.Id == _repo.getCurrentUser(User).Id))
                return Json((Services.Instance.Get<ITasksRepository>()).delCrcRef(project.Tasks));
            else
                return StatusCode(HttpStatusCode.Forbidden);
        }

        // сугубо для теста, ОПАСНО 
        // добавление пользователя на проект через гет запрос - для удобства при тестировании
        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        [Route("{projId}/{userId}")]
        [Authorize(Roles = "admin")]
        public IHttpActionResult GetGiveProject(long projId,string userId)
        {
            ApplicationUserManager _repoUser = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            Project project = _repo.Get(projId);
            project.Users.Add(_repoUser.FindByIdAsync(userId).Result);
            if (!_repo.Update(projId, project)) return BadRequest();
            return StatusCode(HttpStatusCode.OK);
        }



        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "admin")]
        public IHttpActionResult PutProject(long id, [FromBody]string Name)
        {
            Project project = _repo.Get(id);
            project.Name = Name;
            if (!_repo.Update(id, project)) return NotFound();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [ResponseType(typeof(Project))]
        [Route("{projId}/adduser")]
        [Authorize(Roles = "admin")]
        public IHttpActionResult PutProject_give(long projId, [FromBody]string userId)
        {

            Project project = _repo.Get(projId);
            ApplicationUserManager _repoUser = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var newUser = _repoUser.FindByIdAsync(userId).Result;
            newUser.Projects.Add(project);
            //project.Users.Add(_repoUser.FindByIdAsync(userId).Result);
            if (_repoUser.UpdateAsync(newUser).Result.Succeeded)
                return StatusCode(HttpStatusCode.NoContent);
            else
                return BadRequest();
           // if (!_repo.Update(projId, project)) return BadRequest();
            //return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/Projects
        [Authorize(Roles = "admin")]
        [ResponseType(typeof(Project))]
        public IHttpActionResult PostProject(Project project)
        {
            _repo.Add(project);
            return CreatedAtRoute("DefaultApi", new { id = project.Id }, project);
        }

        // DELETE: api/Projects/5
        [Authorize(Roles = "admin")]
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteProject(long id)
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