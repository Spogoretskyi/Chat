using System;
using System.Linq;
using DBLayer.Repositories;
using DBLayer.BisLayer;
using LogData;

namespace AuthenticationService
{
    class AuthenticationService : IAuthenticationContract
    {
        IEntityService<BisUser> BisUserService;
        IEntityService<BisUser> newUser;
        IEntityService<BisAccount> BisAccountService;
        LogAction logAction = new LogAction();
        BisUser user;
        BisAccount account;

        public string ConnectTo(string userName, string userPassword)
        {
            BisUserService = new BisUserService();
            logAction = new LogAction();
            var user = BisUserService.FindBy(x => x.Name.ToLower() == userName.ToLower()).FirstOrDefault();
            if (user == null)
            {
                Console.WriteLine("{0} user {1} is not exists", DateTime.Now, userName);
                logAction.LogAuthentication(1, 7);
                logAction.Dispose();
                return "notRegistered";
            }
            else
            {
                BisAccountService = new BisAccountService();
                var userAccount = BisAccountService.FindBy(x => x.User_Id == user.USER_id).FirstOrDefault();
                var iFverified = Hash.VerifyHashedPassword(userAccount.Password, userPassword);
                if (iFverified)
                {
                    switch (userAccount.registration)
                    {
                        case 1:
                            Console.WriteLine("User {0} login at {1}", user.Name, DateTime.Now);
                            userAccount.registration = 2;
                            BisAccountService.AddOrUpdate(userAccount);
                            logAction.LogConnect(user.USER_id, 1);
                            logAction.Dispose();
                            return "true";
                        case 2:
                            Console.WriteLine("{0} user {1} has already logged in this system", DateTime.Now,
                                user.Name);
                            logAction.LogAuthentication(user.USER_id, 8);
                            logAction.Dispose();
                            return "logged";
                        default:
                            Console.WriteLine("{0} user {1} should enter the registration code", DateTime.Now,
                                user.Name);
                            logAction.LogAuthentication(user.USER_id, 4);
                            logAction.Dispose();
                            return "code";
                    }
                }
                else
                {
                    Console.WriteLine("{0} user {1} entered incorrect password", DateTime.Now, user.Name);
                    logAction.LogAuthentication(user.USER_id, 9);
                    logAction.Dispose();
                    return "false";
                }
            }
        }

        public bool AddNewUser(string name, string password, string email, string phone)
        {
            BisUserService = new BisUserService();
            BisAccountService = new BisAccountService();
            logAction = new LogAction();
            var tmpName = BisUserService.GetAll().Where(n => n.Name == name).FirstOrDefault();
            var tmpMail = BisUserService.GetAll().Where(n => n.Email == email).FirstOrDefault();
            var tmpPhone = BisUserService.GetAll().Where(n => n.Phone == phone).FirstOrDefault();
            if (tmpName == null && tmpMail == null && tmpPhone == null)
            {
                try
                {
                    var code = RegistrationNumber();
                    user = new BisUser();
                    account = new BisAccount();
                    user.Name = name;
                    user.Email = email;
                    user.Phone = phone;
                    BisUserService.AddOrUpdate(user);
                    newUser = new BisUserService();
                    account.User_Id = newUser.FindBy(x => x.Name == name).FirstOrDefault().USER_id;
                    account.Password = Hash.HashPassword(password);
                    account.registration = code;
                    BisAccountService.AddOrUpdate(account);
                    Console.WriteLine("New user {0} was registered at {1}", user.Name, DateTime.Now);
                    logAction.LogAuthentication(Convert.ToInt32(account.User_Id), 3);
                    SMTP.SendMail(email, code);
                    Console.WriteLine("Email was sent on {0} at {1}", user.Email, DateTime.Now);
                    Console.WriteLine();
                    logAction.LogAuthentication(Convert.ToInt32(account.User_Id), 5);
                    logAction.Dispose();
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
            BisUserService = new BisUserService();
            BisAccountService = new BisAccountService();
            logAction = new LogAction();
            var user = BisUserService.FindBy(x => x.Name == userName).FirstOrDefault();
            var account = BisAccountService.FindBy(x => x.User_Id == user.USER_id).FirstOrDefault();
            if (account.registration == code)
            {
                try
                {
                    account.registration = 1;
                    BisAccountService.AddOrUpdate(account);
                    logAction.LogAuthentication(Convert.ToInt32(account.User_Id), 6);
                    logAction.Dispose();
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
