using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GSTDDatum
    {
        public int table_sid { get; set; }
        public int company_sid { get; set; }
        public string table_code { get; set; }
        public string table_name { get; set; }
        public int table_capacity { get; set; }
        public int table_sort { get; set; }
        public string stop_flag { get; set; }
        public string stop_time { get; set; }
        public int stop_unix_time { get; set; }
        public string del_flag { get; set; }
        public string del_time { get; set; }
        public int del_unix_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_store_table_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GSTDDatum> data { get; set; }
    }

}
