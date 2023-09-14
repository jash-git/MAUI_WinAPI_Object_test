using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GPT2Datum
    {
        public string template_sid { get; set; }
        public string template_type { get; set; }
        public string template_name { get; set; }
        public string template_value { get; set; }
        public string include_command { get; set; }
        public string scope { get; set; }
        public string is_default { get; set; }
        public string del_flag { get; set; }
        public int del_unix_time { get; set; }
        public int created_unix_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_printer_template
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GPT2Datum> data { get; set; }
    }
}
