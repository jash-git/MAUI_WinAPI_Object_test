using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GSADDatum
    {
        public int company_sid { get; set; }
        public int set_sid { get; set; }
        public int attribute_sid { get; set; }
        public string attribute_name { get; set; }
        public string attribute_main_price_type { get; set; }
        public string attribute_main_price { get; set; }
        public string attribute_main_max_price { get; set; }
        public string attribute_price_type { get; set; }
        public string attribute_price { get; set; }
        public string attribute_max_price { get; set; }
        public string required_flag { get; set; }
        public int attribute_count { get; set; }
        public string repeat_flag { get; set; }
        public int sort { get; set; }
        public int created_unix_time { get; set; }
        public int updated_unix_time { get; set; }
        public List<GSADProductSetRelation> product_set_relation { get; set; }
    }

    public class GSADProductSetRelation
    {
        public int set_sid { get; set; }
        public int attribute_sid { get; set; }
        public int category_sid { get; set; }
        public int product_sid { get; set; }
        public string main_flag { get; set; }
        public string default_flag { get; set; }
    }

    public class get_set_attribute_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GSADDatum> data { get; set; }
    }

}
