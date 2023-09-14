using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GDHDatum
    {
        public int hotkey_sid { get; set; }
        public string hotkey_name { get; set; }
        public string hotkey_code { get; set; }
        public string discount_code { get; set; }
        public string val_mode { get; set; }
        public int val { get; set; }
        public string round_calc_type { get; set; }
        public string stop_flag { get; set; }
        public int stop_time { get; set; }
        public int stop_unix_time { get; set; }
        public string del_flag { get; set; }
        public int del_time { get; set; }
        public int del_unix_time { get; set; }
        public int sort { get; set; }
        public int created_unix_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_discount_hotkey
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GDHDatum> data { get; set; }
    }

}
