using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using StoreApp.WebApi.Simple.Models;
using StoreApp.WebApi.Simple.Repositories;
using StoreApp.WebApi.Simple.Repositories.Entity;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StoreApp.WebApi.Simple
{


    // КОНТРОЛЛЕР ДЛЯ БАЗОВОЙ ИНИЦИАЛИЗАЦИИ ЗНАЧЕНИЙ
    public class InitsController : ApiController
    {

        //private readonly IUsersRepository _repoUser = Services.Instance.Get<IUsersRepository>();

        private readonly ApplicationUserManager _repoUser = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private readonly IProjectsRepository _repoProject = Services.Instance.Get<IProjectsRepository>();

        private readonly ITasksRepository _repoTask = Services.Instance.Get<ITasksRepository>();
        private readonly ICommentsRepository _repoComment = Services.Instance.Get<ICommentsRepository>();
        private readonly IActivitiesRepository _repoAction = Services.Instance.Get<IActivitiesRepository>();


        // GET: api/Inits
        //[Authorize(Roles = "admins,users", Inits = "sergey,eugene")]
        //[Authorize(Inits = "admin")]




        public IHttpActionResult GetInits()
        {
            // создаем две роли
            var adminRole = new IdentityRole { Name = "admin" };
            var userRole = new IdentityRole { Name = "user" };

            /*
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>()));
            


            
            // добавляем роли в бд
            roleManager.Create(adminRole);
            roleManager.Create(userRole);

            
            
            ApplicationUser user = new ApplicationUser()
            {
                UserName = "user@user.ru",
                Email = "user@user.ru",
                Name = "user",
                About = "user",
                
            };
            var result = _repoUser.Create(user, user.UserName);

            // если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
             //   _repoUser.AddToRole(admin.Id, adminRole.Name);
                _repoUser.AddToRole(user.Id, userRole.Name);
            }
            */
            /*
            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "root@root.ru",
                Email = "root@root.ru",
                Name = "root",
                About = "root",

            };
            var result2 = _repoUser.Create(admin, admin.UserName);

            // если создание пользователя прошло успешно
            if (result2.Succeeded)
            {
                // добавляем для пользователя роль
                 _repoUser.AddToRole(admin.Id, adminRole.Name);
                _repoUser.AddToRole(admin.Id, userRole.Name);
            }
            return Json("asd");
            */
            /*
                        Project project = new Project();
                        project.Name = "FirstProj";
                        _repoProject.Add(project);


            
            Task task = new Task();
            task.Name = "test";
            task.About = "some task";
            task.ProjectId = 1;
            _repoTask.Add(task);

            Comment comment = new Comment();
            comment.UserId = "da406aa7-741d-43a3-be83-de06f4575ee9";
            comment.Value = "some test comment";
            comment.TaskId = 1;
            _repoComment.Add(comment);

            Models.Activity activity = new Models.Activity();
            activity.Id = 1;
            activity.Begin = DateTime.Now;
            activity.End = DateTime.Now;
            activity.TaskId = 1;
            activity.UserId = "da406aa7-741d-43a3-be83-de06f4575ee9";
            activity.Level = 1;
            _repoAction.Add(activity);

 

            return Json(_repoProject.delCrcRef(_repoProject.GetAll()));
            */
            return Json("adads");
          
        }

        // GET: api/Inits/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult GetInit(long id)
        {
            return Json(_repoProject.GetAll());
        }

        // PUT: api/Inits/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInit(long id)
        {
            return Json(_repoProject.GetAll());
        }

        // POST: api/Inits
        [ResponseType(typeof(Project))]
        public IHttpActionResult PostInit()
        {
            return Json(_repoProject.GetAll());
        }

        // DELETE: api/Inits/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteInit(long id)
        {
            return Json(_repoProject.GetAll());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repoProject.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}