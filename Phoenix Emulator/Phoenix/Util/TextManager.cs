using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using Phoenix.Core;
using Phoenix.Storage;
namespace Phoenix.Util
{
	internal class TextManager
	{
        private static Dictionary<string, string> Texts = new Dictionary<string, string>();

        public static void ClearTexts()
        {
            Texts.Clear();
        }

        public static string GetText(string identifier)
        {
            if ((Texts != null) && (Texts.Count > 0))
            {
                return Texts[identifier];
            }
            return identifier;
        }

        public static void LoadTexts(DatabaseClient dbClient)
        {
            Logging.Write("Loading external texts...");
            ClearTexts();
            DataTable table = dbClient.ReadDataTable("SELECT identifier, display_text FROM texts ORDER BY identifier ASC;");
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    Texts.Add(row["identifier"].ToString(), row["display_text"].ToString());
                }
            }
            Logging.WriteLine("completed!");
        }

        public static void WritePhoenix()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("        ______  _                       _          _______             ");
            Console.WriteLine(@"       (_____ \| |                     (_)        (_______)            ");
            Console.WriteLine("        _____) ) | _   ___   ____ ____  _ _   _    _____   ____  _   _ ");
            Console.WriteLine(@"       |  ____/| || \ / _ \ / _  )  _ \| ( \ / )  |  ___) |    \| | | |");
            Console.WriteLine("       | |     | | | | |_| ( (/ /| | | | |) X (   | |_____| | | | |_| |");
            Console.WriteLine(@"       |_|     |_| |_|\___/ \____)_| |_|_(_/ \_)  |_______)_|_|_|\____|");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("                                       " + PhoenixEnvironment.PrettyVersion);
            Console.WriteLine();
            Console.WriteLine("          Dedicated and VPS Hosting available at Otaku-Hosting.com");
            Console.WriteLine("    VPS Hosting from just \x00a36.50 for the first month with coupon OTAKU50!");
        }
	}
}