using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using Phoenix.Util;
using Phoenix.Messages;
namespace Phoenix.Net
{
    public class TcpConnection : Socket, IDisposable
    {
        private byte[] mDataBuffer;
        private AsyncCallback mDataReceivedCallback;
        private AsyncCallback mDataSentCallback;
        private bool mDisposed;
        private readonly uint mID;
        private string mIP;
        private RouteReceivedDataCallback mRouteReceivedDataCallback;

        public TcpConnection(uint pSockID, SocketInformation pSockInfo)
            : base(pSockInfo)
        {
            this.mID = pSockID;
            this.mIP = base.RemoteEndPoint.ToString().Split(new char[] { ':' })[0];
        }

        internal void ConnectionDead()
        {
            try
            {
                this.Dispose();
                PhoenixEnvironment.GetGame().GetClientManager().StopClient(this.mID);
            }
            catch
            {
            }
        }

        private void DataReceived(IAsyncResult iAr)
        {
            if (!this.mDisposed)
            {
                try
                {
                    int numBytes = 0;
                    try
                    {
                        numBytes = base.EndReceive(iAr);
                    }
                    catch
                    {
                        this.ConnectionDead();
                        return;
                    }
                    if (numBytes > 0)
                    {
                        byte[] data = ByteUtil.ChompBytes(this.mDataBuffer, 0, numBytes);
                        this.RouteData(ref data);
                    }
                    else
                    {
                        this.ConnectionDead();
                        return;
                    }
                    this.WaitForData();
                }
                catch
                {
                    this.ConnectionDead();
                }
            }
        }

        private void DataSent(IAsyncResult Iar)
        {
            if (!this.mDisposed)
            {
                try
                {
                    base.EndSend(Iar);
                }
                catch
                {
                    this.ConnectionDead();
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool Disposing)
        {
            if (!this.mDisposed && Disposing)
            {
                this.mDisposed = true;
                try
                {
                    base.Shutdown(SocketShutdown.Both);
                    base.Close();
                    base.Dispose();
                }
                catch { }

                Array.Clear(this.mDataBuffer, 0, this.mDataBuffer.Length);
                this.mDataBuffer = null;
                this.mDataReceivedCallback = null;
                this.mRouteReceivedDataCallback = null;
                PhoenixEnvironment.GetConnectionManager().DropConnection(this.mID);
                TcpAuthorization.FreeConnection(this.mIP);
                if (PhoenixEnvironment.GetConfig().data["emu.messages.connections"] == "1")
                {
                    Console.WriteLine(string.Concat(new object[] { ">> Connection Dropped [", this.mID, "] from [", this.ipAddress, "]" }));
                }
            }
        }

        public static string EncryptDecrypt(string textToEncrypt)
        {
            StringBuilder builder = new StringBuilder(textToEncrypt);
            StringBuilder builder2 = new StringBuilder(textToEncrypt.Length);
            for (int i = 0; i < textToEncrypt.Length; i++)
            {
                char ch = builder[i];
                ch = (char)(ch ^ '\x0081');
                builder2.Append(ch);
            }
            return builder2.ToString();
        }

        private void RouteData(ref byte[] Data)
        {
            if (this.mRouteReceivedDataCallback != null)
            {
                this.mRouteReceivedDataCallback(ref Data);
            }
        }

        internal void SendData(byte[] Data)
        {
            if (!this.mDisposed)
            {
                try
                {
                    if (base.Connected)
                    {
                        SocketError error;
                        base.BeginSend(Data, 0, Data.Length, SocketFlags.None, out error, this.mDataSentCallback, this);
                        if ((error != SocketError.Success) && (error != SocketError.IOPending))
                        {
                            PhoenixEnvironment.GetGame().GetClientManager().AddGameClient(this);
                        }
                    }
                    else
                    {
                        PhoenixEnvironment.GetGame().GetClientManager().AddGameClient(this);
                    }
                }
                catch
                {
                    PhoenixEnvironment.GetGame().GetClientManager().AddGameClient(this);
                }
            }
        }

        public void SendData(string sData)
        {
            this.SendData(PhoenixEnvironment.GetDefaultEncoding().GetBytes(sData));
        }

        public void SendMessage(ServerMessage Message)
        {
            if (Message != null)
            {
                this.SendData(Message.GetBytes());
            }
        }

        internal void Start(RouteReceivedDataCallback dataRouter)
        {
            this.mDataBuffer = new byte[0x400];
            this.mDataReceivedCallback = new AsyncCallback(this.DataReceived);
            this.mDataSentCallback = new AsyncCallback(this.DataSent);
            this.mRouteReceivedDataCallback = dataRouter;
            this.WaitForData();
        }

        private void WaitForData()
        {
            if (!this.mDisposed)
            {
                try
                {
                    base.BeginReceive(this.mDataBuffer, 0, 0x400, SocketFlags.None, this.mDataReceivedCallback, this);
                }
                catch (Exception)
                {
                    this.ConnectionDead();
                }
            }
        }

        public uint ID
        {
            get
            {
                return this.mID;
            }
        }

        public string ipAddress
        {
            get
            {
                return this.mIP;
            }
        }

        public delegate void RouteReceivedDataCallback(ref byte[] Data);
    }
}
