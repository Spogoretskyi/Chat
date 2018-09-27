using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading;
using HW_Chat_V3;

namespace Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    class ChatService : IServiceContract
    {
        Dictionary<string, IClientContract> _callbackList = new Dictionary<string, IClientContract>();
        ChatEngine engine = new ChatEngine();

        public void Connect(User user)
        {
            try
            {
                Console.WriteLine("Connect");
                IClientContract callback = OperationContext.Current.GetCallbackChannel<IClientContract>();
                if (!_callbackList.ContainsKey(user.UserName))
                {
                    var message = engine.AddNewChatUser(user);
                    _callbackList.Add(user.UserName, callback);
                    ThreadPool.QueueUserWorkItem(p => { CallbackSendMessage(message, callback); });
                }
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
               // var callbackReceiver = _callbackList[receiver];
                var callbackReceiver = _callbackList.FirstOrDefault(x => x.Key == receiver);
                var message = engine.AddNewMessage(newMessage, receiver);
                ThreadPool.QueueUserWorkItem(p => { CallbackSendPM(message, callbackReceiver.Value); });
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
                    Thread.Sleep(1000);
                    ThreadPool.QueueUserWorkItem(p => { CallbackRemoveUser(message); });
                }
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        public void GetUser(User user)
        {
            try
            {
                    var curUser = engine.ConnectedUsers.Where(x => x.UserName == user.UserName).FirstOrDefault()
                        .UserName;
                    var callback = _callbackList[curUser];
                    ThreadPool.QueueUserWorkItem(p => { CallbackUser(callback); });
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
                    if (item != callback) item.RecieveMessage(message);
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
                if (_callbackList.ContainsValue(callbackReceiver))
                    callbackReceiver.RecievePmMessage(message);
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

        void CallbackUser(IClientContract callback)
        {
            try
            {
                callback.All_Users(engine.ConnectedUsers);
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
                foreach (var item in _callbackList)
                {
                    item.Value.All_Users(engine.ConnectedUsers);
                }
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }
    }
}
