using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GPMPDatum
    {
        public int SID { get; set; }
        public string payment_module_code { get; set; }
        public string @params { get; set; }
        public string pub_params { get; set; }
        public string del_flag { get; set; }
        public long del_unix_time { get; set; }
        public string stop_flag { get; set; }
        public long stop_unix_time { get; set; }
        public int sort { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_payment_module_params
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GPMPDatum> data { get; set; }
    }
}
