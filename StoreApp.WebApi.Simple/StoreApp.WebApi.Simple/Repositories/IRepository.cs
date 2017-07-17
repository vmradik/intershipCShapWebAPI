using System;
using System.Collections.Generic;
using StoreApp.WebApi.Simple.Models;

namespace StoreApp.WebApi.Simple.Repositories
{
    public interface IRepository<TItem> : IDisposable
        where TItem : IIdEntity
    {
        TItem Get(long id);

        Dictionary<string, object> delCrcRef(object data);
        IEnumerable<Dictionary<string, object>> delCrcRef(IEnumerable<object> lst);
        IEnumerable<TItem> GetAll();

        bool Update(long id, TItem item);

        bool Delete(long id);

        TItem Add(TItem item);


        bool isAdmin(System.Security.Principal.IPrincipal UserApi);
        ApplicationUser getCurrentUser(System.Security.Principal.IPrincipal UserApi);
        //dynamic getDictValue(Dictionary<string, List<dynamic>> dict, string key);

        

    }
}