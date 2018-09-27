using System;
using System.Windows;
using System.Windows.Controls;

namespace WPFClient
{
    public partial class Code : Window
    {
        Validator validator;
        public string ErrMode = string.Empty;
        string userName;
        ProxyAuthentication.AuthenticationContractClient server;

        public Code(string name)
        {
            try
            {
                InitializeComponent();
                validator = new Validator();
                this.DataContext = validator;
                userName = name;
                server = new ProxyAuthentication.AuthenticationContractClient();
            }
            catch (Exception ex)
            {
                LogFile.GetExceptions(ex);
            }
        }

        private void Label_Error(object sender, ValidationErrorEventArgs e)
        {
            ErrMode = e.Error.ErrorContent.ToString();
            ErrorMessage.Content = ErrMode;
            if (e.Action.ToString() == "Removed")
            {
                ErrorMessage.Content = ErrMode;
            }
        }

        private void Button_submit_Click(object sender, RoutedEventArgs e)
        {
            if (validator.IsDigit(Registration_Code.Text))
            {
                if (server.GetCode(userName, Convert.ToInt32(Registration_Code.Text)))
                {
                    Close();
                }
                else
                    ErrorMessage.Content = "Incorrect code";
            }
        }
    }
}
