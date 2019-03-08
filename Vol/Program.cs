using decipher;
using System;
using System.Diagnostics;

namespace Vol
{
    internal class Program
    {
        //replace thes strings with your encodet strings

            //is not a working string 
        public static string _passwd_ = "fgTLbSyV1ymjO3unA0AGR7p+cQsT8QYf0ZQ/cjJ/SN3AEFbIsBNQ/LOkeii2GiDA/FXnT17lkJGJeAHDVu++vb2zzNUTRZuu38vyQ7LDmnTXkM7x3QsMeSJ5F0lnlGLj5O";

        public static string Golbal_vonumendatei = "o+KpbWPXMJXImc6hqjlR2i/q/4CdgJFLMGjOdZ/zNkzPmpMHvmE4jEi+Ar3xfbsCUQihd8cKxirPEqwq0IXMGWXY2TjKNoeHJv3xGFWOgIhEr6QAE1caLaC8XSE7+yPd";
        public static string Golbal_Passwd = "QiCUBrqBZkjjnIk936JIgszjtV0y2hG/cvTHmInbSOAhi8aP2josP0+z184FMwg4SqggC8zKe7Sj7aNd86Z9lmoinm5dma0iiN8m6Ygay7Pbnr1MsU4dFzVMTupHirW1";
        public static string Golbal_keydatei = "8d1EMUqLl47xoVPrn/8PaQ8iZlHf4rwy6xB2d2I5YHnUZ1oTdl6zowTLySleC4SsYn7sTiwfQsAaKgWE8uIkTBqCiRn+ikiKGo2DQ1CAEk41vcKajEsDyC3gQi2g58Yh";

        private static void Main(string[] args)
        {
            Console.WriteLine("Passwort: ");

            string passwd="";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    passwd += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && passwd.Length > 0)
                    {
                        passwd = passwd.Substring(0, (passwd.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            
            //Console.WriteLine("passwd: "+StringCipher.Encrypt(passwd,passwd));
            //Console.WriteLine("Password: " + StringCipher.Encrypt(Golbal_Passwd, passwd));
            //Console.WriteLine("Volu: "+ StringCipher.Encrypt(Golbal_vonumendatei,passwd));
            //Console.WriteLine("Key: " + StringCipher.Encrypt(Golbal_keydatei, passwd));


            if (passwd == StringCipher.Decrypt(_passwd_, passwd))
            {
                //Console.WriteLine("key: "+ StringCipher.Decrypt(Golbal_keydatei, passwd));
                //Console.WriteLine("pas: "+StringCipher.Decrypt(Golbal_Passwd,passwd));
                //Console.WriteLine("vol: "+StringCipher.Decrypt(Golbal_vonumendatei,passwd));
                Console.WriteLine("\nPassword corect!");
                Console.WriteLine("Starting conect:");
                Console.WriteLine("[EXECUTING]: " + @"C:\Program Files\VeraCrypt\VeraCrypt.exe\ " + "/c /q /v " + StringCipher.Decrypt(Golbal_vonumendatei, passwd) + " /l i /p " + "[Password:Hide]" + /*StringCipher.Decrypt(Golbal_Passwd, passwd) +*/ " /k " + StringCipher.Decrypt(Golbal_keydatei, passwd) + "");
                Process.Start(@"C:\Program Files\VeraCrypt\VeraCrypt.exe\", "/c /q /v " + StringCipher.Decrypt(Golbal_vonumendatei, passwd) + " /l i /p " + StringCipher.Decrypt(Golbal_Passwd, passwd) + " /k " + StringCipher.Decrypt(Golbal_keydatei, passwd) + "");
            }
            Console.Read();
        }
    }
}
