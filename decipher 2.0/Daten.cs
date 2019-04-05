using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLSave
{
    public class Daten
    {
        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _passwd;
        public string passwd
        {
            get { return _passwd; }
            set { _passwd = value; }
        }
        private string _execute;
        public string execute
        {
            get { return _execute; }
            set { _execute = value; }
        }
        private string _argus;
        public string argus
        {
            get { return _argus; }
            set { _argus = value; }
        }


    }
}
