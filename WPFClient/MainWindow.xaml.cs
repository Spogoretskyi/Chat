using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.ServiceModel;
using System.Windows.Threading;
using WPFClient.Proxy;
using System.IO;
using Microsoft.Win32;

namespace WPFClient
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext = false)]
    public partial class MainWindow : Window, Proxy.IServiceContractCallback
    {
        static string userName = String.Empty;
        Proxy.User user = new Proxy.User();
        InstanceContext context;
        Proxy.ServiceContractClient server;
        static string _files_path = 
            String.Format(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + 
                "\\Files\\");

        public MainWindow(Login window)
        {
            try
            {
                InitializeComponent();

                DirectoryInfo dir = new DirectoryInfo(_files_path);
                if (!dir.Exists)
                {
                    dir.Create();
                }
                userName = window.login.Text;
                user.UserName = userName;
                context = new InstanceContext(this);
                server = new Proxy.ServiceContractClient(context);
                server.Connect(userName);
                ListBox_Users.Items.Add(userName);
                server.GetAllUsers();
                string connect = String.Format("You connected at {0}\n", DateTime.Now.ToString("HH:mm"));
                Add_Colors(connect, Brushes.CadetBlue);
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        private void Add_Colors(string str, Brush color)
        {
            TextRange tr = new TextRange(TextBox_Chat.Document.ContentEnd, 
                TextBox_Chat.Document.ContentEnd);
            tr.Text = str;
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            TextBox_Chat.ScrollToEnd();
        }

        public void All_Users(User[] allUsers)
        {
            try
            {
                ListBox_Users.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                { ListBox_Users.Items.Clear(); }));
                foreach (var user in allUsers)
                {
                    ListBox_Users.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        ListBox_Users.Items.Add(user.UserName);
                    }));
                }
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        public void UserRemove(User user)
        {
            try
            {
                ListBox_Users.Items.Remove(user);
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        public void RecieveMessage(string message)
        {
            try
            {
                TextBox_Chat.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    Add_Colors(message, Brushes.DarkSlateGray);
                }));
                server.GetAllUsers();
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        private void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Proxy.ChatMessage message = new Proxy.ChatMessage();
                context = new InstanceContext(this);
                server = new Proxy.ServiceContractClient(context);
                if (TextBox_Input.Text != String.Empty)
                {
                    message.User = user;
                    message.Message = TextBox_Input.Text;
                    message.Date = DateTime.Now;
                    string ownMeassage = String.Format("You says to all: {0} at {1}\n", 
                        message.Message, DateTime.Now.ToString("HH:mm"));
                    server.SendMessage(message);
                    Add_Colors(ownMeassage, Brushes.DarkCyan);
                }
                TextBox_Input.Text = String.Empty;
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }
        private void Button_Send_PM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Proxy.ChatMessage message = new Proxy.ChatMessage();
                context = new InstanceContext(this);
                server = new Proxy.ServiceContractClient(context);
                if (TextBox_Input.Text != String.Empty)
                {
                    message.User = user;
                    message.Message = TextBox_Input.Text;
                    message.Date = DateTime.Now;
                    if (ListBox_Users.SelectedItem != null && 
                        ListBox_Users.SelectedItem.ToString() != userName)
                    {
                        string ownMeassage = String.Format("You says to {0}: {1} at {2}\n", 
                            ListBox_Users.SelectedItem.ToString(), message.Message, 
                            DateTime.Now.ToString("HH:mm"));
                        server.SendPMmessage(message, ListBox_Users.SelectedItem.ToString());
                        Add_Colors(ownMeassage, Brushes.DarkCyan);
                    }
                }
                TextBox_Input.Text = String.Empty;
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                context = new InstanceContext(this);
                server = new Proxy.ServiceContractClient(context);
                server.RemoveUser(user);
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
            Environment.Exit(0);
        }

        private void Button_Disconnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                context = new InstanceContext(this);
                server = new Proxy.ServiceContractClient(context);
                server.RemoveUser(user);
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
           Environment.Exit(0);
        }

        public void ReceiverFile(FileMessage fileMsg)
        {
            try
            {
                FileStream stream = new FileStream(_files_path +
                               fileMsg.FileName, FileMode.Create,
                               FileAccess.ReadWrite);
                stream.Write(fileMsg.Data, 0, fileMsg.Data.Length);
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        private void Button_Send_File_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox_Users.SelectedItem != null && 
                ListBox_Users.SelectedItem.ToString() != userName)
            {
                Stream stream = null;
                try
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Multiselect = false;
                    fileDialog.CheckFileExists = true;
                    fileDialog.RestoreDirectory = true;

                    if (fileDialog.ShowDialog() == true)
                    {
                        stream = fileDialog.OpenFile();
                        if (stream != null)
                        {
                            byte[] buffer = new byte[(int)stream.Length];
                            int bytes = stream.Read(buffer, 0, buffer.Length);
                            if (bytes > 0)
                            {
                                Proxy.FileMessage message = new FileMessage();
                                message.FileName = fileDialog.SafeFileName;
                                message.Sender = user.UserName;
                                message.Time = DateTime.Now;
                                message.Data = buffer;
                                server.SendFile(message, ListBox_Users.SelectedItem.ToString());
                            }
                            string ownMeassage = String.Format("You sent to {0} file at {1}\n", 
                                ListBox_Users.SelectedItem.ToString(), DateTime.Now.ToString("HH:mm"));
                            Add_Colors(ownMeassage, Brushes.DarkCyan);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFile.GetExceptions(ex);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
        }

        private void Button_File_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(_files_path);
        }
    }
}
