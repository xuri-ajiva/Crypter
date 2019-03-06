using System;
using System.IO;
using System.Text;

namespace Cipher
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            //var taget = (Environment.SystemDirectory + "\\" + Path.GetFileName(Application.ExecutablePath));
            //if (Application.ExecutablePath != taget)
            //{
            //    File.Copy(Application.ExecutablePath, taget, true);
            //    string argumente = "";
            //    if (args.Length > 0)
            //        for (int i = 0; i < args.Length; i++)
            //        {
            //            argumente += args[i] + " ";
            //        }
            //    Process.Start(taget, argumente);
            //    Environment.Exit(99);
            //}

            string passwd = "";
            string file = "";

            if (args.Length == 0)
                goto Start;
            if (args.Length != 3)
            {
                Console.WriteLine("Use: crypter [Decrypt: d / Encrypt: e] [Passwd] [PathToFile]"); Environment.Exit(-1);
            }


            passwd = args[1];
            file = args[2];


            //en or decrypt
            if (args[0].ToLower() == "d")
                goto dencryptedstring_console;
            if (args[0].ToLower() == "e")
                goto encryptedstring_console;



            Ende:
            Console.WriteLine("Finished !");
            Console.ReadKey();
            Environment.Exit(0);

        Start:
            string choise = "";
            while (true)
            {
                Console.WriteLine("Encrypr/Decrypt? [E/D]:");
                choise = Console.ReadLine();
                if (choise.ToLower() == "e")
                    goto encryptedstring;
                if (choise.ToLower() == "d")
                    goto dencryptedstring;
            }
        encryptedstring:
            Console.WriteLine("Please enter a password to use:");
            string passwordToEncrypt = Console.ReadLine();
            Console.WriteLine("Please enter a File to encrypt:");
            string pathToFile = Console.ReadLine();
            string plaintextToEncrypt = Encoding.UTF8.GetString(File.ReadAllBytes(pathToFile));
            Console.WriteLine("");

            Console.WriteLine("Your encrypted File is:");
            string encryptedstringToEncrypt = StringCipher.Encrypt(plaintextToEncrypt, passwordToEncrypt);
            Console.WriteLine(encryptedstringToEncrypt);
            var new_Path = Path.GetDirectoryName(pathToFile) + "\\" + Path.GetFileName(pathToFile) + ".dec";
            File.Create(new_Path).Close();
            File.WriteAllBytes(new_Path, Encoding.ASCII.GetBytes(encryptedstringToEncrypt));
            Console.WriteLine("Written all to: " + new_Path);
            Console.WriteLine("");

            Console.WriteLine("Your decrypted File is:");
            string decryptedstringToEncrypt = StringCipher.Decrypt(encryptedstringToEncrypt, passwordToEncrypt);
            Console.WriteLine(decryptedstringToEncrypt);
            Console.WriteLine("");
            goto Ende;



        dencryptedstring:
            Console.WriteLine("Please enter a password to use:");
            string passwordToDencrypt = Console.ReadLine();
            Console.WriteLine("Please enter a File to decrypt:");
            string _pathToFile = Console.ReadLine();
            string encryptedstringToDencrypt = Encoding.UTF8.GetString(File.ReadAllBytes(_pathToFile));
            //Console.WriteLine(encryptedstringToDencrypt);
            Console.WriteLine("");

            Console.WriteLine("Your decrypted string is:");
            string decryptedstringToDencrypt = StringCipher.Decrypt(encryptedstringToDencrypt, passwordToDencrypt);
            Console.WriteLine(decryptedstringToDencrypt);
            var _new_Path = Path.GetDirectoryName(_pathToFile) + "\\" + Path.GetFileName(_pathToFile).Replace(".dec", "");
            File.Create(_new_Path).Close();
            File.WriteAllBytes(_new_Path, Encoding.ASCII.GetBytes(decryptedstringToDencrypt));
            Console.WriteLine("Written all to: "+ _new_Path);
            Console.WriteLine("");
            goto Ende;

        dencryptedstring_console:
            string __encryptedstringToDencrypt = Encoding.UTF8.GetString(File.ReadAllBytes(file));
            //Console.WriteLine(encryptedstringToDencrypt);

            string __decryptedstringToDencrypt = StringCipher.Decrypt(__encryptedstringToDencrypt, passwd);
            //Console.WriteLine(__decryptedstringToDencrypt);
            var ___new_Path = Path.GetDirectoryName(file) + "\\" + Path.GetFileName(file).Replace(".dec", "");
            File.Create(___new_Path).Close();
            File.WriteAllBytes(___new_Path, Encoding.ASCII.GetBytes(__decryptedstringToDencrypt));
            Console.WriteLine("Written all to: " + ___new_Path);
            goto Ende;

        encryptedstring_console:
            string __plaintextToEncrypt = Encoding.UTF8.GetString(File.ReadAllBytes(file));

            string __encryptedstringToEncrypt = StringCipher.Encrypt(__plaintextToEncrypt, passwd);
            //Console.WriteLine(__encryptedstringToEncrypt);
            var __new_Path = Path.GetDirectoryName(file) + "\\" + Path.GetFileName(file) + ".dec";
            File.Create(__new_Path).Close();
            File.WriteAllBytes(__new_Path, Encoding.ASCII.GetBytes(__encryptedstringToEncrypt));
            Console.WriteLine("Written all to: " + __new_Path);
            goto Ende;
        }
    }
}
