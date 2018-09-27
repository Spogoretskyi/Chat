using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.ServiceModel;
using System.Windows.Threading;
using WPFClient.Proxy;
using System.IO;
using System.Security.Authentication.ExtendedProtection;
using System.Threading;
using Microsoft.Win32;

namespace WPFClient
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext = false)]
    public partial class MainWindow : Window, Proxy.IServiceContractCallback
    {
        private static string userName = String.Empty;
        private static string SendTo = String.Empty;
        private Proxy.User user = new Proxy.User();
        private InstanceContext context;
        private Proxy.ServiceContractClient server;
        private bool isFileChecked = false;
        private static string _files_path =
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
                server.Connect(user);
                Thread.Sleep(1000);
                server.GetUser(user);

                string connect = String.Format("You connected at {0}\n", DateTime.Now.ToString("HH:mm"));
                SendTo = "";
                Add_Colors(connect, Brushes.CadetBlue);
                
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        private void Add_Colors(string str, Brush color)
        {
            TextRange tr = new TextRange(TextBox_Chat.Document.ContentEnd, TextBox_Chat.Document.ContentEnd);
            tr.Text = str;
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            TextBox_Chat.ScrollToEnd();
        }

        public void All_Users(User[] allUsers)
        {
            try
            {
                ListBox_Users.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                {
                    ListBox_Users.Items.Clear();

                    if (allUsers.Length > 2)
                        ListBox_Users.Items.Add("All");

                    ListBox_Users.Items.Add(userName + " (you)");

                    foreach (var user in allUsers)
                    {
                        if (user.UserName != userName) ListBox_Users.Items.Add(user.UserName);
                    }
                }));
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
                TextBox_Chat.Dispatcher.Invoke(DispatcherPriority.Background,
                    new Action(() => { Add_Colors(message, Brushes.DarkSlateGray); }));
                server.GetUser(user);
                //server.GetAllUsers();
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        public void RecievePmMessage(string message)
        {
            try
            {
                TextBox_Chat.Dispatcher.Invoke(DispatcherPriority.Background,
                    new Action(() => { Add_Colors(message, Brushes.DarkSlateGray); }));
               server.GetUser(user);
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
                        if (TextBox_Input.Text != String.Empty)
                        {
                            //send to all
                            if (SendTo == String.Empty || SendTo == "All")
                            {
                                message.User = user;
                                message.Message = TextBox_Input.Text;
                                message.Date = DateTime.Now;
                                string ownMeassage = String.Format("You says to all: {0} at {1}\n", message.Message,
                                    DateTime.Now.ToString("HH:mm"));
                                server.SendMessage(message);
                                Add_Colors(ownMeassage, Brushes.DarkCyan);
                            }
                            //send PM
                            else if (SendTo != String.Empty || SendTo != "All")
                            {
                                message.User = user;
                                message.Message = TextBox_Input.Text;
                                message.Date = DateTime.Now;
                                string ownMeassage = String.Format("You says to {0}: {1} at {2}\n",
                                    SendTo, message.Message,
                                    DateTime.Now.ToString("HH:mm"));
                                server.SendPMmessage(message, SendTo);
                                Add_Colors(ownMeassage, Brushes.DarkCyan);
                                
                             }
                        }
                        TextBox_Input.Text = String.Empty;
                        SendTo = "";
                        ListBox_Users.UnselectAll();
                    }
                    catch (Exception ex)
                    {
                        LogFile.GetExceptions(ex);
                    }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
                server.RemoveUser(user);
                Environment.Exit(0);
        }

        private void Button_Disconnect_Click(object sender, RoutedEventArgs e)
        {
                server.RemoveUser(user);
                Environment.Exit(0);
        }

        public void ReceiverFile(FileMessage fileMsg)
        {
            try
            {
                FileStream stream =
                    new FileStream(_files_path + fileMsg.FileName, FileMode.Create, FileAccess.ReadWrite);
                stream.Write(fileMsg.Data, 0, fileMsg.Data.Length);
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        private void Button_Send_File_Click(object sender, RoutedEventArgs e)
        {
            if (SendTo != String.Empty || SendTo != (userName + " (you)"))
            {
                Stream stream = null;
                try
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Multiselect = false;
                    fileDialog.CheckFileExists = true;
                    fileDialog.CheckPathExists = true;
                    fileDialog.RestoreDirectory = true;

                    if (fileDialog.ShowDialog() == true)
                    {
                        stream = fileDialog.OpenFile();
                        if (stream != null)
                        {
                            byte[] buffer = new byte[(int) stream.Length];
                            int bytes = stream.Read(buffer, 0, buffer.Length);
                            if (bytes > 0)
                            {
                                Proxy.FileMessage message = new FileMessage();
                                message.FileName = fileDialog.SafeFileName;
                                message.Data = buffer;
                                message.Sender = user.UserName;
                                message.Time = DateTime.Now;
                                message.Data = buffer;
                                server.SendFile(message, SendTo);
                            }

                            string ownMeassage = String.Format("You sent to {0} file at {1}\n",
                                SendTo, DateTime.Now.ToString("HH:mm"));
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
                TextBox_Input.Text = String.Empty;
                SendTo = "";
                ListBox_Users.UnselectAll();
            }
        }

        private void Button_File_Click(object sender, RoutedEventArgs e)
        {
            ListBox_Users.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                if (ListBox_Users.SelectedItem.ToString() != (userName + " (you)"))
                {
                    System.Diagnostics.Process.Start(_files_path);
                }
            }));
        }

        private void ListBox_Users_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
                    if (ListBox_Users.SelectedItem.ToString() != (userName + " (you)"))
                        SendTo = ListBox_Users.SelectedItem.ToString();
        }

    }
}
