using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GPCDatum
    {
        public int category_sid { get; set; }
        public int company_sid { get; set; }
        public string category_code { get; set; }
        public string category_name { get; set; }
        public int sort { get; set; }
        public string display_flag { get; set; }//商品類別依據設定[是否顯示]，決定POS端是否出現該類別
        public string stop_flag { get; set; }
        public string stop_time { get; set; }
        public int stop_unix_time { get; set; }
        public string del_flag { get; set; }
        public string del_time { get; set; }
        public int del_unix_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
        public List<GPCProductRelationDatum> product_relation_data { get; set; }
    }

    public class GPCProductRelationDatum
    {
        public int product_sid { get; set; }
        public string product_code { get; set; }
    }

    public class get_products_category
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GPCDatum> data { get; set; }
    }

}
