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
