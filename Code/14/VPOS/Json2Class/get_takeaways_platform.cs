using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GTPDatum
    {
        public string platform_sid { get; set; }
        public string platform_name { get; set; }
        public int created_unix_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_takeaways_platform
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GTPDatum> data { get; set; }
    }
}
