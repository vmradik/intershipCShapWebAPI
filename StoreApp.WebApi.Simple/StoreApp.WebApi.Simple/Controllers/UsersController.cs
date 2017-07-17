using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using StoreApp.WebApi.Simple.Models;
using StoreApp.WebApi.Simple.Repositories;

namespace StoreApp.WebApi.Simple.Controllers
{
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private readonly ApplicationUserManager _repo = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

        // GET: api/Users
        protected ApplicationUser getCurrentUser()
        {
            ApplicationUser user = _repo.FindByName(User.Identity.Name);
            return user;
        }
        protected bool isAdmin()
        {
            ApplicationUser user = _repo.FindByName(User.Identity.Name);
            return (user != null)?_repo.IsInRole(user.Id, "admin"):false;
        }

        public IHttpActionResult GetUsers()
        {
            if (isAdmin())
            {
                var users = _repo.Users.ToList();
                return Json(DelCrcRef(users, true));
            }
            else return Json(DelCrcRef(getCurrentUser(),false));
        }

        // GET: api/Users/5
        [ResponseType(typeof(System.Web.Http.Results.StatusCodeResult))]
        public IHttpActionResult GetUser(string id)
        {
          var user = _repo.FindByIdAsync(id).Result;
          if (user == null)
               return NotFound();
          return Json(DelCrcRef(user, (id==getCurrentUser().Id)?true:isAdmin()));
        }




        //// PUT: api/Users/5
        [ResponseType(typeof(System.Web.Http.Results.StatusCodeResult))]
        public IHttpActionResult PutUser( ApplicationUser newUser)
        {
            if (isAdmin())
                return updateUser(newUser);
            else
            {
                var oldUser = getCurrentUser();
                if (newUser.Id != oldUser.Id)
                    return StatusCode(HttpStatusCode.Forbidden);
                else
                {
                    oldUser.Email = newUser.Email;
                    oldUser.UserName = newUser.UserName;
                    oldUser.About = newUser.About;
                    return updateUser(oldUser);
                }
            }            
        }

        public dynamic updateUser(ApplicationUser newUser)
        {
            if (_repo.UpdateAsync(newUser).Result.Succeeded)
                return StatusCode(HttpStatusCode.NoContent);
            else
                return BadRequest();
        }




        //[POST: api/Users]
        [Authorize(Roles = "admin")]
        [ResponseType(typeof(System.Web.Http.Results.StatusCodeResult))]
        public IHttpActionResult PostUser(ApplicationUser user, string password)
        {
            var result = _repo.Create(user, password);
            if (result.Succeeded)
                return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
            else
                return BadRequest();
           
        }

        //[POST: api/Users/adduser]
        [Authorize(Roles = "admin")]
        [Route("adduser")]
        [ResponseType(typeof(System.Web.Http.Results.StatusCodeResult))]
        public IHttpActionResult PostCreateUser(userRegInfo userAdd)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = userAdd.userName,
                Email = userAdd.email,
                Name = userAdd.name,
                About = userAdd.about,

            };
            var result = _repo.Create(user, userAdd.password);

            // если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                var roleManager = new RoleManager<IdentityRole>(
                            new RoleStore<IdentityRole>(HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>()));
                foreach (var role in userAdd.roles)
                    if(roleManager.FindByName(role)!=null)
                         _repo.AddToRole(user.Id, role);
            }
            if (result.Succeeded)
                return Created("/api/Users/" + user.Id, user);
            else
                return BadRequest();


        }
        
        public struct userRegInfo
        {
            public string name;
            public string about;
            public string userName;
            public string password;
            public string[] roles;
            public string email;
        }



        // DELETE: api/Users/5
        [Authorize(Roles = "admin")]
        [ResponseType(typeof(System.Web.Http.Results.StatusCodeResult))]
        public IHttpActionResult DeleteUser(string id)
        {
            if (_repo.GetLockoutEnabledAsync(id).Result)
                return Ok();
             return BadRequest();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }
            base.Dispose(disposing);
        }


//      ларец с инструментами 
        // динамик каст форева
        public List<dynamic> interfaceIdIE(object x)
        {
            List<dynamic> rez = new List<dynamic>();
            var b = ((IEnumerable)x).Cast<dynamic>();
            foreach (var elem in b)
            {
                rez.Add(elem.Id);
            }
            return rez;
        }
      
        private List<string> RolesToString(ApplicationUser data)
        {
            ApplicationUser user = _repo.FindByEmail(data.Email);

            // велосипед пригодился
            IList<string> roles = new List<string>();
            List<string> rez = new List<string>();
            if (user != null)
            {
                roles = _repo.GetRoles(user.Id);
                foreach(var elem in roles)
                {
                    rez.Add(elem);
                }
            }
            return rez;

        }
        private Dictionary<string, object> DelCrcRef(ApplicationUser data, bool fool)
        {
            Dictionary<string, object> rez = new Dictionary<string, object>();
            if (fool)
            {
                rez.Add("Email", data.Email);
                rez.Add("Projects", interfaceIdIE(data.Projects));
                rez.Add("Comments", interfaceIdIE(data.Comments));
                rez.Add("Actions", interfaceIdIE(data.Activities));
            }
            rez.Add("Name", data.Name);
            rez.Add("Id", data.Id);
            rez.Add("Roles", RolesToString(data));
            rez.Add("About", data.About);
            rez.Add("UserName", data.UserName);

            return rez;
        }
        private IEnumerable<Dictionary<string, object>> DelCrcRef(IEnumerable<ApplicationUser> lst, bool fool)
        {
            return lst.Select(x => DelCrcRef(x, fool));
        }


    }
}