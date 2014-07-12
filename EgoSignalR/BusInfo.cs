using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EgoSignalR
{
    public class BusInfo
    {
        public int LineNumber { get; set; }
        public int StopNo { get; set; }

        public BusInfo(int lineNo, int stopNo)
        {
            this.LineNumber = lineNo;
            this.StopNo = StopNo;
        }
        public BusInfo()
        {

        }
    }
}
