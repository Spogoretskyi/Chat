using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace AuthenticationService
{
    [ServiceContract]
    interface IAuthorizationContract
    {
        [OperationContract]
        string Join(string userName, string userPassword);
        [OperationContract]
        bool AddNewUser(string name, string password, string email, string phone);
        [OperationContract]
        bool GetCode(string userName, int code);
    }
}
