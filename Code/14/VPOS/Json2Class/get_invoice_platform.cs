using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GIPDatum
    {
        public int SID { get; set; }
        public string platform_name { get; set; }
        public string inv_url_1 { get; set; }
        public string inv_url_2 { get; set; }
        public string inv_test_url_1 { get; set; }
        public string inv_test_url_2 { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_invoice_platform
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GIPDatum> data { get; set; }
    }
}
