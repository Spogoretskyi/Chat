using System.ServiceModel;

namespace AuthenticationService
{
    [ServiceContract]
    interface IAuthenticationContract
    {
        [OperationContract]
        string ConnectTo(string userName, string userPassword);

        [OperationContract]
        bool AddNewUser(string name, string password, string email, string phone);

        [OperationContract]
        bool GetCode(string userName, int code);
    }
}
