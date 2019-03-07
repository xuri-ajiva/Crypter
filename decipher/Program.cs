using System;
using System.IO;
using System.Text;

namespace decipher
{
    internal class Program
    {
        public static string passwd = "";
        public static string file = "";
        public static string plaintext = "";
        public static string encryptedstring = "";
        public static string decryptedstring = "";
        public static string Info = "";
        public static string Dateiname = "";
        private static void Main(string[] args)
        {
            if (args.Length == 0)
                UserImput();
            if (args.Length != 3)
            {
                Console.WriteLine("Use: crypter [Decrypt: d / Encrypt: e] [Passwd] [PathToFile]"); Environment.Exit(-1);
            }

            passwd = args[1];
            file = args[2];


            //en or decrypt
            if (args[0].ToLower() == "d")
                Decrypt();
            if (args[0].ToLower() == "e")
                Encrypt();
        }

        public static void Encrypt()
        {
            plaintext = Encoding.UTF8.GetString(File.ReadAllBytes(file));
            plaintext = fullsize("User:" + Environment.UserName + "  MachineName:" + Environment.MachineName) + fullsize(Path.GetExtension(file)) + plaintext;
            encryptedstring = StringCipher.Encrypt(plaintext, passwd);


            var new_Path = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + ".dec";
            File.Create(new_Path).Close();

            var bytes = Encoding.ASCII.GetBytes("decipher" + encryptedstring);
            File.WriteAllBytes(new_Path, bytes);
            Console.WriteLine("Written all to: " + new_Path);
            Ende();
        }

        public static void Decrypt()
        {
            encryptedstring = Encoding.UTF8.GetString(File.ReadAllBytes(file));
            //Console.WriteLine(encryptedstringToDencrypt);
            Console.WriteLine("");

            Console.WriteLine("Your decrypted file is:");
            if (encryptedstring.Substring(0, 8) != "decipher")
            {
                Console.WriteLine("keine gültige datei");
                Ende();
            }
            encryptedstring = encryptedstring.Substring(8);
            decryptedstring = StringCipher.Decrypt(encryptedstring, passwd);

            Info = "";
            Dateiname = "";
            try
            {
                var readlength = short.Parse(decryptedstring.Substring(0, 5));
                Info = decryptedstring.Substring(5, readlength);
                var _readlength = short.Parse(decryptedstring.Substring(5 + readlength, 5));
                Dateiname = decryptedstring.Substring(10 + readlength, _readlength);
                decryptedstring = decryptedstring.Substring(5 + readlength + 5 + _readlength);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (decryptedstring.Substring(0, 1) == "?")
                decryptedstring = decryptedstring.Substring(1);

            Console.WriteLine("info: " + Info);

            var new_Path = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + Dateiname;
            File.Create(new_Path).Close();
            File.WriteAllBytes(new_Path, Encoding.ASCII.GetBytes(decryptedstring));
            Console.WriteLine("Written all to: " + new_Path);
            Ende();
        }

        private static void Ende()
        {
            reset_vars();
            Console.WriteLine("Finished !");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private static void UserImput()
        {
            Console.WriteLine("Please enter a password to use:");
            passwd = Console.ReadLine();
            Console.WriteLine("Please enter a File to use:");
            file = Console.ReadLine();

            string choise = "";
            while (true)
            {
                Console.WriteLine("Encrypr/Decrypt? [E/D]:");
                choise = Console.ReadLine();
                if (choise.ToLower() == "e")
                    Encrypt();
                if (choise.ToLower() == "d")
                    Decrypt();
            }
        }
        private static string fullsize(string daten = "teststring")
        {
            string result = "";
            short length = (short)daten.Length;

            //Console.WriteLine("length: "+ length+"  "+ length.ToString().Length+"   "+ Int16.MaxValue.ToString().Length);
            for (int i = length.ToString().Length; i < short.MaxValue.ToString().Length; i++)
            {
                result += "0";
            }

            result += length.ToString();

            result += daten;
            return result;
        }
        private static void reset_vars()
        {
            passwd = "";
            file = "";

            plaintext = "";
            encryptedstring = "";
            decryptedstring = "";
        }
    }
}
