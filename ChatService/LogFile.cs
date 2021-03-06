﻿using System;
using System.IO;

namespace Server
{
    public static class LogFile
    {
        static string _logpath =
            String.Format(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

        public static void GetExceptions(Exception ex)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(_logpath + @"ChatLogFile.txt", true))
                {
                    outputFile.WriteLine(DateTime.Now.ToString());
                    outputFile.WriteLine(ex);
                    outputFile.WriteLine();
                    outputFile.Close();
                }
            }
            catch
            {
            }
        }
    }
}
