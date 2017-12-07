using System;
using System.Windows;
using System.Windows.Input;

namespace WPFClient
{
    public partial class Login : Window
    {
        Code code;
        Registrarion registrarion;
        MainWindow mainwindow;
        ProxyAuthentication.AuthenticationContractClient server;

        public Login() 
        {
            try
            {
                InitializeComponent();
                server = new ProxyAuthentication.AuthenticationContractClient();
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        private void Button_connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = server.ConnectTo(login.Text, password.Password);
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
                {
                    ErrorMessage.Content = "Name or password is incorrect";
                }
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        private void Sign_in_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            registrarion = new Registrarion();
            registrarion.Show();
            Close();
        }
    }
}
