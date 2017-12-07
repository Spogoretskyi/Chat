using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace WPFClient
{
    class Validator : IDataErrorInfo
    {
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Registration_Code { get; set; }
        string pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$";

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Phone":
                        for (int i = 0; i < Phone.Length; i++)
                        {
                            if (!Char.IsDigit(Phone[i]))
                                error = "Please enter the digits";
                        }
                        break;

                    case "Name":
                        if (Name.Length < 2)
                        {
                            error = "Min length 2 symbols";
                        }
                        break;
                    case "Mail":
                        if (!Regex.IsMatch(Mail, pattern))
                        {
                            error = "Please enter correct email address";
                        }
                        break;
                    case "Registration_Code":
                        {
                            for (int i = 0; i < Registration_Code.Length; i++)
                            {
                                if (!Char.IsDigit(Registration_Code[i]))
                                    error = "Please enter 4 digits";
                            }
                        }
                        break;
                    default:
                        {
                            error = String.Empty;
                        }
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsDigit(string number)
        {
            for (int i = 0; i < number.Length; i++)
            {
                if (!Char.IsDigit(number[i]))
                    return false;
            }
            return true;
        }
        public bool IsPassword(string psw, string repeatPsw)
        {
            if (psw.Length < 4)
                return false;
            if (psw != repeatPsw)
                return false;
            return true;
        }
        public bool IsName(string name)
        {
            if (name.Length < 2)
                return false;
            return true;
        }
        public bool IsMail(string mail)
        {
            if (!Regex.IsMatch(Mail, pattern))
                return false;
            return true;
        }
    }
}
