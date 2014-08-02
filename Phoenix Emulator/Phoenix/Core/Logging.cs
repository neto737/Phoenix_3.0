using System;
using System.IO;
using System.Text;
namespace Phoenix.Core
{
	public sealed class Logging
	{
		private static bool IsRunning = false;

		internal static void Write(string Line)
		{
			if (!Logging.IsRunning)
			{
				Console.Write(Line);
			}
		}

		internal static void WriteLine(string Line)
		{
			if (!Logging.IsRunning)
			{
				Console.WriteLine(Line);
			}
		}

		internal static void LogException(string logText)
		{
			try
			{
				FileStream fileStream = new FileStream("exceptions.err", FileMode.Append, FileAccess.Write);
				byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": ",
					logText,
					"\r\n\r\n"
				}));
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Close();
			}
			catch (Exception)
			{
				Logging.WriteLine(DateTime.Now + ": " + logText);
			}
			Console.ForegroundColor = ConsoleColor.Red;
			Logging.WriteLine("Exception has been saved");
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		internal static void LogCriticalException(string logText)
		{
			try
			{
				FileStream fileStream = new FileStream("criticalexceptions.err", FileMode.Append, FileAccess.Write);
				byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": ",
					logText,
					"\r\n\r\n"
				}));
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Close();
				Console.ForegroundColor = ConsoleColor.Red;
				Logging.WriteLine("CRITICAL ERROR LOGGED");
				Console.ForegroundColor = ConsoleColor.Gray;
			}
			catch (Exception)
			{
				Logging.WriteLine(DateTime.Now + ": " + logText);
			}
		}

		internal static void LogCacheError(string logText)
		{
			try
			{
				FileStream fileStream = new FileStream("cacheerror.err", FileMode.Append, FileAccess.Write);
				byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": ",
					logText,
					"\r\n\r\n"
				}));
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Close();
			}
			catch (Exception)
			{
				Logging.WriteLine(DateTime.Now + ": " + logText);
			}
			Console.ForegroundColor = ConsoleColor.Red;
			Logging.WriteLine("Critical error saved");
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		internal static void LogDDoS(string logText)
		{
			try
			{
				FileStream fileStream = new FileStream("ddos.txt", FileMode.Append, FileAccess.Write);
				byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": ",
					logText,
					"\r\n\r\n"
				}));
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Close();
			}
			catch
			{
			}
			Logging.WriteLine(DateTime.Now + ": " + logText);
		}

		internal static void LogThreadException(string Exception, string Threadname)
		{
			try
			{
				FileStream fileStream = new FileStream("threaderror.err", FileMode.Append, FileAccess.Write);
				byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": Error in thread ",
					Threadname,
					": \r\n",
					Exception,
					"\r\n\r\n"
				}));
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Close();
				Console.ForegroundColor = ConsoleColor.Red;
				Logging.WriteLine("Error in " + Threadname + " caught");
				Console.ForegroundColor = ConsoleColor.Gray;
			}
			catch (Exception)
			{
				Logging.WriteLine(DateTime.Now + ": " + Exception);
			}
		}

		internal static void DisablePrimaryWriting()
		{
			Logging.IsRunning = true;
		}

		internal static void smethod_8(string logText)
		{
			throw new NotImplementedException();
		}
	}
}
