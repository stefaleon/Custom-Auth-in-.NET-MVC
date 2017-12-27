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
