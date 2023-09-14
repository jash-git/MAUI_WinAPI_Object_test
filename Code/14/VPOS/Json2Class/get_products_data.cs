using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GPDCondimentRelation
    {
        public int condiment_sid { get; set; }
        public string condiment_code { get; set; }
    }

    public class GPDPriceTypeRelation
    {
        public int price_type_sid { get; set; }
        public object price { get; set; }
    }
    public class GPDDatum
    {
        public int product_sid { get; set; }
        public int company_sid { get; set; }
        public string product_code { get; set; }
        public string product_barcode { get; set; }
        public string product_type { get; set; }
        public string product_name { get; set; }
        public string product_shortname { get; set; }
        public object price_mode { get; set; }
        public string product_price { get; set; }
        public int unit_sid { get; set; }
        public int tax_sid { get; set; }
        public int sort { get; set; }
        public string display_flag { get; set; }//商品資料依據設定[是否顯示]，決定POS端是否出現
        public string memo { get; set; }
        public string stop_flag { get; set; }
        public string del_flag { get; set; }
        public string stop_time { get; set; }
        public string del_time { get; set; }
        public string condiment_update_time { get; set; }
        public string category_update_time { get; set; }
        public object price_update_time { get; set; }
        public string created_time { get; set; }
        public string updated_time { get; set; }
        public int stop_unix_time { get; set; }
        public int del_unix_time { get; set; }
        public int condiment_update_unix_time { get; set; }
        public int category_update_unix_time { get; set; }
        public int price_update_unix_time { get; set; }
        public int created_unix_time { get; set; }
        public int updated_unix_time { get; set; }
        public List<GPDCondimentRelation> condiment_relation { get; set; }
        public List<GPDPriceTypeRelation> price_type_relation { get; set; }
    }

    public class get_products_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GPDDatum> data { get; set; }
    }

}
