using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GPSDDatum
    {
        public int company_sid { get; set; }
        public int spec_sid { get; set; }
        public string spec_name { get; set; }
        public int init_product_sid { get; set; }
        public string del_flag { get; set; }
        public string del_time { get; set; }
        public int del_unix_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
        public List<GPSDSpecRelationDatum> spec_relation_data { get; set; }
    }

    public class GPSDSpecRelationDatum
    {
        public int product_sid { get; set; }
        public string alias_name { get; set; }
        public int sort { get; set; }
    }

    public class get_product_spec_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GPSDDatum> data { get; set; }
    }

}
