using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GCIPDatum
    {
        public int company_sid { get; set; }
        public string company_no { get; set; }
        public string company_name { get; set; }
        public string inv_EIN { get; set; }
        public string EIN { get; set; }
        public int platform_sid { get; set; }
        public string env_type { get; set; }
        public string branch_no { get; set; }
        public string active_state { get; set; }
        public string qrcode_aes_key { get; set; }
        public string auth_account { get; set; }
        public string auth_password { get; set; }
        public string reg_id { get; set; }
        public int inv_renew_count { get; set; }
        public int booklet { get; set; }
        public string business_name { get; set; }
        public long created_unix_time { get; set; }
        public long updated_unix_time { get; set; }
    }

    public class get_company_invoice_params
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GCIPDatum> data { get; set; }
    }
}
