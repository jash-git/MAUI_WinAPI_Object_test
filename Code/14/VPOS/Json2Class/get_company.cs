using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    /*
    {
        "status": "ACCEPTED",
        "message": "",
        "data": [
            {
                "company_sid": 7,
                "company_no": "vtshop",
                "company_ein": "28537502",
                "business_name": "VTSHOP企業",
                "name": "VTEAM-茶飲店",
                "shortname": "VTEAM測試店家",
                "owner": "VTEAM",
                "tel": "04,22230090,11",
                "fax": "05,2223333332,22",
                "zip_code": "403",
                "country_code": "tw",
                "country_name": "台灣",
                "province_code": "",
                "province_name": null,
                "city_code": null,
                "city_name": null,
                "district_code": null,
                "district_name": null,
                "address": "自由路一段101號22樓206室",
                "def_order_type": 3,
                "def_unit_sid": 6,
                "def_tax_sid": 3,
                "def_tax_name": "TX",
                "def_tax_rate": "5.00",
                "def_tax_type": "A",
                "vtstore_order_url": "",
                "take_service_flag": "N",
                "take_service_type": "P",
                "take_service_val": "10.00"
            }
        ]
    }
     */
    public class GCDatum
    {
        public int company_sid { get; set; }
        public string company_no { get; set; }
        public string company_ein { get; set; }
        public string business_name { get; set; }
        public string name { get; set; }
        public string shortname { get; set; }
        public string owner { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string zip_code { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string province_code { get; set; }
        public object province_name { get; set; }
        public object city_code { get; set; }
        public object city_name { get; set; }
        public object district_code { get; set; }
        public object district_name { get; set; }
        public string address { get; set; }
        public int def_order_type { get; set; }
        public int def_unit_sid { get; set; }
        public int def_tax_sid { get; set; }
        public string def_tax_name { get; set; }
        public string def_tax_rate { get; set; }
        public string def_tax_type { get; set; }
        public string vtstore_order_url { get; set; }
        public string take_service_flag { get; set; }
        public string take_service_type { get; set; }
        public string take_service_val { get; set; }
        public string def_params { get; set; }
    }

    public class get_company
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GCDatum> data { get; set; }
    }
}
