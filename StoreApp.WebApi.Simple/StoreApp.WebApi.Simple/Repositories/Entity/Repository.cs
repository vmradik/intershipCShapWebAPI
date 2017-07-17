using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using StoreApp.WebApi.Simple.Models;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace StoreApp.WebApi.Simple.Repositories.Entity
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, IIdEntity
    {
        private readonly DbContext _ctx;
        private readonly DbSet<T> _table;







        // очень тёмное колдунство

        public IEnumerable<Dictionary<string, object>> delCrcRef(IEnumerable<object> lst)
        {
            return lst.Select(x => delCrcRef(x));
        }

        public Dictionary<string, object> delCrcRef(object data)
        {
            Dictionary<string, object> rez = new Dictionary<string, object>();
            foreach (var p in typeof(T).GetProperties())
            {
                var value = getObjValue(data,p);
                if (value != null)
                {
                    var strValue = value.ToString();
                    if (strValue.Contains("System.Collections"))
                        rez.Add(toCamel(p.Name), interfaceIdIE(value));
                    else if (!strValue.Contains("System.Data.Entity.DynamicProxies"))
                        rez.Add(toCamel(p.Name), value);
                }
            }
            return rez;
        }

        // динамик каст форева
        public List<dynamic> interfaceIdIE( object x)
        {
            List<dynamic> rez = new List<dynamic>();
            var b = ((IEnumerable)x).Cast<dynamic>();
            foreach (var elem in b)
            {
                rez.Add(elem.Id);
            }
            return rez;
        }

        public object getObjValue(object data, System.Reflection.PropertyInfo x)
        {
            return x.GetGetMethod().Invoke(data, null);
        }



        // когда очень хотелось сделать в рукопашку
        public string toCamel(string str) // нравится название))
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1, str.Length-1);
        }


                // вот прям очень кровавое вуду
        private readonly ApplicationUserManager _repoUsers = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public ApplicationUser getCurrentUser(System.Security.Principal.IPrincipal User)
        {
            ApplicationUser user = _repoUsers.FindByName(User.Identity.Name);
            return user;
        }
        public bool isAdmin(System.Security.Principal.IPrincipal User)
        {
            ApplicationUser user = getCurrentUser(User);
            return (user != null) ? _repoUsers.IsInRole(user.Id, "admin") : false;
        }




        //public List<dynamic> objToNode(object obj)
        //{
        //    List<dynamic> rez = new List<dynamic>();
        //    rez.Add(obj);
        //    return rez;
        //}
        //public List<dynamic> getObjNode(object data, System.Reflection.PropertyInfo x)
        //{
        //    return objToNode(getObjValue(data, x));
        //}
        //public List<dynamic> getObjStr(object data, System.Reflection.PropertyInfo x)
        //{
        //    List<dynamic> rez = new List<dynamic>();
        //    rez.Add(x.GetGetMethod().Invoke(data, null));
        //    return rez;
        //}
        //public dynamic getDictValue(Dictionary<string, List<dynamic>> dict, string key)
        //{
        //    List<dynamic>  value = new List<dynamic>();
        //    dict.TryGetValue("tif", out value);
        //    return value.First();
        //}





        protected Repository(DbContext ctx, DbSet<T> table)
        {
            _ctx = ctx;
            _table = table;
        }

        public T Add(T item)
        {
            T res = _table.Add(item);
            try
            {
                _ctx.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                System.Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new System.InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            return res;
        }

        public bool Delete(long id)
        {
            T item = Get(id);
            if (item == null) return false;

            _table.Remove(item);
            _ctx.Dispose();
            return true;
        }

        public T Get(long id)
        {
            return _table.Find(id);
        }



        public IEnumerable<T> GetAll()
        {
            return _table;
        }

        public bool Update(long id, T item)
        {
            _ctx.Entry(item).State = EntityState.Modified;

            try
            {
                _ctx.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return false;
                }

                throw;
            }

            return true;
        }
        private bool ProductExists(long id)
        {
            return _table.Count(e => e.Id == id) > 0;
        }

        public void Dispose()
        {
            _ctx?.Dispose();
        }
    }
}