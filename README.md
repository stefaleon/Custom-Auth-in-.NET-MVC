# Custom Authentication and Authorization in a .NET MVC Web Application
### As demonstrated in [Custom Authentication and Authorization with Session in ASP.NET MVC](https://www.youtube.com/watch?v=iNSy97kqGQY)
### by [Learning Programming](https://www.youtube.com/channel/UCPSDAF3Htm3AIxw4OUM3Lew)


&nbsp;
## 00 Start project

* New ASP.NET Web Application.
* MVC, No Authentication



&nbsp;
## 01 Account controller and view

* Remove the auto generated controllers, models and views.

* Add the empty MVC5 *AccountController* and the *Account/Index* in the related view.

*Controllers/AccountController.cs*
```
using System.Web.Mvc;

namespace CustomAuthMVC.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
    }
}
```

*Views/Account/Index.cshtml*
```
@{
    Layout = null;
}

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Index</title>
</head>
<body>
    <div>
    </div>
</body>
</html>
```



&nbsp;
## 02 Models

* Add the *Account* class.

*Models/Account.cs*
```
using System.ComponentModel.DataAnnotations;

namespace CustomAuthMVC.Models
{
    public class Account
    {
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        public string[] Roles { get; set; }
    }
}
```


* Add the *AccountModel* class. Define three demo accounts in the constructor and the *find* and *login* methods that return *Account* objects.

*Models/AccountModel.cs*
```
using System.Collections.Generic;
using System.Linq;

namespace CustomAuthMVC.Models
{
    public class AccountModel
    {
        private List<Account> listAccounts = new List<Account>();

        public AccountModel()
        {
            listAccounts.Add(new Account
            {
                UserName = "acc1",
                Password = "123",
                Roles = new string[] { "superadmin", "admin", "employee" }
            });
            listAccounts.Add(new Account
            {
                UserName = "acc2",
                Password = "123",
                Roles = new string[] { "admin", "employee" }
            });
            listAccounts.Add(new Account
            {
                UserName = "acc3",
                Password = "123",
                Roles = new string[] { "employee" }
            });
        }

        public Account find(string username)
        {
            return listAccounts.Where(acc => acc.UserName.Equals(username)).FirstOrDefault(); ;
        }

        public Account login(string username, string password)
        {
            return listAccounts.Where(acc => acc.UserName.Equals(username) && acc.Password.Equals(password)).FirstOrDefault();
        }
    }
}
```


&nbsp;
## 03 Display the Account Index View in the default route

* In *App_Start/RouteConfig.cs* set the default route controller to the *AccountController*.

```
defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
```


* Add the *ViewModels* folder and the *AccountViewModel* class.

*ViewModels/AccountViewModel.cs*
```
using CustomAuthMVC.Models;

namespace CustomAuthMVC.ViewModels
{
    public class AccountViewModel
    {
        public Account Account { get; set; }
    }
}
```

* Edit the *Account* index view in order to receive the *AccountViewModel* and display a login form.

*Views/Account/Index.cshtml*
```
@model CustomAuthMVC.ViewModels.AccountViewModel

@{
    Layout = null;
}

<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>AccountIndex</title>
</head>
<body>
    @using (Html.BeginForm("Login", "Account", FormMethod.Post))
    {
        @ViewBag.Error
        <table cellpadding="2" cellspacing="2">
            <tr>
                <td>@Html.LabelFor(model => model.Account.UserName)</td>
                <td>@Html.TextBoxFor(model => model.Account.UserName)</td>
            </tr>
            <tr>
                <td>@Html.LabelFor(model => model.Account.Password)</td>
                <td>@Html.PasswordFor(model => model.Account.Password)</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><input type="submit" value="Login"></td>
            </tr>
        </table>
    }
</body>
</html>
```


&nbsp;
## 04 Login method with Forms Authentication

* In the project's *Web.config*, inside *<configuration>/<system.web>*, add the *<authentication>* tag, directing to the *Account* index view for the login url.

```
    <authentication mode="Forms">
      <forms loginUrl="~/Account/Index">
      </forms>
    </authentication>
```

* Add the *Login* method in the *Account* controller.

```
using CustomAuthMVC.Models;
using CustomAuthMVC.ViewModels;
```
```
        [HttpPost]
        public ActionResult Login(AccountViewModel avm)
        {
            AccountModel am = new AccountModel();
            if (string.IsNullOrEmpty(avm.Account.UserName) ||
                string.IsNullOrEmpty(avm.Account.Password) ||
                am.login(avm.Account.UserName, avm.Account.Password) == null)
            {
                ViewBag.Error = "Login failed.";
                return View("Index");
            }            
            return View("Success");
        }
```


* Add the *Success* view.

*Views/Account/Success.cshtml*
```
@{
    Layout = null;
}

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Success</title>
</head>
<body>
    <div>
        The login was successful.
    </div>
</body>
</html>
```



&nbsp;
## 05 Custom Principal

* Add the *Security* folder.

* Add the *CustomPrincipal* class which inherits from *IPrincipal*. The custom principal constructor takes an account parameter and defines a generic identity for the user name of the account. The roles attached to this account are defined in the implementation of the *IsInRole* method, which is required by the *IPrincipal* interface.

*Security/CustomPrincipal.cs*
```
using SecurityWithASPNETMVC.Models;
using System.Linq;
using System.Security.Principal;


namespace SecurityWithASPNETMVC.Security
{
    public class CustomPrincipal : IPrincipal
    {
        private Account Account;        

        public CustomPrincipal(Account account)
        {
            Account = account;
            Identity = new GenericIdentity(account.UserName);        
        }

        public IIdentity Identity { get; set; }        

        public bool IsInRole(string role)
        {
            var roles = role.Split(new char[] { ',' });
            return roles.Any(r => Account.Roles.Contains(r));
        }
    }
}
```



&nbsp;
## 06 Session Persister

* In the *Security* folder, add the *SessionPersister* **static** class. On successful login, the authenticated username from the form is being assigned to the session persister username, hence username persistence is achieved in the current HttpContext for this session.

*Controllers/AccountController.cs*
```
using CustomAuthMVC.Security;
```
```
    [HttpPost]
    public ActionResult Login(AccountViewModel avm)
    {
        // ...
        SessionPersister.Username = avm.Account.UserName;
        return View("Success");
    }
```


*Security/SessionPersister.cs*
```
using System.Web;

namespace CustomAuthMVC.Security
{
    public static class SessionPersister
    {
        static string usernameSessionVar = "username";

        public static string Username
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;
                var sessionVar = HttpContext.Current.Session[usernameSessionVar];
                if (sessionVar != null)
                    return sessionVar as string;
                return null;
            }
            set
            {
                HttpContext.Current.Session[usernameSessionVar] = value;
            }
        }
    }
}
```
