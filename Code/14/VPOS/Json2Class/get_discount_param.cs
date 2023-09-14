using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GDPProductRelation//discount_product_relation 資料表的資料結構
    {
        public int product_sid { get; set; }
        public string product_code { get; set; }
    }

    public class GDPDatum
    {
        public int param_sid { get; set; }
        public string discount_code { get; set; }
        public string filter_type { get; set; }
        public string round_calc_type { get; set; }
        public string del_flag { get; set; }
        public int del_time { get; set; }
        public int del_unix_time { get; set; }
        public int created_unix_time { get; set; }
        public int updated_unix_time { get; set; }

        public List<GDPProductRelation> product_relation { get; set; }
    }

    public class get_discount_param
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GDPDatum> data { get; set; }
    }
}
