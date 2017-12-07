using System;
using System.Collections.Generic;
using System.Linq;
using HW_Chat_V3;

namespace Server
{
    class ChatEngine
    {
        List<User> connectedUsers = new List<User>();
        Dictionary<string, List<ChatMessage>> incomingMessages = new Dictionary<string, List<ChatMessage>>();

        public List<User> ConnectedUsers
        {
            get { return connectedUsers; }
        }

        public string AddNewChatUser(User user)
        {
                var exists =
                    from User e in this.ConnectedUsers
                    where e.UserName == user.UserName
                    select e;

                if (exists.Count() == 0)
                {
                    this.ConnectedUsers.Add(user);
                    incomingMessages.Add(user.UserName, new List<ChatMessage>() {
                    new ChatMessage() { User = user, Message = " Welcome to the Chat", Date = DateTime.Now }
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
            string message = String.Format("{0} says to all: {1} at {2}\n", newMessage.User.UserName, newMessage.Message, newMessage.Date.ToString("HH:mm"));

            try
            {
                foreach (var user in this.ConnectedUsers)
                {
                    if (!newMessage.User.UserName.Equals(user.UserName))
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
            return message;
        }

        public string AddNewMessage(ChatMessage newMessage, string receiver)
        {
            string message = String.Format("{0} says to you: {1} at {2}\n", newMessage.User.UserName, newMessage.Message, newMessage.Date.ToString("HH:mm"));
            try
            {
                foreach (var user in this.ConnectedUsers)
                {
                    if (!newMessage.User.UserName.Equals(user.UserName))
                    {
                        incomingMessages[user.UserName].Add(newMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
            Console.Write("{0} says to {1}: {2} at {3}\n", newMessage.User.UserName, receiver, newMessage.Message, newMessage.Date.ToString("HH:mm"));
            return message;
        }

        public string AddNewMessage(FileMessage newMessage, string receiver)
        {
            string message = String.Format("{0} sent to you file: {1} at {2}\n", newMessage.Sender, newMessage.FileName, newMessage.Time.ToString("HH:mm"));
            Console.Write(message);
            return message;
        }

        public string Remove(User user)
        {
            string message = String.Format("User {0} has left the Chat at {1}\n", user.UserName, DateTime.Now.ToString("HH:mm"));
            Console.Write(message);
            try
            {
                this.ConnectedUsers.RemoveAll(u => u.UserName == user.UserName);
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
            return message;
        }
    }
}
