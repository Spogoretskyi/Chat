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
    class ChatEngine
    {
        List<User> conectedUsers = new List<User>();
        Dictionary<string, List<ChatMessage>> incomingMessages = new Dictionary<string, List<ChatMessage>>();

        public List<User> ConnectedUsers
        {
            get { return conectedUsers; }
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
                    new ChatMessage() { User = user, Message = " Welcome to the Chat\n", Date = DateTime.Now }
                });

                    string newUser = String.Format("User {0} connected at {1}\n", user, DateTime.Now.ToString("HH:mm"));
                    Console.WriteLine(newUser);
                    return newUser;
                }
                else
                    return null;
        }
        public string AddNewMessage(ChatMessage newMessage)
        {
            string message = String.Format("{0} says: {1} at {2}\n", newMessage.User.UserName, newMessage.Message, newMessage.Date.ToString("HH:mm"));

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
            Console.WriteLine(message);
            return message;
        }
        public string Remove(User user)
        {
            try
            {
                this.ConnectedUsers.RemoveAll(u => u.UserName == user.UserName);
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
            string message = String.Format("User {0} has left the Chat at {1}\n", user.UserName, DateTime.Now.ToString("HH:mm"));
            Console.WriteLine(message);
            return message;
        }

    }
}
