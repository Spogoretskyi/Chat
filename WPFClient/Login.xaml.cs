using System;
using System.ServiceModel;
using System.ServiceModel.Description;
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
        private ServiceHost host;

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
                switch (result)
                {
                    case "true":
                        mainwindow = new MainWindow(this);
                        mainwindow.Show();
                        Close();
                        break;
                    case "code":
                        MessageBox.Show("Please enter registration code in registration form", "Chat",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        code = new Code(login.Text);
                        code.Show();
                        ErrorMessage.Content = "";
                        break;
                    case "logged":
                        ErrorMessage.Content = "User has already logged in this system";
                        break;
                    case "notRegistered":
                        ErrorMessage.Content = "The user is not registered";
                        break;
                    case "false":
                        ErrorMessage.Content = "Name or password is incorrect";
                        break;
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
