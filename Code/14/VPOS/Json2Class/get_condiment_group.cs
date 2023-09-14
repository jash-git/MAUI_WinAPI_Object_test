using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GCGDatum
    {
        public int group_sid { get; set; }
        public int company_sid { get; set; }
        public string group_name { get; set; }
        public string required_flag { get; set; }
        public string single_flag { get; set; }
        public string newline_flag { get; set; }
        public string count_flag { get; set; }
        public int min_count { get; set; }
        public int max_count { get; set; }
        public int sort { get; set; }
        public string del_flag { get; set; }
        public string del_time { get; set; }
        public int del_unix_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_condiment_group
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GCGDatum> data { get; set; }
    }

}
