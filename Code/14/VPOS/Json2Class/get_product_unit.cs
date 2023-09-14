using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GPUDatum
    {
        public int product_unit_sid { get; set; }
        public int company_sid { get; set; }
        public string unit_name { get; set; }
        public int sort { get; set; }
        public string del_flag { get; set; }
        public string del_time { get; set; }
        public int del_unix_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_product_unit
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GPUDatum> data { get; set; }
    }
}
