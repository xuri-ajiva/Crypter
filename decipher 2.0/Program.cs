using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Xml.Serialization;
using XMLSave;
using decipherMain.Utils;

namespace decipherMain
{
    internal static class Program
    {
        //* Kernel imports
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;
        //*

        private static string config = "config.xml";
        private static bool startconsol = true;

        private static string fileName = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location) + "_savedData.xml";
        private static string name = "";
        private static string passwd = "";
        private static string exe = "";
        private static string argus = "";
        private static string helpstring = " -------- Help -------- \nuse:\n    set name:passwd:execute:argus\n    load - load saved config\n    save - save current config\n    exit - exit programm\n    show - shows current config\n    start -start with current config\n    clear - clear variables";

        private static void Main(string[] args)
        {
            Cryptographer c = new Cryptographer("aaaaaaaa");

            byte[] vs = Encoding.Unicode.GetBytes("'sÙ%šM6›>‚êUÝãÄ›ýÿ%šM6›>‚êýÿãÄ›");

            printeByte(vs);

            var r =c.Encrypt(vs);
            Console.WriteLine();
            printeByte(r);
            Console.WriteLine();
            printeByte(c.Decrypt(r));
            //var handle = GetConsoleWindow();
            //FixExeptions();
            //load();
            //loadConfig();
            //if (!startconsol)
            //    ShowWindow(handle, SW_HIDE);


            //if (exe != "" && name != "")
            //{
            //    start();
            //}
            //while (startconsol)
            //    Consol();
            Console.WriteLine("Press any Key to exit...");
            //if (!startconsol)
                Console.ReadKey();
        }

        private static void printeByte(byte[] b)
        {
           Console.WriteLine(Encoding.Unicode.GetString(b));
        }

        private static void Consol()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(Environment.UserName + "@" + Environment.MachineName);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" ~");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                path = Directory.GetParent(path).ToString();
            }

            Console.WriteLine("" + Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace(path, "").Replace('/', '\\')) + "\\");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" $ ~> ");
            Console.ForegroundColor = ConsoleColor.White;
            string comm = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Command(comm);
        }

        private static void FixExeptions()
        {
            if (!File.Exists(config))
                File.Create(config).Close();
            if (!File.Exists(fileName))
                File.Create(fileName).Close();
        }

        private static void loadConfig()
        {
            FileStream fs = null;
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(config));
                fs = new FileStream(config, FileMode.Open, FileAccess.Read, FileShare.Read);
                config c = (config)xs.Deserialize(fs);
                startconsol = c.startconsol;
                fs.Close();
            }
            catch (Exception e) { System.Console.WriteLine("ConfigFileLoadError: " + e.Message); fs.Close(); }

        }

        private static void Command(string comm)
        {
            if ((comm.ToLower() == "exit"))
                Environment.Exit(0);
            else if (comm.ToLower() == "help")
                Console.WriteLine(helpstring);
            else if (comm.ToLower() == "show")
                Console.WriteLine("name: " + name + "\npasswd: " + passwd + "\nexe: " + exe + "\nargus: " + argus);
            else if (comm.ToLower() == "load")
                load();
            else if (comm.ToLower() == "save")
                save();
            else if (comm.ToLower() == "start")
                start();
            else if (comm.ToLower() == "clear")
                clear();
            else
            {
                var vs = comm.Split(' ');
                string data = "";
                if (vs[0].ToLower() == "set")
                {
                    for (int i = 1; i < vs.Length; i++)
                    {
                        data += vs[i] + (vs.Length - 1 == i ? "" : " ");
                    }
                    //Console.WriteLine("|"+data + "|");
                    var save = data.Split(':');
                    name = save[0];
                    passwd = save[1];
                    exe = save[2];
                    argus = save[3];
                }
                else if (vs[0].ToLower() == "editconfig")
                {
                    for (int i = 1; i < vs.Length; i++)
                    {
                        data += vs[i] + (vs.Length - 1 == i ? "" : " ");
                    }
                    //Console.WriteLine("|"+data + "|");
                    var save = data.Split(':');
                    startconsol = save[0] == "1" ? true : false;

                    saveConfog();
                }
                else
                    Console.WriteLine(helpstring);
            }
        }

        private static void saveConfog()
        {
            try
            {
                config c = new config();
                c.startconsol = startconsol;
                dataSave.SaveData(c, config);
            }
            catch (Exception ex) { Console.WriteLine("ConfigFileSaveError: " + ex.Message); }
        }

        private static void start()
        {
            try
            {

                //var p = Process.Start("psexec.exe", $"\\\\{Environment.MachineName} -u {name} -p {passwd} {exe}");
                var secure = new SecureString();
                foreach (char c in passwd)
                {
                    secure.AppendChar(c);
                }
                var st = DateTime.Now;
                var p = Process.Start(exe, argus, name, secure, null);
                Console.Write("Started Process: |" + p.StartInfo.FileName + " " + p.StartInfo.Arguments + "| with id: |" + p.Id);
                p.WaitForExit();
                Console.WriteLine("| With time espand: |" + (DateTime.Now - st) + "|");
            }
            catch (Exception e) { System.Console.WriteLine("ProcessStartError: " + e.Message); }
        }

        private static void save()
        {
            try
            {
                Daten daten = new Daten();
                daten.name = name;
                daten.passwd = passwd;
                daten.execute = exe;
                daten.argus = argus;
                dataSave.SaveData(daten, fileName);
            }
            catch (Exception ex) { Console.WriteLine("InitFileSaveError: " + ex.Message); }
        }

        private static void load()
        {
            if (File.Exists(fileName))
            {
                FileStream fs = null;
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Daten));
                    fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    Daten daten = (Daten)xs.Deserialize(fs);
                    name = daten.name;
                    passwd = daten.passwd;
                    exe = daten.execute;
                    argus = daten.argus;
                    fs.Close();
                }
                catch (Exception e) { System.Console.WriteLine("InitFileLoadError: " + e.Message); fs.Close(); }
            }
        }

        private static void clear()
        {
            Console.Clear();
            name = "";
            passwd = "";
            exe = "";
            argus = "";
        }
    }
}


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
        private static void DechipherMain(string[] args)
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
            EncDec.Encrypt(file, file + ".dec", passwd);
            Ende();
        }

        public static void __Decrypt()
        {
            EncDec.Decrypt(file, Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file), passwd);
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
                else
                {
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