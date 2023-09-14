using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GUDDatum
    {
        public int user_sid { get; set; }
        public int company_sid { get; set; }
        public int role_sid { get; set; }
        public string user_account { get; set; }
        public string user_pwd { get; set; }
        public string user_name { get; set; }
        public string employee_no { get; set; }
        public string job_title { get; set; }
        public string tel { get; set; }
        public string cellphone { get; set; }
        public string state_flag { get; set; }
        public int state_unix_time { get; set; }
        public string state_time { get; set; }
        public string del_flag { get; set; }
        public string del_time { get; set; }
        public int del_unix_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_user_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GUDDatum> data { get; set; }
    }

}
