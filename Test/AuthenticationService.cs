using System;
using System.Linq;
using DBLayer.Repositories;
using DBLayer.BizLayer;

namespace AuthenticationService
{
    class AuthenticationService : IAuthenticationContract
    {

        public string ConnectTo(string userName, string userPassword)
        {
            IEntityService<BisUsers> BisUsersService = new BisUsersService();
            var user = BisUsersService.GetAll().Where(x => x.name == userName).FirstOrDefault();
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
            IEntityService<BisUsers> BisUsersService = new BisUsersService();
            var tmpName = BisUsersService.GetAll().Where(n => n.name == name).FirstOrDefault();
            var tmpMail = BisUsersService.GetAll().Where(n => n.email == email).FirstOrDefault();
            var tmpPhone = BisUsersService.GetAll().Where(n => n.phone == phone).FirstOrDefault();

                if (tmpName == null && tmpMail == null && tmpPhone == null)
                {
                try
                {
                    var code = RegistrationNumber();
                    BisUsers user = new BisUsers();
                    user.name = name;
                    user.password = Hash.HashPassword(password);
                    user.email = email;
                    user.phone = phone;
                    user.registration = code;
                    BisUsersService.AddOrUpdate(user);
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
            IEntityService<BisUsers> BisUsersService = new BisUsersService();
            var user = BisUsersService.GetAll().Where(x => x.name == userName).FirstOrDefault();
            if (user.registration == code)
                {
                try
                {
                    user.registration = 1;
                    BisUsersService.AddOrUpdate(user);
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
