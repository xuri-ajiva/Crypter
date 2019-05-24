using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using decipherMain.Utils;

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
        public static string DateiEndung = "";
        public static string name = "----------------------------------------------------------- decipher -----------------------------------------------------------\n";
        public static bool binery = false;
        public static string new_Path = "";
        private static string EncEndung = ".dec";

        private static void Main(string[] args)
        {

            if (args.Length == 1) {
                file = args[0];
                UserImput();
            }
            if (args.Length == 0)
                UserImput();
            if (args.Length != 3)
            {
                Console.WriteLine("Use: crypter [Decrypt: d / Encrypt: e] [Passwd] [PathToFile]"); Environment.Exit(-1);
            }

            passwd = args[1];
            file = args[2];

            new_Path = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file);
            if (args[0].ToLower() == "e")
                Encrypt();
            if (args[0].ToLower() == "d")
                Decrypt();
            Ende();
        }
        public static void Encrypt()
        {
            plaintext = Encoding.Unicode.GetString(File.ReadAllBytes(file));
            plaintext = fullsize("User:" + Environment.UserName + "  MachineName:" + Environment.MachineName) + fullsize(Path.GetExtension(file)) + plaintext;

            encryptedstring = Encryption.EncryptString(plaintext, passwd);

            //encryptedstring = StringCipher.Encrypt(plaintext, passwd);
            new_Path += EncEndung;

            File.Create(new_Path).Close();

            var temp = StringExtensions.SplitInParts(encryptedstring, name.Length);

            string finalstring = "";

            foreach (var item in temp)
            {
                finalstring += item + "\n";
            }

            //Console.WriteLine(name + "\n" + finalstring + name);

            var bytes = Encoding.Unicode.GetBytes(name + finalstring + name);
            File.WriteAllBytes(new_Path, bytes);
            Console.WriteLine("Written all to: " + new_Path);
        }

        public static void Decrypt()
        {
            encryptedstring = Encoding.Unicode.GetString(File.ReadAllBytes(file));
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

            //decryptedstring = StringCipher.Decrypt(encryptedstring, passwd);
            decryptedstring = Encryption.DecryptString(encryptedstring, passwd);

            Info = "";
            DateiEndung = "";
            try
            {
                var readlength = short.Parse(decryptedstring.Substring(0, 5));
                Info = decryptedstring.Substring(5, readlength);
                var _readlength = short.Parse(decryptedstring.Substring(5 + readlength, 5));
                DateiEndung = decryptedstring.Substring(10 + readlength, _readlength);
                decryptedstring = decryptedstring.Substring(5 + readlength + 5 + _readlength);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //if (decryptedstring.Substring(0, 1) == "?")
            //    decryptedstring = decryptedstring.Substring(1);

            new_Path += DateiEndung;

            Console.WriteLine("info: " + Info);
            File.Create(new_Path).Close();
            File.WriteAllBytes(new_Path, Encoding.Unicode.GetBytes(decryptedstring));
            Console.WriteLine("Written all to: " + new_Path);
        }

        public static void Ende()
        {
            reset_vars();
            Console.WriteLine("Finished!\nPress Any Key To Exit...");
            var p = ParentProcessUtilities.GetParentProcess();
            if(p.ProcessName.ToLower() != "cmd")
                Console.ReadKey();
            Environment.Exit(0);
        }
        private static void UserImput()
        {
            if (passwd == "")
            {
                Console.WriteLine("Please enter a password to use:");
                passwd = Console.ReadLine();
            }
            if (file == "")
            {
                Console.WriteLine("Please enter a File to use:");
                file = Console.ReadLine();
            }

            new_Path = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file);
            string choise = "";

            while (true)
            {
                Console.WriteLine("Encrypr/Decrypt? [E/D]:");
                choise = Console.ReadLine();
                if (choise.ToLower() == "e")
                    Encrypt();
                else if (choise.ToLower() == "d")
                    Decrypt();
                else
                    continue;
                Ende();
            }
        }
        public static string fullsize(string daten = "teststring")
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
    /// <summary>
    /// A utility class to determine a process parent.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ParentProcessUtilities
    {
        // These members must match PROCESS_BASIC_INFORMATION
        internal IntPtr Reserved1;
        internal IntPtr PebBaseAddress;
        internal IntPtr Reserved2_0;
        internal IntPtr Reserved2_1;
        internal IntPtr UniqueProcessId;
        internal IntPtr InheritedFromUniqueProcessId;

        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ParentProcessUtilities processInformation, int processInformationLength, out int returnLength);

        /// <summary>
        /// Gets the parent process of the current process.
        /// </summary>
        /// <returns>An instance of the Process class.</returns>
        public static Process GetParentProcess()
        {
            return GetParentProcess(Process.GetCurrentProcess().Handle);
        }

        /// <summary>
        /// Gets the parent process of specified process.
        /// </summary>
        /// <param name="id">The process id.</param>
        /// <returns>An instance of the Process class.</returns>
        public static Process GetParentProcess(int id)
        {
            Process process = Process.GetProcessById(id);
            return GetParentProcess(process.Handle);
        }

        /// <summary>
        /// Gets the parent process of a specified process.
        /// </summary>
        /// <param name="handle">The process handle.</param>
        /// <returns>An instance of the Process class.</returns>
        public static Process GetParentProcess(IntPtr handle)
        {
            ParentProcessUtilities pbi = new ParentProcessUtilities();
            int returnLength;
            int status = NtQueryInformationProcess(handle, 0, ref pbi, Marshal.SizeOf(pbi), out returnLength);
            if (status != 0)
                throw new Win32Exception(status);

            try
            {
                return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());
            }
            catch (ArgumentException)
            {
                // not found
                return null;
            }
        }
    }
    public static class NotNeedet {
        public static string file = Program.file;
        public static string passwd = Program.passwd;
        public static string plaintext = "";
        public static string encryptedstring = "";
        public static string decryptedstring = "";
        public static string Info = "";
        public static string DateiEndung = "";
        public static string name = "----------------------------------------------------------- decipher -----------------------------------------------------------\n";
        public static bool binery = false;
        public static string new_Path = "";
        private static string EncEndung = ".dec";


        public static void __Encrypt()
        {
            EncDec.Encrypt(file, file + ".dec", passwd);
            Program.Ende();
        }

        public static void __Decrypt()
        {
            EncDec.Decrypt(file, Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file), passwd);
            Program.Ende();
        }

        public static void _Encrypt()
        {
            var temp_file = Path.GetFileNameWithoutExtension(file) + ".dec.temp";

            EncDec.Encrypt(file, temp_file, passwd);

            plaintext = Program.fullsize("User:" + Environment.UserName + "  MachineName:" + Environment.MachineName) + Program.fullsize(Path.GetExtension(file));

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

            var byte1 = Encoding.Unicode.GetBytes(name + finalstring);
            var byte2 = File.ReadAllBytes(temp_file);
            var byte3 = Encoding.Unicode.GetBytes(name);


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
            Program.Ende();
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

            var name_ = Encoding.UTF8.GetBytes(name);

            for (int i = 0; i < name_.Length; i++)
            {
                if (name_[i] != list[i])
                {
                    Console.WriteLine("error no suported file");
                    Program.Ende();
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
                headers += Encoding.Unicode.GetString(new byte[] { list[0] });
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
                Program.Ende();
            }
            encryptedstring = encryptedstring.Substring(name.Length);
            encryptedstring = encryptedstring.Substring(0, encryptedstring.Length - name.Length);

            //encryptedstring = encryptedstring.Replace("\n", "");

            //Console.WriteLine(encryptedstring);

            //Console.Read();

            //decryptedstring = StringCipher.Decrypt(encryptedstring, passwd);
            decryptedstring = Encryption.DecryptString(headers, passwd);

            Info = "";
            DateiEndung = "";
            try
            {
                var readlength = short.Parse(decryptedstring.Substring(0, 5));
                Info = decryptedstring.Substring(5, readlength);
                var _readlength = short.Parse(decryptedstring.Substring(5 + readlength, 5));
                DateiEndung = decryptedstring.Substring(10 + readlength, _readlength);
                decryptedstring = decryptedstring.Substring(5 + readlength + 5 + _readlength);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //if (decryptedstring.Substring(0, 1) == "?")
            //    decryptedstring = decryptedstring.Substring(1);

            Console.WriteLine("info: " + Info);

            var new_Path = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + DateiEndung;
            File.Create(new_Path).Close();
            File.WriteAllBytes(new_Path, resultArr);
            Console.WriteLine("Written all to: " + new_Path);
            Program.Ende();
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
