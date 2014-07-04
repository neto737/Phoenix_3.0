using System;
using System.Text;
using Phoenix.Util;
namespace Phoenix.Messages
{
    internal class ClientMessage
    {
        private byte[] Body;
        private uint MessageId;
        private int Pointer;

        public ClientMessage(uint _MessageId, byte[] _Body)
        {
            if (_Body == null)
            {
                _Body = new byte[0];
            }
            this.MessageId = _MessageId;
            this.Body = _Body;
            this.Pointer = 0;
        }

        public void AdvancePointer(int i)
        {
            this.Pointer += i;
        }

        public string GetBody()
        {
            return Encoding.Default.GetString(this.Body);
        }

        public byte[] PlainReadBytes(int Bytes)
        {
            if (Bytes > this.RemainingLength)
            {
                Bytes = this.RemainingLength;
            }
            byte[] buffer = new byte[Bytes];
            int index = 0;
            for (int i = this.Pointer; index < Bytes; i++)
            {
                buffer[index] = this.Body[i];
                index++;
            }
            return buffer;
        }

        public bool PopBase64Boolean()
        {
            return ((this.RemainingLength > 0) && (this.Body[this.Pointer++] == 0x41));
        }

        public int PopFixedInt32()
        {
            int result = 0;
            int.TryParse(this.PopFixedString(Encoding.ASCII), out result);
            return result;
        }

        public string PopFixedString()
        {
            return this.PopFixedString(PhoenixEnvironment.GetDefaultEncoding());
        }

        public string PopFixedString(Encoding encoding)
        {
            return encoding.GetString(this.ReadFixedValue()).Replace(Convert.ToChar(1), ' ');
        }

        public uint PopFixedUInt32()
        {
            return (uint)this.PopFixedInt32();
        }

        public int PopInt32()
        {
            return Base64Encoding.DecodeInt32(this.ReadBytes(2));
        }

        public uint PopUInt32()
        {
            return (uint)this.PopInt32();
        }

        public bool PopWiredBoolean()
        {
            return ((this.RemainingLength > 0) && (this.Body[this.Pointer++] == 0x49));
        }

        public int PopWiredInt32()
        {
            if (this.RemainingLength < 1)
            {
                return 0;
            }
            byte[] bzData = this.PlainReadBytes(6);
            int totalBytes = 0;
            int num2 = WireEncoding.DecodeInt32(bzData, out totalBytes);
            this.Pointer += totalBytes;
            return num2;
        }

        public uint PopWiredUInt()
        {
            return (uint)this.PopWiredInt32();
        }

        public byte[] ReadBytes(int Bytes)
        {
            if (Bytes > this.RemainingLength)
            {
                Bytes = this.RemainingLength;
            }
            byte[] buffer = new byte[Bytes];
            for (int i = 0; i < Bytes; i++)
            {
                buffer[i] = this.Body[this.Pointer++];
            }
            return buffer;
        }

        public byte[] ReadFixedValue()
        {
            int bytes = Base64Encoding.DecodeInt32(this.ReadBytes(2));
            return this.ReadBytes(bytes);
        }

        public void ResetPointer()
        {
            this.Pointer = 0;
        }

        public override string ToString()
        {
            return (this.Header + PhoenixEnvironment.GetDefaultEncoding().GetString(this.Body));
        }

        public string Header
        {
            get
            {
                return Encoding.Default.GetString(Base64Encoding.Encodeuint(this.MessageId, 2));
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
                return this.Body.Length;
            }
        }

        public int RemainingLength
        {
            get
            {
                return (this.Body.Length - this.Pointer);
            }
        }
    }
}
