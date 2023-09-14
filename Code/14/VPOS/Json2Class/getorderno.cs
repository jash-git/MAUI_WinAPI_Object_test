using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    //透過WEB API 取號 對應解析類別

    public class getorderno
    {
        public string orderno { get; set; }
        public string systime { get; set; }
    }

    public class getdailyno
    {
        public string dailyno { get; set; }
        public string systime { get; set; }
    }

    public class getclassno
    {
        public string classno { get; set; }
        public string systime { get; set; }
    }
}
