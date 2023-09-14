using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class CHpayment_data
    {
        public int payment_sid { get; set; }
        public string payment_code { get; set; }
        public string payment_name { get; set; }
        public int payment_amount { get; set; }
        public int total_count { get; set; }

        public CHpayment_data()
        {
            payment_sid=0;
            payment_code = "";
            payment_name = "";
            payment_amount = 0;
            total_count = 0;
        }
    }

    public class get_CHpayment_data
    {
        public List<CHpayment_data> data { get; set; }
        public get_CHpayment_data()
        {
            data = new List<CHpayment_data>();
        }
    }

}
