using DBLayer.BisLayer;
using DBLayer.Repositories;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace LogData
{
    public class LogAction : IDisposable
    {
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                handle.Dispose();
            }

            disposed = true;
        }

        public void LogAuthentication(int user_id, int action)
        {
            BisLogAuthentication bisLogAuthentication = new BisLogAuthentication();
            if (user_id == 0)
                bisLogAuthentication.User_id = null;
            bisLogAuthentication.User_id = user_id;
            bisLogAuthentication.Action = action;
            bisLogAuthentication.Time = DateTime.Now;
            IEntityService<BisLogAuthentication> BisUserLogAuthentication = new LogAuthenticationService();
            BisUserLogAuthentication.AddOrUpdate(bisLogAuthentication);
        }

        public void LogConnect(int user_id, int action)
        {
            BisLogConnect bisLogConnect = new BisLogConnect();
            bisLogConnect.User_id = user_id;
            bisLogConnect.Action = action;
            bisLogConnect.Time = DateTime.Now;
            IEntityService<BisLogConnect> BisUserLogAuthentication = new LogConnectService();
            BisUserLogAuthentication.AddOrUpdate(bisLogConnect);
        }

        public void LogMessaging(int sender, string message, string fileName, string reciever)
        {
            BisLogMessaging bisLogMessaging = new BisLogMessaging();
            bisLogMessaging.Sender = sender;
            bisLogMessaging.message = message;
            bisLogMessaging.file_name = fileName;
            bisLogMessaging.reciever = reciever;
            bisLogMessaging.Time = DateTime.Now;
            IEntityService<BisLogMessaging> BisUserLogMessaging = new LogMessagingService();
            BisUserLogMessaging.AddOrUpdate(bisLogMessaging);
        }
    }
}
