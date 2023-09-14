using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GCDDatum1
    {
        public int condiment_sid { get; set; }
        public int company_sid { get; set; }
        public string condiment_code { get; set; }
        public string condiment_name { get; set; }
        public string condiment_price { get; set; }
        public int unit_sid { get; set; }
        public int group_sid { get; set; }
        public int sort { get; set; }
        public string stop_flag { get; set; }
        //沒有使用- public int stop_time { get; set; }
        public int stop_unix_time { get; set; }
        public string del_flag { get; set; }
        public string del_time { get; set; }
        public int del_unix_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_condiment_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GCDDatum1> data { get; set; }
    }

}
