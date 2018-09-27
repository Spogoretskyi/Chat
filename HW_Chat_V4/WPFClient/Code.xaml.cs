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

namespace WPFClient
{
    /// <summary>
    /// Логика взаимодействия для Code.xaml
    /// </summary>
    public partial class Code : Window
    {
        Validator validator;
        public string ErrMode = string.Empty;
        string userName;
        ProxyAuthorization.AuthorizationContractClient server;
        public Code(string name)
        {
            InitializeComponent();
            validator = new Validator();
            this.DataContext = validator;
            userName = name;
            server = new ProxyAuthorization.AuthorizationContractClient();
        }

        private void Label_Error(object sender, ValidationErrorEventArgs e)
        {
            ErrMode = e.Error.ErrorContent.ToString();
            ErrorMessage.Content = ErrMode;
            if (e.Action.ToString() == "Removed")
            {
                ErrMode = String.Empty;
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
