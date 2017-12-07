using System;
using System.Runtime.Serialization;

namespace HW_Chat_V3
{
    [DataContract]
    public class FileMessage
    {
        string sender;
        string fileName;
        byte[] data;
        DateTime time;

        [DataMember]
        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        [DataMember]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        [DataMember]
        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }

        [DataMember]
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}
