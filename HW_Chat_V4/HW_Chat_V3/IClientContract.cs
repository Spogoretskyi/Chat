using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using DBLayer;

namespace Server
{
    interface IClientContract
    {
        [OperationContract(IsOneWay = true)]
        void RecieveMessage(string message);
        [OperationContract(IsOneWay = true)]
        void All_Users(List<User> allUsers);
        [OperationContract(IsOneWay = true)]
        void UserRemove(User user);
    }
    [ServiceContract(CallbackContract = typeof(IClientContract))]
    interface IServiceContract
    {
        [OperationContract(IsOneWay = true)]
        void Connect(string userName);
        [OperationContract(IsOneWay = true)]
        void SendMessage(ChatMessage message);
        [OperationContract(IsOneWay = true)]
        void RemoveUser(User user);
        [OperationContract(IsOneWay = true)]
        void GetAllUsers();
    }
}