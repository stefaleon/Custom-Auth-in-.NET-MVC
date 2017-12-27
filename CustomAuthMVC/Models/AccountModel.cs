using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
