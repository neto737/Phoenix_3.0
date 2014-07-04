using System;
using System.Net.Sockets;
using Phoenix.Core;
namespace Phoenix.Net
{
    internal class TcpConnectionManager
    {
        private TcpConnection[] Connections;
        private TcpConnectionListener Listener;

        public TcpConnectionManager(string LocalIP, int Port, int maxSimultaneousConnections)
        {
            this.Connections = new TcpConnection[maxSimultaneousConnections];
            this.Listener = new TcpConnectionListener(LocalIP, Port, this);
        }

        public bool ContainsConnection(uint Id)
        {
            return (this.Connections[Id] != null);
        }

        public void DestroyManager()
        {
            for (int i = 0; i < this.Connections.Length; i++)
            {
                if (this.Connections[i] != null)
                {
                    this.Connections[i].Dispose();
                }
            }
            this.Connections = null;
            this.Listener = null;
        }

        internal void DropConnection(uint Id)
        {
            this.Connections[Id] = null;
        }

        internal int GenerateConnectionID()
        {
            return Array.IndexOf<TcpConnection>(this.Connections, null);
        }

        public TcpConnection GetConnection(uint Id)
        {
            return this.Connections[Id];
        }

        public TcpConnectionListener GetListener()
        {
            return this.Listener;
        }

        internal void HandleNewConnection(SocketInformation connectioninfo, int PreconnID)
        {
            TcpConnection connection = new TcpConnection(Convert.ToUInt32(PreconnID), connectioninfo);
            this.Connections[PreconnID] = connection;
            PhoenixEnvironment.GetGame().GetClientManager().CreateAndStartClient((uint)PreconnID, ref connection);
            if (PhoenixEnvironment.GetConfig().data["emu.messages.connections"] == "1")
            {
                Logging.WriteLine(string.Concat(new object[] { ">> Connection [", PreconnID, "] from [", connection.ipAddress, "]" }));
            }
        }

        internal void Shutdown()
        {
            this.Listener.Close();
            for (int i = 0; i < this.Connections.Length; i++)
            {
                if (this.Connections[i] != null)
                {
                    this.Connections[i].Dispose();
                }
            }
        }

        public int AmountOfActiveConnections
        {
            get
            {
                int num = 0;
                for (int i = 0; i < this.Connections.Length; i++)
                {
                    if (this.Connections[i] != null)
                    {
                        num++;
                    }
                }
                return num;
            }
        }
    }
}
