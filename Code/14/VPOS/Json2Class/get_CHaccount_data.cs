using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class CHaccount_data
    {
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string account_type { get; set; }
        public string payment_code { get; set; }
        public string payment_name { get; set; }
        public int money { get; set; }

        public CHaccount_data()
        {
            account_code = "";
            account_name = "";
            account_type = "";
            payment_code = "";
            payment_name = "";
            money = 0;
        }
    }

    public class get_CHaccount_data
    {
        public List<CHaccount_data> data { get; set; }
        public get_CHaccount_data()
        {
            data = new List<CHaccount_data>();
        }
    }

}
