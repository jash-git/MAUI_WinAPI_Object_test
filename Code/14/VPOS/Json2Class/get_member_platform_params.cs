using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GMPPDatum
    {
        public int SID { get; set; }
        public int company_sid { get; set; }
        public string platform_type { get; set; }
        public string @params { get; set; }
        public int sort { get; set; }
        public string stop_flag { get; set; }
        public int stop_unix_time { get; set; }
        public string del_flag { get; set; }
        public int del_unix_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_member_platform_params
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GMPPDatum> data { get; set; }
    }
}
