using CustomAuthMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CustomAuthMVC.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(SessionPersister.Username))
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new {
                        controller = "Account",
                        action = "Index"
                    }));
            else
            {
                AccountModel am = new AccountModel();
                CustomPrincipal mp = new CustomPrincipal(am.find(SessionPersister.Username));
                if (!mp.IsInRole(Roles))
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new {
                            controller = "AccessDenied",
                            action = "Index"
                        }));
            }
        }
    }
}