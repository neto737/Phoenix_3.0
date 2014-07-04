using System;
using System.Collections.Generic;
using System.Text;
using Phoenix.Util;
namespace Phoenix.Messages
{
    public class ServerMessage
    {
        private List<byte> Body;
        private uint MessageId;

        public ServerMessage()
        {
        }

        public ServerMessage(uint _MessageId)
        {
            this.Init(_MessageId);
        }

        public void AppendBoolean(bool Bool)
        {
            if (Bool)
            {
                this.Body.Add(0x49);
            }
            else
            {
                this.Body.Add(0x48);
            }
        }

        public void AppendByte(byte b)
        {
            this.Body.Add(b);
        }

        public void AppendBytes(byte[] Data)
        {
            if ((Data != null) && (Data.Length != 0))
            {
                this.Body.AddRange(Data);
            }
        }

        public void AppendInt32(int i)
        {
            this.AppendBytes(WireEncoding.EncodeInt32(i));
        }

        public void AppendRawInt32(int i)
        {
            this.AppendString(i.ToString(), Encoding.ASCII);
        }

        public void AppendRawUInt(uint i)
        {
            int num = (int)i;
            this.AppendRawInt32(num);
        }

        public void AppendString(string s)
        {
            this.AppendString(s, PhoenixEnvironment.GetDefaultEncoding());
        }

        public void AppendString(string s, Encoding encoding)
        {
            if ((s != null) && (s.Length != 0))
            {
                this.AppendBytes(encoding.GetBytes(s));
            }
        }

        public void AppendStringWithBreak(string s)
        {
            this.AppendStringWithBreak(s, 2);
        }

        public void AppendStringWithBreak(string s, byte BreakChar)
        {
            this.AppendString(s);
            this.AppendByte(BreakChar);
        }

        public void AppendUInt(uint i)
        {
            int num = (int)i;
            this.AppendInt32(num);
        }

        public void Clear()
        {
            this.Body.Clear();
        }

        public byte[] GetBytes()
        {
            byte[] buffer = new byte[this.Length + 3];
            byte[] buffer2 = Base64Encoding.Encodeuint(this.MessageId, 2);
            buffer[0] = buffer2[0];
            buffer[1] = buffer2[1];
            for (int i = 0; i < this.Length; i++)
            {
                buffer[i + 2] = this.Body[i];
            }
            buffer[buffer.Length - 1] = 1;
            return buffer;
        }

        public void Init(uint _MessageId)
        {
            this.MessageId = _MessageId;
            this.Body = new List<byte>();
        }

        public string ToBodyString()
        {
            return PhoenixEnvironment.GetDefaultEncoding().GetString(this.Body.ToArray());
        }

        public override string ToString()
        {
            return (this.Header + PhoenixEnvironment.GetDefaultEncoding().GetString(this.Body.ToArray()));
        }

        public string Header
        {
            get
            {
                return PhoenixEnvironment.GetDefaultEncoding().GetString(Base64Encoding.Encodeuint(this.MessageId, 2));
            }
        }

        public uint Id
        {
            get
            {
                return this.MessageId;
            }
        }

        public int Length
        {
            get
            {
                return this.Body.Count;
            }
        }
    }
}
