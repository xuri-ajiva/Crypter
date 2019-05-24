using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ArchCipher {
    class Packer {

        public static string Filename = "+.pac";
        public static string[] Files = new string[99];
        private static List<string> _fs; // = new List<string>(files);

        public const int IntLength = int.MaxValue;
        private void Pack() {



            Console.WriteLine( "Creating Header..." );
            var headList = new string[_fs.Count];

            for (var i = 0; i < _fs.Count; i++) {
                var s = _fs[i];
                Console.WriteLine( "Processing Header for file:" + s );
                var f = new FileInfo( s );
                headList[i] = ( MakeList( new[] { f.Name, f.Length.ToString() } ) );
            }

            var header = MakeList( headList );
            var head = Encoding.UTF8.GetBytes( header.Length + header );
            Console.WriteLine();

            //contend
            var contend = new List<byte>();
            foreach (var s in _fs) {

                Console.WriteLine( "Processing contend of file:" + s );

                var tmp = File.ReadAllBytes( s );
                contend.AddRange( tmp );
            }
            var final = new byte[contend.Count + head.Length];

            Console.WriteLine( "Creating binery..." );

            var t = 0;
            for (var i = t; i < head.Length; i++, t++) {
                final[t] = head[t];
            }

            for (var i = 0; i < contend.Count; i++, t++) {
                final[t] = contend[i];
            }

            //Console.WriteLine( Encoding.UTF8.GetString( final ) );
            Console.WriteLine( "Writing all to file" );
            File.WriteAllBytes( Filename, final );

            Console.WriteLine( "Cleaning..." );
            // ReSharper disable RedundantAssignment
            final = new byte[1];
            contend = new List<byte>();
            head = new byte[1];
            header = "";
            // ReSharper restore RedundantAssignment
        }
        private void Unpack() {
            var contend = new List<byte>( File.ReadAllBytes( Filename ) );

            Console.WriteLine( "reading header..." );

            var hlength = GetNextInt( Encoding.UTF8.GetString( contend.GetRange( 0, 30 ).ToArray() ) );

            var ascy = Encoding.UTF8.GetString( contend.GetRange( hlength.ToString().Length, hlength ).ToArray() );
            var elements = UnMakeList( ascy, out _, false );

            hlength += hlength.ToString().Length;

            Console.WriteLine( "Isolating contend..." );

            contend = contend.GetRange( hlength, contend.Count - hlength );
            // File.WriteAllBytes( "tmp", contend.ToArray() );

            var cn = 0;
            for (var i = 0; i < elements.Length; i++) {
                var e = elements[i];

                var elementTemp = UnMakeList( e, out _, false );
                Console.WriteLine( $"Extracting File: {elementTemp[0]}, Length: {elementTemp[1]}" );
                var itemlemgth = int.Parse( elementTemp[1] );

                File.WriteAllBytes( elementTemp[0], contend.GetRange( cn, itemlemgth ).ToArray() );
                cn += itemlemgth;
            }

            Console.WriteLine( "Cleaning..." );
            // ReSharper disable RedundantAssignment
            contend = new List<byte>();
            ascy = "";
            // ReSharper restore RedundantAssignment
        }

        public string ReverseSize(string dtn) {
            var result = "";
            var length = (short) dtn.Length;

            for (var i = length.ToString().Length; i < IntLength; i++) {
                result += "0";
            }

            result += length.ToString();

            result += dtn;
            return result;
        }

        // ReSharper disable once UnusedMember.Global
        public string MakeVailed(string size) {
            var result = "";

            for (var i = size.Length; i < IntLength; i++) {
                result += "0";
            }

            result += size;
            return result;
        }

        public static string MakeList(string[] dtn) {
            var returns = "";

            foreach (var s in dtn) {
                returns += s.Replace( "\\", "\\%" ).Replace( "\n", "\\/" ) + "\\=";
            }

            returns += "\\?";
            return "\\&" + dtn.Length + "\\$" + returns + "\n";
        }
        public static string[] UnMakeList(string dtn, out int length, bool writeOutput = true) {
            var counts = 0;
            var wo = writeOutput;
            var c = 0;

            try {

                if (dtn.Substring( c, 2 ) == "\\&")
                    counts++;
                else throw new Exception( "No valet String" );

                c += 2;

                var returns = new List<string>();

                var anzahl = GetNextInt( dtn.Substring( c, int.MaxValue.ToString().Length ) );
                c += anzahl.ToString().Length;

                if (dtn.Substring( c, 2 ) == "\\$")
                    counts++;
                else throw new Exception( "Error:!!!" );

                c += 2;

                for (var i = 0; i < anzahl; i++) {
                    var element = GetElementNext( dtn.Substring( c ), "\\=" );

                    if (wo)
                        Console.WriteLine( element );

                    returns.Add( element );
                    c += ( element.Length );

                    if (dtn.Substring( c, 2 ) == "\\=")
                        counts++;
                    else throw new Exception( "Error:!!!" );
                    c += ( 2 );
                }

                if (dtn.Substring( c, 2 ) == "\\?")
                    counts++;
                else throw new Exception( "Error:!!!" );
                c += 2;

                if (dtn.Substring( c, 1 ) == "\n")
                    counts++;
                else throw new Exception( "Error:!!!" );
                c += 1;

                var mx = anzahl + 4;
                if (wo)
                    Console.WriteLine( $"Finished:[{counts}/{mx}] " + ( ( counts == mx ) ? "Perfect!" : ( counts == mx - 1 ) ? "Good" : "We Do not get all =[" ) );

                //if (dtn.Substring( Il + 2 + length, 2 ) == "\n")
                //    Console.WriteLine( "vailet" );

                var ret = new string[returns.Count];
                for (var i = 0; i < ret.Length; i++) {
                    ret[i] = returns[i].Replace( "\\%", "\\" ).Replace( "\\/", "\n" );
                }

                length = c;
                return ret;
            }
            catch (Exception e) {
                if (wo)
                    Console.WriteLine( $"Error @[{counts}]: {e.Message}!" );
                length = c;
                return new string[] { };
            }
        }

        private static string GetElementNext(string dtn, string strEnd) {
            var length = -99;
            for (var i = 0; i < dtn.Length; i++) {
                if (dtn.Substring( i, 2 ) == strEnd) {
                    length = i; break;
                }
            }

            return dtn.Substring( 0, length );
        }

        private static int GetNextInt(string v) {
            int returns;
            var i = 1;
            for (; i < v.Length; i++)
                if (!int.TryParse( v.Substring( 0, i ), out returns )) break;
            int.TryParse( v.Substring( 0, i - 1 ), out returns );
            return returns;
        }
    }
}
