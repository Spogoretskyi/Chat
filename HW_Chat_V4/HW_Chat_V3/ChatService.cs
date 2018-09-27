using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading;
using Server;
using System.Collections.ObjectModel;
using System.Windows;
using DBLayer;
using System.Data.Entity.Migrations;

namespace Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    class ChatService : IServiceContract
    {
        List<IClientContract> _callbackList = new List<IClientContract>();
        ChatEngine engine = new ChatEngine();

        public void Connect(string userName)
        {
            try
            {
                IClientContract callback = OperationContext.Current.GetCallbackChannel<IClientContract>();
                var message = engine.AddNewChatUser(new User() { UserName = userName });
                if (!_callbackList.Contains(callback))
                {
                    _callbackList.Add(callback);
                }
                ThreadPool.QueueUserWorkItem(p => { CallbackMethod(message); });
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        public void SendMessage(ChatMessage newMessage)
        {
            try
            {
                IClientContract callback = OperationContext.Current.GetCallbackChannel<IClientContract>();
                var message = engine.AddNewMessage(newMessage);
                ThreadPool.QueueUserWorkItem(p => { CallbackMethod(message); });
                foreach (var item in _callbackList)
                {
                    item.RecieveMessage(message);
                }
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }
        public void RemoveUser(User user)
        {
            try
            {
                IClientContract callback = OperationContext.Current.GetCallbackChannel<IClientContract>();
                if (_callbackList.Contains(callback))
                {
                    _callbackList.Remove(callback);
                }
                if (_callbackList.Count > 0)
                {
                    var message = engine.Remove(user);
                    ThreadPool.QueueUserWorkItem(p => { CallbackMethod(message); });
                }
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }
        public void GetAllUsers()
        {
            try
            {
                IClientContract callback = OperationContext.Current.GetCallbackChannel<IClientContract>();
                ThreadPool.QueueUserWorkItem(p => { CallbackAllUsers(engine.ConnectedUsers); });
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
            }
        void CallbackMethod(string message)
        {
            try
            {
                foreach (var item in _callbackList)
                {
                    item.RecieveMessage(message);
                }
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }
        void CallbackAllUsers(List <User> users)
        {
            try
            {
                foreach (var item in _callbackList)
                {
                    item.All_Users(engine.ConnectedUsers);
                }
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }
    }
}
