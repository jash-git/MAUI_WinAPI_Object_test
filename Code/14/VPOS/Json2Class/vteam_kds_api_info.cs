using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    /*
    VTEAM_KDS_API_INFO
    {
        "active_state": "N",
        "api_url": "",
        "client_id": "",
        "client_secret": "",
        "include_formula_value": "N"
    } 
    */
    public class vteam_kds_api_info
    {
        public string active_state { get; set; }
        public string api_url { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string include_formula_value { get; set; }

        public vteam_kds_api_info()
        {
            active_state = "N";
            api_url = "";
            client_id = "";
            client_secret = "";
	        include_formula_value = "N";
        }
    }
}
