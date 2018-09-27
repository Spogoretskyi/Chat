using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ServiceModel;
using System.Windows.Threading;
using WPFClient.Proxy;

namespace WPFClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext = false)]
    public partial class MainWindow : Window, Proxy.IServiceContractCallback
    {
        static string userName = String.Empty;
        Proxy.User user = new Proxy.User();
        InstanceContext context;
        Proxy.ServiceContractClient server;
        public MainWindow(Login window)
        {
            InitializeComponent();
            userName = window.login.Text;
            user.UserName = userName;
            context = new InstanceContext(this);
            server = new Proxy.ServiceContractClient(context);
            server.Connect(userName);
            server.GetAllUsers();
        }

        public void All_Users(User[] allUsers)
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

        public void UserRemove(User user)
        {
            ListBox_Users.Items.Remove(user);
        }

        public void RecieveMessage(string message)
        {
            TextBox_Chat.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                TextBox_Chat.Text = TextBox_Chat.Text + message;
            }));
            server.GetAllUsers();
        }

        private void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            Proxy.ChatMessage message = new Proxy.ChatMessage();
            context = new InstanceContext(this);
            server = new Proxy.ServiceContractClient(context);
            if (TextBox_Input.Text != String.Empty)
            {
                message.User = user;
                message.Message = TextBox_Input.Text;
                message.Date = DateTime.Now;
                server.SendMessage(message);
            }
            TextBox_Input.Text = String.Empty;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            context = new InstanceContext(this);
            server = new Proxy.ServiceContractClient(context);
            server.RemoveUser(user);
            server.GetAllUsers();
            Environment.Exit(0);
        }

        private void Button_Disconnect_Click(object sender, RoutedEventArgs e)
        {
            context = new InstanceContext(this);
            server = new Proxy.ServiceContractClient(context);
            server.RemoveUser(user);
            Environment.Exit(0);
        }
    }
}
