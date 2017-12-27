using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomAuthMVC.Models
{
    public class AccountModel
    {
        private List<Account> listAccounts = new List<Account>();

        private CustomAuthMVCEntities db = new CustomAuthMVCEntities();

        public AccountModel()
        {

            foreach (var user in db.Users)
            {
                listAccounts.Add(new Account
                {
                    UserName = user.UserName,
                    Password = user.Password,
                    Roles = user.Roles.Split(new char[] { ',' })
            });
            }
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
