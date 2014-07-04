using System;
namespace Phoenix.Storage
{
    public class DatabaseServer
    {
        private readonly string mHost;
        private readonly string mPassword;
        private readonly uint mPort;
        private readonly string mUser;

        public DatabaseServer(string sHost, uint Port, string sUser, string sPassword)
        {
            if ((sHost == null) || (sHost.Length == 0))
            {
                throw new ArgumentException("sHost");
            }
            if ((sUser == null) || (sUser.Length == 0))
            {
                throw new ArgumentException("sUser");
            }
            this.mHost = sHost;
            this.mPort = Port;
            this.mUser = sUser;
            this.mPassword = (sPassword != null) ? sPassword : "";
        }

        public override string ToString()
        {
            return (this.mUser + "@" + this.mHost);
        }

        public string Host
        {
            get
            {
                return this.mHost;
            }
        }

        public string Password
        {
            get
            {
                return this.mPassword;
            }
        }

        public uint Port
        {
            get
            {
                return this.mPort;
            }
        }

        public string User
        {
            get
            {
                return this.mUser;
            }
        }
    }
}
