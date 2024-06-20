using InvoiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagementSystem.IRepository
{
    public interface IAccountRepository
    {

        AccountModel Login(AccountModel cls);
        AccountModel ForgotPassword(AccountModel cls);
        AccountModel MyProfile(AccountModel cls);
        AccountModel UpdateProfile(AccountModel cls);
        AccountModel ChangePassword(AccountModel cls);
        void sendEmail(string toEmail, string subject, string body, string imagePath);
    }
}
