using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings
{
    public class PureDataSettings
    {
        public bool commandBlock = false;

        public string singleNumber = string.Empty;
        public string[] messages = new string[0];

        public int androidCompatibility = 0;
        public int numbersExtractionRegion = 0;

        public int betweenMessagesDelay = 2000;
        public int betweenNumbersDelay = 1000;
        public int maxMessagesSafeLock = 10000;

        public string data = string.Empty;
    }
}
