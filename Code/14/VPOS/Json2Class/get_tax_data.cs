using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GTDDatum
    {
        public int tax_sid { get; set; }
        public int company_sid { get; set; }
        public string tax_name { get; set; }
        public string tax_rate { get; set; }
        public string tax_type { get; set; }
        public string del_flag { get; set; }
        public string del_time { get; set; }
        public int del_unix_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_tax_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GTDDatum> data { get; set; }
    }
}
