using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Phoenix.Core;
namespace Phoenix
{
    public class Program
    {
        private static EventHandler _handler;
        private static bool ProgramLoaded = false;

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.MyHandler);
            _handler = (EventHandler)Delegate.Combine(_handler, new EventHandler(Program.Shutdown));
            SetConsoleCtrlHandler(_handler, true);
            try
            {
                new PhoenixEnvironment().Initialize();
                ProgramLoaded = true;
            }
            catch (Exception exception)
            {
                Console.Write(exception.ToString());
            }
            while (true)
            {
                Console.ReadKey();
            }
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Logging.DisablePrimaryWriting();
            Logging.LogCriticalException(((Exception)args.ExceptionObject).ToString());
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        private static bool Shutdown(CtrlType sig)
        {
            if (ProgramLoaded)
            {
                Logging.DisablePrimaryWriting();
                Console.Clear();
                Console.WriteLine("The server is saving users furniture, rooms, etc. WAIT FOR THE SERVER TO CLOSE, DO NOT EXIT THE PROCESS IN TASK MANAGER!!");
                PhoenixEnvironment.PerformShutDown("", true);
            }
            return true;
        }

        private enum CtrlType
        {
            CTRL_BREAK_EVENT = 1,
            CTRL_C_EVENT = 0,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private delegate bool EventHandler(Program.CtrlType sig);
    }
}