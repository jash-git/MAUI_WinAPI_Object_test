using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    /*
    SELECT TP1.SID,TP1.platform_name,TP2.active_state,TP2.params FROM takeaways_platform AS TP1 INNER JOIN takeaways_params AS TP2 
    ON TP2.active_state='Y' 
    WHERE TP1.SID=TP2.platform_sid
    */
    public class VTSTORE_params//點點食
    {
        public string terminal_sid { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string product_convert_to_pos_name { get; set; }
        public string condiment_convert_to_pos_name { get; set; }
}

    public class NIDIN_POS_params//你訂
    {
        public object terminal_sid { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public object api_url { get; set; }
        public string product_convert_to_pos_name { get; set; }
        public string condiment_convert_to_pos_name { get; set; }
    }

    public class UBER_EATS_params//吳柏毅
    {
        public string terminal_sid { get; set; }
        public string store_no { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string product_convert_to_pos_name { get; set; }
        public string condiment_convert_to_pos_name { get; set; }
    }

    public class FOODPANDA_params//熊貓
    {
        public string terminal_sid { get; set; }
        public string store_no { get; set; }
        public string product_convert_to_pos_name { get; set; }
        public string condiment_convert_to_pos_name { get; set; }
    }

    public class YORES_POS_params//享什麼
    {
        public string env_type { get; set; }
        public string posid { get; set; }
        public string terminal_sid { get; set; }
        public string product_convert_to_pos_name { get; set; }
        public string condiment_convert_to_pos_name { get; set; }
    }
}
