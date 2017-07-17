using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using StoreApp.WebApi.Simple.Models;
using StoreApp.WebApi.Simple.Repositories;
using StoreApp.WebApi.Simple.Repositories.Entity;
using WebApiApp.Filters;

namespace StoreApp.WebApi.Simple
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //   config.Filters.Add(new CustomAuthenticationAttribute());
            //   config.Filters.Add(new CustomAuthorizationAttribute());

            config.Filters.Add(new IdentityBasicAuthenticationAttribute());

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            Services.Instance.Add<IProjectsRepository>(() => new ProjectsRepository(new ApplicationDbContext()));
            Services.Instance.Add<ITasksRepository>(() => new TasksRepository(new ApplicationDbContext()));
            //Services.Add<IUsersRepository>(() => new UsersRepository(new ApplicationDbContext()));
            Services.Instance.Add<IActivitiesRepository>(() => new ActivitiesRepository(new ApplicationDbContext()));
            Services.Instance.Add<ICommentsRepository>(() => new CommentsRepository(new ApplicationDbContext()));
        }
    }
}
