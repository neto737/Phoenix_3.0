using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Phoenix.Core;
namespace Phoenix.Net
{
	internal sealed class MusSocket
	{
        public HashSet<string> allowedIps;
		public Socket Sock;
		public string Host;
		public int Port;

		public MusSocket(string mHost, int mPort, string[] mAllowedIps, int backlog)
		{
			this.Host = mHost;
			this.Port = mPort;
			this.allowedIps = new HashSet<string>();
            foreach (string str in mAllowedIps)
            {
                this.allowedIps.Add(str);
            }
			try
			{
				this.Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				this.Sock.Bind(new IPEndPoint(IPAddress.Parse(PhoenixEnvironment.GetConfig().data["mus.tcp.bindip"]), this.Port));
				this.Sock.Listen(backlog);
				this.Sock.BeginAccept(new AsyncCallback(this.OnEvent_NewConnection), this.Sock);
				Logging.WriteLine("Listening for MUS on port: " + this.Port);
			}
			catch (Exception ex)
			{
				throw new Exception("Could not set up MUS socket:\n" + ex.ToString());
			}
		}

		public void OnEvent_NewConnection(IAsyncResult iAr)
		{
            try
            {
                Socket Sock = ((Socket)iAr.AsyncState).EndAccept(iAr);
                string item = Sock.RemoteEndPoint.ToString().Split(new char[] { ':' })[0];
                if (this.allowedIps.Contains(item))
                {
                    new MusConnection(Sock);
                }
                else
                {
                    Sock.Close();
                }
            }
            catch (Exception) { }

            this.Sock.BeginAccept(new AsyncCallback(this.OnEvent_NewConnection), this.Sock);
		}
	}
}
