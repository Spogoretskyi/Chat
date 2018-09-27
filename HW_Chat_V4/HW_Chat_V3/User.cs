using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Server
{
    [DataContract]
    class User
    {
        [DataMember]
        public string UserName { get; set; }


        public override string ToString()
        {
            return UserName;
        }
    }
    [DataContract]
    class ChatMessage
    {
        [DataMember]
        public User User { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
    }
}
