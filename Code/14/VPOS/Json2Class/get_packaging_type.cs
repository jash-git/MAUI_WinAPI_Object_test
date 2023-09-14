using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GPTDatum1
    {
        public int SID { get; set; }
        public int company_sid { get; set; }
        public string name { get; set; }
        public int sort { get; set; }
        public string show_flag { get; set; }
        public string required_flag { get; set; }
        public string del_flag { get; set; }
        public int del_time { get; set; }
        public int del_unix_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_packaging_type
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GPTDatum1> data { get; set; }
    }

}
