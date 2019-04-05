using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace decipherMain.Utils
{
    class contend
    {

        public static byte[] Enc(byte[] data,byte[] key)
        {
            byte[] result = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[i] = (byte)(data[i] ^ key[i]);
            }
            return result;
        }
    }
    public class Cryptographer
    {
        private byte[] Keys { get; set; }

        public Cryptographer(string password)
        {
            Keys = Encoding.ASCII.GetBytes(password);
        }

        public byte[] Encrypt(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ Keys[i % Keys.Length]);
            }
            return data;
        }

        public byte[] Decrypt(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(Keys[i % Keys.Length] ^ data[i]);
            }
            return data;
        }
    }
}
