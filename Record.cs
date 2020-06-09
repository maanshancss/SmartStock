using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStock
{
    public class Record
    {
        public DateTime RecordTime { get; set; }
        public string price1 { get; set; }
        public string price2 { get; set; }
        public string price3 { get; set; }
        public string price4 { get; set; }
        public string num { get; set; }
        public string amount { get; set; }
        public string avgprice { get; set; }
    }
}