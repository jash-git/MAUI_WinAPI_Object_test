using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GADDatum
    {
        public int SID { get; set; }
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string type { get; set; }
        public string sort { get; set; }
        public string stop_flag { get; set; }
        public int stop_unix_time { get; set; }
        public string del_flag { get; set; }
        public int del_unix_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_account_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GADDatum> data { get; set; }
    }

}
