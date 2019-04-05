using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLSave
{
    public class config
    {
        private bool _startconsol;
        public bool startconsol
        {
            get { return _startconsol; }
            set { _startconsol = value; }
        }

    }
}
