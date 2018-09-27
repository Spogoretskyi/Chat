using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBLayer;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Security.Cryptography;

namespace AuthenticationService
{
    class AuthorizationService : IAuthorizationContract
    {
        DBmodel context;

        public string Join(string userName, string userPassword)
        {
                context = new DBmodel();
                var user = context.UsersAll.Where(x => x.name == userName).FirstOrDefault();
            if (user == null)
            {
                Console.WriteLine("{0} user {1} not exist", DateTime.Now, userName);
                return "false";
            }
            else
            {
                    if (Hash.VerifyHashedPassword(user.password, userPassword) && user.registration == 1)
                    {
                        Console.WriteLine("User {0} login at {1}", user.name, DateTime.Now);
                        return "true";
                    }
                    else if (Hash.VerifyHashedPassword(user.password, userPassword) && user.registration != 1)
                    {
                        Console.WriteLine("{0} user {1} should enter the registration code", DateTime.Now, user.name);
                        return "code";
                    }
                    else
                    {
                        Console.WriteLine("{0} user {1} entered incorrect password", DateTime.Now, user.name);
                        return "false";
                    }
                }   
        }
        public bool AddNewUser(string name, string password, string email, string phone)
        {
                context = new DBmodel();
                var tmpName = context.UsersAll.Where(n => n.name == name).FirstOrDefault();
                var tmpMail = context.UsersAll.Where(n => n.email == email).FirstOrDefault();
                var tmpPhone = context.UsersAll.Where(n => n.phone == phone).FirstOrDefault();

                if (tmpName == null && tmpMail == null && tmpPhone == null)
                {
                try
                {
                    var code = RegistrationNumber();
                    Users user = new Users();
                    user.name = name;
                    user.password = Hash.HashPassword(password);
                    user.email = email;
                    user.phone = phone;
                    user.registration = code;
                    context.UsersAll.AddOrUpdate(user);
                    context.SaveChanges();
                    SMTP.SendMail(email, code);
                }
                catch (Exception ex)
                {
                    LogFile.GetExceptions(ex);
                }
                return true;
                }
                else
                    return false;
        }
        private int RegistrationNumber()
        {
            Random random = new Random();
            int number = random.Next(1001, 10000);
            return number;
        }

        public bool GetCode(string userName, int code)
        {
                context = new DBmodel();
                var user = context.UsersAll.Where(x => x.name == userName).FirstOrDefault();
            if (user.registration == code)
                {
                try
                {
                    user.registration = 1;
                    context.UsersAll.AddOrUpdate(user);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogFile.GetExceptions(ex);
                }
                return true;
                }
                else
                    return false;   
}
    }
}
