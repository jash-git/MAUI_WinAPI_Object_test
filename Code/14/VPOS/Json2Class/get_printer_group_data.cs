using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GPGDDatum
    {
        public int company_sid { get; set; }
        public int printer_group_sid { get; set; }
        public string printer_group_name { get; set; }
        public int printer_sid { get; set; }
        public int order_type_sid { get; set; }
        public string filter_type { get; set; }
        public string stop_flag { get; set; }
        public int stop_time { get; set; }
        public int stop_unix_time { get; set; }
        public string del_flag { get; set; }
        public int del_time { get; set; }
        public int del_unix_time { get; set; }
        public int created_unix_time { get; set; }
        public int updated_unix_time { get; set; }
        public List<GPGDRelationDatum> relation_data { get; set; }
        public List<GPGDOrderTypeRelation> order_type_relation { get; set; }
    }

    public class GPGDOrderTypeRelation
    {
        public int order_type_sid { get; set; }
    }

    public class GPGDRelationDatum
    {
        public int printer_group_sid { get; set; }
        public int product_sid { get; set; }
    }

    public class get_printer_group_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GPGDDatum> data { get; set; }
    }

}
