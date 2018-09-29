using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DBLayer.BisLayer;
using DBLayer.Repositories;
using HW_Chat_V3;
using LogData;

namespace Server
{
    class ChatEngine
    {
        List<User> connectedUsers = new List<User>();
        Dictionary<string, List<ChatMessage>> incomingMessages = new Dictionary<string, List<ChatMessage>>();
        IEntityService<BisUser> BisUserService;
        IEntityService<BisAccount> BisAccountService;
        LogAction logAction;

        public List<User> ConnectedUsers
        {
            get { return connectedUsers; }
        }

        public string AddNewChatUser(User user)
        {
            if (!connectedUsers.Contains(user))
            {
                connectedUsers.Add(user);
                incomingMessages.Add(user.UserName,
                    new List<ChatMessage>()
                    {
                        new ChatMessage() {User = user, Message = " Welcome to the Chat",  Date = DateTime.Now}
                    });
                string newUser = String.Format("User {0} connected at {1}\n", user, DateTime.Now.ToString("HH:mm"));
                Console.Write(newUser);
                return newUser;
            }
            else
                return null;
        }

        public string AddNewMessage(ChatMessage newMessage)
        {
            logAction = new LogAction();
            BisUserService = new BisUserService();
            var sender = BisUserService.FindBy(x => x.Name.ToLower() == newMessage.User.UserName.ToLower())
                .FirstOrDefault()
                .USER_id;
            string message = String.Format("{0} says to all: {1} at {2}\n", newMessage.User.UserName,
                newMessage.Message, newMessage.Date.ToString("HH:mm"));
            try
            {
                foreach (var user in connectedUsers)
                {
                    if (newMessage.User.UserName != user.UserName)
                    {
                        incomingMessages[user.UserName].Add(newMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }

            Console.Write(message);
            logAction.LogMessaging(sender, newMessage.Message, null, "all");
            logAction.Dispose();
            return message;
        }

        public string AddNewMessage(ChatMessage newMessage, string reciever)
        {
            logAction = new LogAction();
            BisUserService = new BisUserService();
            var sender = BisUserService.FindBy(x => x.Name.ToLower() == newMessage.User.UserName.ToLower())
                .FirstOrDefault()
                .USER_id;
            var reciever_id = BisUserService.FindBy(x => x.Name.ToLower() == reciever.ToLower())
                .FirstOrDefault()
                .USER_id.ToString();
            string message = String.Format("{0} says to you: {1} at {2}\n", newMessage.User.UserName,
                newMessage.Message, newMessage.Date.ToString("HH:mm"));
            try
            {
                incomingMessages[newMessage.User.UserName].Add(newMessage);
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }

            Console.Write("{0} says to {1}: {2} at {3}\n", newMessage.User.UserName, reciever, newMessage.Message,
                newMessage.Date.ToString("HH:mm"));
            logAction.LogMessaging(sender, newMessage.Message, null, reciever_id);
            logAction.Dispose();
            return message;
        }

        public string AddNewMessage(FileMessage newMessage, string reciever)
        {
            logAction = new LogAction();
            BisUserService = new BisUserService();
            string message = String.Format("{0} sent you file: {1} at {2}\n", newMessage.Sender, newMessage.FileName,
                newMessage.Time.ToString("HH:mm"));
            Console.Write(message);
            var sender = BisUserService.FindBy(x => x.Name.ToLower() == newMessage.Sender.ToLower())
                .FirstOrDefault()
                .USER_id;
            var reciever_id = BisUserService.FindBy(x => x.Name.ToLower() == reciever.ToLower())
                .FirstOrDefault()
                .USER_id.ToString();
            logAction.LogMessaging(sender, null, newMessage.FileName, reciever_id);
            logAction.Dispose();
            return message;
        }

        public string Remove(User user)
        {
            string message = String.Format("User {0} has left the Chat at {1}\n", user.UserName,
                DateTime.Now.ToString("HH:mm"));
            Console.Write(message);
            try
            {
                logAction = new LogAction();
                BisUserService = new BisUserService();
                BisAccountService = new BisAccountService();
                var bisUser = BisUserService.FindBy(x => x.Name.ToLower() == user.UserName.ToLower())
                    .FirstOrDefault();
                var bisAccount = BisAccountService.FindBy(x => x.User_Id == bisUser.USER_id).FirstOrDefault();
                bisAccount.registration = 1;
                connectedUsers.Remove(user);
                BisAccountService.AddOrUpdate(bisAccount);
                logAction.LogConnect(Convert.ToInt32(bisAccount.User_Id), 2);
                logAction.Dispose();
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }

            return message;
        }
    }
}
