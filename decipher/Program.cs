using System;
using System.Collections.Generic;
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
        public static string name = "----------------------------------------------------------- decipher -----------------------------------------------------------\n";
        public static bool binery = false;
        private static void Main(string[] args)
        {

            if (args.Length == 0)
                UserImput();
            if (args.Length != 4)
            {
                Console.WriteLine("Use: crypter [Decrypt: d / Encrypt: e] [Bin: true/false] [Passwd] [PathToFile]"); Environment.Exit(-1);
            }

            passwd = args[2];
            file = args[3];
            binery = bool.Parse(args[1]);

            if (binery)
            {
                //en or decrypt
                if (args[0].ToLower() == "d")
                    __Decrypt();
                if (args[0].ToLower() == "e")
                    __Encrypt();
            }
            else
            {
                if (args[0].ToLower() == "e")
                    Encrypt();
                if (args[0].ToLower() == "d")
                    Decrypt();
            }
        }

        public static void Encrypt()
        {
            plaintext = Encoding.UTF8.GetString(File.ReadAllBytes(file));
            plaintext = fullsize("User:" + Environment.UserName + "  MachineName:" + Environment.MachineName) + fullsize(Path.GetExtension(file)) + plaintext;

            encryptedstring = Encryption.EncryptString(plaintext, passwd);

            //encryptedstring = StringCipher.Encrypt(plaintext, passwd);


            var new_Path = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + ".dec";
            File.Create(new_Path).Close();

            var temp = StringExtensions.SplitInParts(encryptedstring, name.Length);

            string finalstring = "";

            foreach (var item in temp)
            {
                finalstring += item + "\n";
            }

            //Console.WriteLine(name + "\n" + finalstring + name);

            var bytes = Encoding.ASCII.GetBytes(name + finalstring + name);
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
            if (encryptedstring.Substring(0, name.Length) != name)
            {
                Console.WriteLine("keine gültige datei");
                Ende();
            }
            encryptedstring = encryptedstring.Substring(name.Length);
            encryptedstring = encryptedstring.Substring(0, encryptedstring.Length - name.Length);

            encryptedstring = encryptedstring.Replace("\n", "");

            //Console.WriteLine(encryptedstring);

            //Console.Read();

            //decryptedstring = StringCipher.Decrypt(encryptedstring, passwd);
            decryptedstring = Encryption.DecryptString(encryptedstring, passwd);

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

        public static void __Encrypt()
        {
            EncDec.Encrypt(file, file+".dec", passwd);
            Ende();
        }

        public static void __Decrypt()
        {
            EncDec.Decrypt(file,Path.GetDirectoryName(file)+"\\"+ Path.GetFileNameWithoutExtension(file), passwd);
            Ende();
        }

        public static void _Encrypt()
        {
            var temp_file = Path.GetFileNameWithoutExtension(file) + ".dec.temp";

            EncDec.Encrypt(file, temp_file, passwd);

            plaintext = fullsize("User:" + Environment.UserName + "  MachineName:" + Environment.MachineName) + fullsize(Path.GetExtension(file));

            encryptedstring = Encryption.EncryptString(plaintext, passwd);


            var new_Path = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + ".dec";
            File.Create(new_Path).Close();

            var temp = StringExtensions.SplitInParts(encryptedstring, name.Length);

            string finalstring = "";

            foreach (var item in temp)
            {
                finalstring += item + "\n";
            }

            //Console.WriteLine(name + "\n" + finalstring + name);

            if (finalstring.Length > 128)
                Console.WriteLine("expectete error!");

            var byte1 = Encoding.ASCII.GetBytes(name + finalstring);
            var byte2 = File.ReadAllBytes(temp_file);
            var byte3 = Encoding.ASCII.GetBytes(name);


            byte[] bytes = new byte[byte1.Length + byte2.Length + byte3.Length];

            int t = 0;

            for (int i = t; i < byte1.Length; i++)
            {
                bytes[i] = byte1[i - t];
            }
            t += byte1.Length;
            for (int i = t; i < byte2.Length + t; i++)
            {
                bytes[i] = byte2[i - t];
            }
            t += byte2.Length;
            for (int i = t; i < byte3.Length + t; i++)
            {
                bytes[i] = byte3[i - t];
            }
            t += byte3.Length;

            //not hapend
            if (t != bytes.Length)
                Console.WriteLine("fatal error: {0},{1}", t, bytes.Length);


            File.WriteAllBytes(new_Path, bytes);
            Console.WriteLine("Written all to: " + new_Path);
            Ende();
        }

        public static void _Decrypt()
        {

            List<byte> list;



            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))

            {

                byte[] arrfile = new byte[fs.Length];

                fs.Read(arrfile, 0, arrfile.Length);

                list = new List<byte>(arrfile);

            }

            var name_ = Encoding.ASCII.GetBytes(name);

            for (int i = 0; i < name_.Length; i++)
            {
                if (name_[i] != list[i])
                {
                    Console.WriteLine("error no suported file");
                    Ende();
                }
            }

            for (int i = 0; i < name.Length; i++)
            {
                list.RemoveAt(0);
            }

            for (int i = 0; i < name.Length; i++)
            {
                list.RemoveAt(list.Count - 1);
            }

            string headers = "";

            for (int i = 0; i < name.Length; i++)
            {
                headers += Encoding.ASCII.GetString(new byte[] { list[0] });
                list.RemoveAt(0);
            }

            Console.WriteLine(headers);
            byte[] resultArr = list.ToArray();

            encryptedstring = Encoding.UTF8.GetString(File.ReadAllBytes(file));
            //Console.WriteLine(encryptedstringToDencrypt);
            Console.WriteLine("");

            Console.WriteLine("Your decrypted file is:");
            if (encryptedstring.Substring(0, name.Length) != name)
            {
                Console.WriteLine("keine gültige datei");
                Ende();
            }
            encryptedstring = encryptedstring.Substring(name.Length);
            encryptedstring = encryptedstring.Substring(0, encryptedstring.Length - name.Length);

            //encryptedstring = encryptedstring.Replace("\n", "");

            //Console.WriteLine(encryptedstring);

            //Console.Read();

            //decryptedstring = StringCipher.Decrypt(encryptedstring, passwd);
            decryptedstring = Encryption.DecryptString(headers, passwd);

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

            //if (decryptedstring.Substring(0, 1) == "?")
            //    decryptedstring = decryptedstring.Substring(1);

            Console.WriteLine("info: " + Info);

            var new_Path = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + Dateiname;
            File.Create(new_Path).Close();
            File.WriteAllBytes(new_Path, resultArr);
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
            Console.WriteLine("use binery mode (must use for a *.exe) [true/false]:");
            var r = Console.ReadLine();

            binery = bool.Parse(r);

            Console.WriteLine(binery);

            string choise = "";
            while (true)
            {
                Console.WriteLine("Encrypr/Decrypt? [E/D]:");
                choise = Console.ReadLine();

                if (binery)
                {
                    if (choise.ToLower() == "e")
                        __Encrypt();
                    if (choise.ToLower() == "d")
                        __Decrypt();
                }
                else {
                    if (choise.ToLower() == "e")
                        Encrypt();
                    if (choise.ToLower() == "d")
                        Decrypt();
                }
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

    internal static class StringExtensions
    {

        public static IEnumerable<string> SplitInParts(this string s, int partLength)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

    }
}
