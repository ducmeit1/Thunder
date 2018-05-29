using System;
using System.Text.RegularExpressions;

namespace Thunder.Helpers
{
    public static class Ultilities
    {
        public static bool StringIsEmptyOrNull(string str)
        {
            str = str.Trim();
            return String.IsNullOrEmpty(str);
        }

        public static string NormalizeText(string str)
        {
            Regex regex = new Regex(@"\W+");
            str = regex.Replace(str, " ").Trim();
            string[] stringsArray = str.Split(' ');
            string newString = "";
            foreach (var s in stringsArray)
            {
                newString += s[0].ToString().ToUpper() + s.Substring(1, s.Length) + " ";
            }
            return newString.Trim();
        }

        public static bool StringIsPhoneNumber(string str)
        {
            Regex regex = new Regex(@"^[\d+-]{7,}$");
            return regex.IsMatch(str);
        }

        public static bool StringIsEmail(string str)
        {
            Regex regex = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            return regex.IsMatch(str);
        }

        public static string RandomNumber(int length)
        {
            Random rd = new Random();
            string numberRandomded = "";
            for (int i = 0; i < length; i++)
            {
                int rdNum = rd.Next(0, 10);
                numberRandomded += $"{rdNum}";
            }
            return numberRandomded;
        }
    }
}
