using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GTRFuncRelation
    {
        public string func_sid { get; set; }
    }
    public class GTRDatum
    {
        public int SID { get; set; }
        public int company_sid { get; set; }
        public string role_name { get; set; }
        public string del_flag { get; set; }
        public string del_time { get; set; }
        public DateTime created_time { get; set; }
        public int created_unix_time { get; set; }
        public DateTime updated_time { get; set; }
        public int updated_unix_time { get; set; }
        public List<GTRFuncRelation> func_relation { get; set; }
    }

    public class get_terminal_roles
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GTRDatum> data { get; set; }
    }

}
