// Type: Phoenix.HabboHotel.Misc.AntiMutant
// Assembly: Phoenix 3.0, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\Users\NETO\Desktop\Phoenix 3.0.exe

using Phoenix.Core;
using Phoenix.HabboHotel.Rooms;
using System;
using System.Text.RegularExpressions;

namespace Phoenix.HabboHotel.Misc
{
    internal class AntiMutant
    {
        public static void ApplyClothing(RoomUser User, string ToAdd)
        {
            string str;
            string str2;
            if (ToAdd.Contains("hr-"))
            {
                str2 = Regex.Split(ToAdd, "hr-")[1];
                str2 = "hr-" + str2.Split(new char[] { '.' })[0];
                if (User.GetClient().GetHabbo().Look.IndexOf("hr-") == -1)
                {
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look + str2;
                }
                else
                {
                    str = Regex.Split(User.GetClient().GetHabbo().Look, "hr-")[1];
                    str = "hr-" + str.Split(new char[] { '.' })[0];
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look.Replace(str, str2);
                }
            }
            if (ToAdd.Contains("hd-"))
            {
                str2 = Regex.Split(ToAdd, "hd-")[1];
                str2 = "hd-" + str2.Split(new char[] { '.' })[0];
                if (User.GetClient().GetHabbo().Look.IndexOf("hd-") == -1)
                {
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look + str2;
                }
                else
                {
                    str = Regex.Split(User.GetClient().GetHabbo().Look, "hd-")[1];
                    str = "hd-" + str.Split(new char[] { '.' })[0];
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look.Replace(str, str2);
                }
            }
            if (ToAdd.Contains("ch-"))
            {
                str2 = Regex.Split(ToAdd, "ch-")[1];
                str2 = "ch-" + str2.Split(new char[] { '.' })[0];
                if (User.GetClient().GetHabbo().Look.IndexOf("ch-") == -1)
                {
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look + str2;
                }
                else
                {
                    str = Regex.Split(User.GetClient().GetHabbo().Look, "ch-")[1];
                    str = "ch-" + str.Split(new char[] { '.' })[0];
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look.Replace(str, str2);
                }
            }
            if (ToAdd.Contains("lg-"))
            {
                str2 = Regex.Split(ToAdd, "lg-")[1];
                str2 = "lg-" + str2.Split(new char[] { '.' })[0];
                if (User.GetClient().GetHabbo().Look.IndexOf("lg-") == -1)
                {
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look + str2;
                }
                else
                {
                    str = Regex.Split(User.GetClient().GetHabbo().Look, "lg-")[1];
                    str = "lg-" + str.Split(new char[] { '.' })[0];
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look.Replace(str, str2);
                }
            }
            if (ToAdd.Contains("sh-"))
            {
                str2 = Regex.Split(ToAdd, "sh-")[1];
                str2 = "sh-" + str2.Split(new char[] { '.' })[0];
                if (User.GetClient().GetHabbo().Look.IndexOf("sh-") == -1)
                {
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look + str2;
                }
                else
                {
                    str = Regex.Split(User.GetClient().GetHabbo().Look, "sh-")[1];
                    str = "sh-" + str.Split(new char[] { '.' })[0];
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look.Replace(str, str2);
                }
            }
            if (ToAdd.Contains("ea-"))
            {
                str2 = Regex.Split(ToAdd, "ea-")[1];
                str2 = "ea-" + str2.Split(new char[] { '.' })[0];
                if (User.GetClient().GetHabbo().Look.IndexOf("ea-") == -1)
                {
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look + str2;
                }
                else
                {
                    str = Regex.Split(User.GetClient().GetHabbo().Look, "ea-")[1];
                    str = "ea-" + str.Split(new char[] { '.' })[0];
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look.Replace(str, str2);
                }
            }
            if (ToAdd.Contains("ca-"))
            {
                str2 = Regex.Split(ToAdd, "ca-")[1];
                str2 = "ca-" + str2.Split(new char[] { '.' })[0];
                if (User.GetClient().GetHabbo().Look.IndexOf("ca-") == -1)
                {
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look + str2;
                }
                else
                {
                    str = Regex.Split(User.GetClient().GetHabbo().Look, "ca-")[1];
                    str = "ca-" + str.Split(new char[] { '.' })[0];
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look.Replace(str, str2);
                }
            }
            if (ToAdd.Contains("ha-"))
            {
                str2 = Regex.Split(ToAdd, "ha-")[1];
                str2 = "ha-" + str2.Split(new char[] { '.' })[0];
                if (User.GetClient().GetHabbo().Look.IndexOf("ha-") == -1)
                {
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look + str2;
                }
                else
                {
                    str = Regex.Split(User.GetClient().GetHabbo().Look, "ha-")[1];
                    str = "ha-" + str.Split(new char[] { '.' })[0];
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look.Replace(str, str2);
                }
            }
            if (ToAdd.Contains("he-"))
            {
                str2 = Regex.Split(ToAdd, "he-")[1];
                str2 = "he-" + str2.Split(new char[] { '.' })[0];
                if (User.GetClient().GetHabbo().Look.IndexOf("he-") == -1)
                {
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look + str2;
                }
                else
                {
                    str = Regex.Split(User.GetClient().GetHabbo().Look, "he-")[1];
                    str = "he-" + str.Split(new char[] { '.' })[0];
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look.Replace(str, str2);
                }
            }
            if (ToAdd.Contains("wa-"))
            {
                str2 = Regex.Split(ToAdd, "wa-")[1];
                str2 = "wa-" + str2.Split(new char[] { '.' })[0];
                if (User.GetClient().GetHabbo().Look.IndexOf("wa-") == -1)
                {
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look + str2;
                }
                else
                {
                    str = Regex.Split(User.GetClient().GetHabbo().Look, "wa-")[1];
                    str = "wa-" + str.Split(new char[] { '.' })[0];
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look.Replace(str, str2);
                }
            }
            if (ToAdd.Contains("fa-"))
            {
                str2 = Regex.Split(ToAdd, "fa-")[1];
                str2 = "fa-" + str2.Split(new char[] { '.' })[0];
                if (User.GetClient().GetHabbo().Look.IndexOf("fa-") == -1)
                {
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look + str2;
                }
                else
                {
                    str = Regex.Split(User.GetClient().GetHabbo().Look, "fa-")[1];
                    str = "fa-" + str.Split(new char[] { '.' })[0];
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look.Replace(str, str2);
                }
            }
            if (ToAdd.Contains("cc-"))
            {
                str2 = Regex.Split(ToAdd, "cc-")[1];
                str2 = "cc-" + str2.Split(new char[] { '.' })[0];
                if (User.GetClient().GetHabbo().Look.IndexOf("cc-") == -1)
                {
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look + str2;
                }
                else
                {
                    str = Regex.Split(User.GetClient().GetHabbo().Look, "cc-")[1];
                    str = "cc-" + str.Split(new char[] { '.' })[0];
                    User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().Look.Replace(str, str2);
                }
            }
        }

        public static bool ValidateLook(string Figure, string Gender)
        {
            bool flag = false;
            if (Figure.Length >= 1)
            {
                try
                {
                    string[] strArray = Figure.Split(new char[] { '.' });
                    if (strArray.Length < 4)
                    {
                        return false;
                    }
                    foreach (string str in strArray)
                    {
                        string[] strArray2 = str.Split(new char[] { '-' });
                        if (strArray2.Length < 3)
                        {
                            return false;
                        }
                        string str2 = strArray2[0];
                        int num = int.Parse(strArray2[1]);
                        int num2 = int.Parse(strArray2[1]);
                        if ((num <= 0) || (num2 < 0))
                        {
                            return false;
                        }
                        if (str2.Length != 2)
                        {
                            return false;
                        }
                        if (str2 == "hd")
                        {
                            flag = true;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                if (flag && (!(Gender != "M") || !(Gender != "F")))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
