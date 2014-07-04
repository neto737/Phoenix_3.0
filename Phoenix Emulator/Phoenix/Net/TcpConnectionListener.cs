using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Phoenix.Core;
namespace Phoenix.Net
{
    internal class TcpConnectionListener
    {
        private AsyncCallback ConnectionReqCallback;
        private bool IsListening;
        private Socket Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private TcpConnectionManager Manager;
        private int mSystraID = Process.GetCurrentProcess().Id;
        private const int QUEUE_LENGTH = 1;

        public TcpConnectionListener(string LocalIp, int Port, TcpConnectionManager Manager)
        {
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, Port);
            this.Listener.Bind(localEP);
            this.Listener.Listen(1000);
            this.ConnectionReqCallback = new AsyncCallback(this.ConnectionRequest);
            this.Manager = Manager;
            TcpAuthorization.SetupTcpAuthorization(20000);
            Logging.WriteLine("Listening for connections on port: " + Port);
        }

        internal void Close()
        {
            this.IsListening = false;
            try
            {
                this.Listener.Shutdown(SocketShutdown.Both);
                this.Listener.Close();
                this.Listener.Dispose();
            }
            catch
            {
            }
        }

        private void ConnectionRequest(IAsyncResult iAr)
        {
            if (this.IsListening)
            {
                try
                {
                    int preconnID = this.Manager.GenerateConnectionID();
                    if (preconnID > -1)
                    {
                        Socket sock = ((Socket)iAr.AsyncState).EndAccept(iAr);
                        if (TcpAuthorization.CheckConnection(sock))
                        {
                            this.Manager.HandleNewConnection(sock.DuplicateAndClose(this.mSystraID), preconnID);
                        }
                        else
                        {
                            try
                            {
                                sock.Dispose();
                                sock.Close();
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logging.LogException("[TCPListener.OnRequest]: Could not handle new connection request: " + exception.ToString());
                }
                finally
                {
                    this.WaitForNextConnection();
                }
            }
        }

        public void Destroy()
        {
            this.Stop();
            this.Listener = null;
            this.Manager = null;
        }

        public void Start()
        {
            if (!this.IsListening)
            {
                this.IsListening = true;
                this.Listener.BeginAccept(this.ConnectionReqCallback, this.Listener);
            }
        }

        public void Stop()
        {
            if (this.IsListening)
            {
                this.IsListening = false;
                try
                {
                    this.Listener.Close();
                }
                catch
                {
                }
                Console.WriteLine("Listener -> Stopped!");
            }
        }

        private void WaitForNextConnection()
        {
            try
            {
                this.Listener.BeginAccept(this.ConnectionReqCallback, this.Listener);
            }
            catch
            {
            }
        }

        public bool isListening
        {
            get
            {
                return this.isListening;
            }
        }
    }
}
