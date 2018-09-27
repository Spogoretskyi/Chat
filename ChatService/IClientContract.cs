using System.Collections.Generic;
using System.ServiceModel;
using HW_Chat_V3;

namespace Server
{
    interface IClientContract
    {
        [OperationContract(IsOneWay = true)]
        void RecieveMessage(string message);

        [OperationContract(IsOneWay = true)]
        void RecievePmMessage(string message);

        [OperationContract(IsOneWay = true)]
        void All_Users(List<User> allUsers);

        [OperationContract(IsOneWay = true)]
        void UserRemove(User user);

        [OperationContract(IsOneWay = true)]
        void ReceiverFile(FileMessage message);
    }

    [ServiceContract(CallbackContract = typeof(IClientContract), SessionMode = SessionMode.Required)]
    interface IServiceContract
    {
        [OperationContract(IsOneWay = true)]
        void Connect(User user);

        [OperationContract(IsOneWay = true)]
        void SendMessage(ChatMessage message);

        [OperationContract(IsOneWay = true)]
        void SendPMmessage(ChatMessage newMessage, string receiver);

        [OperationContract(IsOneWay = true)]
        void RemoveUser(User user);

        [OperationContract(IsOneWay = true)]
        void GetUser(User user);

        [OperationContract(IsOneWay = true)]
        void GetAllUsers();

        [OperationContract(IsOneWay = false)]
        bool SendFile(FileMessage message, string receiver);
    }
}