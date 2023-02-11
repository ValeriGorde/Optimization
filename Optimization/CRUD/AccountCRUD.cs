using Optimization.DB_EF;
using Optimization.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimization.CRUD
{
    internal class AccountCRUD : ICRUD<Account>
    {
        public List<Account> _Account { get; set; }
        ApplicationContext context;

        public AccountCRUD() 
        {
            context = new ApplicationContext();
            _Account = context.Accounts.ToList();
            
        }
        public void Create(Account item)
        {
            context.Accounts.Add(item);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var account = context.Accounts.FirstOrDefault(x => x.Id == id);
            context.Accounts.Remove(account);
            context.SaveChanges();
        }

        public bool Read(int id)
        {
            var account = _Account.Find(a => a.Id == id);
            return false;
        }

        public void Update(Account item)
        {
            var account = _Account.FirstOrDefault(a => a.Id == item.Id);
            account.Login = item.Login;
            account.Password = item.Password;
            account.Role = item.Role;

            context.Accounts.Update(account);
            context.SaveChanges();
        }

        public void Delete(Account item)
        {
            throw new NotImplementedException();
        }
    }
}
