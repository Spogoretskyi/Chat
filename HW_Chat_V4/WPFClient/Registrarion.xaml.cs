using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WPFClient
{
    /// <summary>
    /// Логика взаимодействия для UserData.xaml
    /// </summary>
    public partial class Registrarion : Window
    {
        Validator validator;
        public string ErrMode = string.Empty;
        ProxyAuthorization.AuthorizationContractClient server;
        Login login;

        public Registrarion()
        {
            InitializeComponent();
            validator = new Validator();
            this.DataContext = validator;
            server = new ProxyAuthorization.AuthorizationContractClient();
        }

        private void Label_Error_Phone(object sender, ValidationErrorEventArgs e)
        {
                ErrMode = e.Error.ErrorContent.ToString();
                Fill.Content = ErrMode;

           if (e.Action.ToString() == "Removed")
            {
                ErrMode = String.Empty;
                Fill.Content = ErrMode;
            }
        }
        private void Label_Error_Name(object sender, ValidationErrorEventArgs e)
        {
            ErrMode = e.Error.ErrorContent.ToString();
            Fill.Content = ErrMode;

            if (e.Action.ToString() == "Removed")
            {
                ErrMode = String.Empty;
                Fill.Content = ErrMode;
            }
        }
        private void Label_Error_Mail(object sender, ValidationErrorEventArgs e)
        {
            ErrMode = e.Error.ErrorContent.ToString();
            Fill.Content = ErrMode;

            if (e.Action.ToString() == "Removed")
            {
                ErrMode = String.Empty;
                Fill.Content = ErrMode;
            }
        }
        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            Text_Box_Name.Text = "";
            Text_Box_Password.Password = "";
            Text_Box_Password_repeat.Password = "";
            Text_Box_mail.Text = "";
            Text_Box_Phone1.Text = "";
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            if (validator.IsPassword(Text_Box_Password.Password, Text_Box_Password_repeat.Password))
            {
                if (validator.IsDigit(Text_Box_Phone1.Text) &&

                    validator.IsName(Text_Box_Name.Text) && validator.IsMail(Text_Box_mail.Text))
                {
                    if (server.AddNewUser(Text_Box_Name.Text, Text_Box_Password.Password, Text_Box_mail.Text, "380" + Text_Box_Phone1.Text))
                    {
                        login = new Login();
                        login.Show();
                        Close();
                    }
                    else
                        Fill.Content = "User " + Text_Box_Name.Text + " has already exist";
                }
            }
            else
                Fill.Content = "Enter correct passwords";
        }
    }
}
