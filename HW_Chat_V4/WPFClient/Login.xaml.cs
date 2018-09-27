using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using WPFClient.Proxy; 

namespace WPFClient
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        Code code;
        Registrarion registrarion;
        MainWindow mainwindow;
        ProxyAuthorization.AuthorizationContractClient server;
        
        public Login()
        {
            InitializeComponent();
            server = new ProxyAuthorization.AuthorizationContractClient();        
        }
        private void Button_connect_Click(object sender, RoutedEventArgs e)
        {
            var result = server.Join(login.Text, password.Password);
            if (result == "true")
            {
                mainwindow = new MainWindow(this);
                mainwindow.Show();
                Close();
            }
            else if (result == "code")
            {
                ErrorMessage.Content = "Please enter registration code";
                code = new Code(login.Text);
                code.Show();
                ErrorMessage.Content = "";
            }
            else
                ErrorMessage.Content = "Name or password is incorrect";
        }

        private void Sign_in_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            registrarion = new Registrarion();
            registrarion.Show();
            Close();
        }

    }
}
