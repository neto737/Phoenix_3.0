using System;
using System.Net.Sockets;
using Phoenix.Core;
using Phoenix.Util;
namespace Phoenix.Net
{
    internal class TcpAuthorization
    {
        private static string[] mConnectionStorage;
        private static string mLastIpBlocked;

        internal static bool CheckConnection(Socket Sock)
        {
            string iP = Sock.RemoteEndPoint.ToString().Split(new char[] { ':' })[0];
            if (iP == mLastIpBlocked)
            {
                iP = null;
                return false;
            }
            if (((GetConnectionAmount(iP) > 10) && (iP != "127.0.0.1")) && !GlobalClass.AntiDDoSEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(iP + " was banned by Anti-DDoS system.");
                Console.ForegroundColor = ConsoleColor.White;
                Logging.LogDDoS(iP + " - " + DateTime.Now.ToString());
                mLastIpBlocked = iP;
                iP = null;
                return false;
            }
            int freeConnectionID = GetFreeConnectionID();
            if (freeConnectionID < 0)
            {
                return false;
            }
            mConnectionStorage[freeConnectionID] = iP;
            iP = null;
            return true;
        }

        internal static void FreeConnection(string IP)
        {
            for (int i = 0; i < mConnectionStorage.Length; i++)
            {
                if (mConnectionStorage[i] == IP)
                {
                    mConnectionStorage[i] = null;
                    return;
                }
            }
        }

        private static int GetConnectionAmount(string IP)
        {
            int num = 0;
            for (int i = 0; i < mConnectionStorage.Length; i++)
            {
                if (mConnectionStorage[i] == IP)
                {
                    num++;
                }
            }
            return num;
        }

        private static int GetFreeConnectionID()
        {
            for (int i = 0; i < mConnectionStorage.Length; i++)
            {
                if (mConnectionStorage[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        internal static void SetupTcpAuthorization(int ConnectionCount)
        {
            mConnectionStorage = new string[ConnectionCount];
        }
    }
}
