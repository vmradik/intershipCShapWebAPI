using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApiApp.Filters
{
    public class CustomAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private string[] usersList;
        public CustomAuthorizationAttribute(params string[] users)
        {
            this.usersList = users;
        }
        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext,
                        CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            IPrincipal principal = actionContext.RequestContext.Principal;
            if (principal == null || !usersList.Contains(principal.Identity.Name))
            {
                return Task.FromResult<HttpResponseMessage>(
                       actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized));
            }
            else
            {
                return continuation();
            }
        }
        public bool AllowMultiple
        {
            get { return false; }
        }
    }
}