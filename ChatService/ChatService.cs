using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using HW_Chat_V3;

namespace Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    class ChatService : IServiceContract
    {
        Dictionary <string ,IClientContract> _callbackList = new Dictionary<string, IClientContract>();
        ChatEngine engine = new ChatEngine();

        public void Connect(string userName)
        {
            try
            {
                IClientContract callback = OperationContext.Current.GetCallbackChannel<IClientContract>();
                var message = engine.AddNewChatUser(new User() { UserName = userName });
                if (!_callbackList.ContainsKey(userName))
                {
                    _callbackList.Add(userName, callback);
                }
                ThreadPool.QueueUserWorkItem(p => { CallbackSendMessage(message, callback); });
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
                var callbackSender = _callbackList[newMessage.User.UserName];
                var message = engine.AddNewMessage(newMessage);
                ThreadPool.QueueUserWorkItem(p => { CallbackSendMessage(message, callbackSender); });
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        public void SendPMmessage(ChatMessage newMessage, string receiver)
        {
            try
            {
                var message = engine.AddNewMessage(newMessage, receiver);
                var callbackReceiver = _callbackList[receiver];
                ThreadPool.QueueUserWorkItem(p => { CallbackSendPM(message, callbackReceiver); });
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }
        public bool SendFile(FileMessage message, string receiver)
        {
            var file_message = engine.AddNewMessage(message, receiver);
            var callbackReceiver = _callbackList[receiver];
            ThreadPool.QueueUserWorkItem(p => { CallbackSendFile(message, callbackReceiver, file_message); });
            return true;
        }

        public void RemoveUser(User user)
        {
            try
            {
                if (_callbackList.ContainsKey(user.UserName))
                {
                    var message = engine.Remove(user);
                    _callbackList.Remove(user.UserName);
                    ThreadPool.QueueUserWorkItem(p => { CallbackRemoveUser(message); });
                    ThreadPool.QueueUserWorkItem(p => { CallbackAllUsers(); });
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
                ThreadPool.QueueUserWorkItem(p => { CallbackAllUsers(); });
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }
        void CallbackSendFile(FileMessage file_message, IClientContract callback, string message)
        {
            callback.ReceiverFile(file_message);
            callback.RecieveMessage(message);
        }

        void CallbackSendMessage(string message, IClientContract callback)
        {
            try
            {
                foreach (IClientContract item in _callbackList.Values)
                {
                    if(item != callback)
                    item.RecieveMessage(message);
                }
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }
        void CallbackSendPM(string message, IClientContract callbackReceiver)
        {
            try
            {
                callbackReceiver.RecieveMessage(message);
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }
        void CallbackRemoveUser(string message)
        {
            try
            {
                foreach (IClientContract item in _callbackList.Values)
                {
                        item.RecieveMessage(message);
                }
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }
        void CallbackAllUsers()
        {
            try
            {
                foreach (IClientContract item in _callbackList.Values)
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
