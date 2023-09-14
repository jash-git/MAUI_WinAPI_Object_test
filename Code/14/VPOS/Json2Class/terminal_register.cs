using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class TRDatum
    {
        public int company_sid { get; set; }
        public string terminal_sid { get; set; }
        public string terminal_name { get; set; }
        public string license_type { get; set; }
        public string reg_state { get; set; }
        public string reg_submit_time { get; set; }
        public int reg_submit_unix_time { get; set; }
        public object reg_accept_time { get; set; }
        public object reg_accept_unix_time { get; set; }
        public string api_token { get; set; }
    }

    public class terminal_register
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<TRDatum> data { get; set; }
    }
}
