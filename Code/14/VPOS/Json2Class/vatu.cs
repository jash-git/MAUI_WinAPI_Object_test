using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class VautDatum
    {
        public string deploy_sid { get; set; }
        public string data_module { get; set; }
        public string data_type { get; set; }
        public string file_name { get; set; }
        public string version { get; set; }
        public string current_version { get; set; }
        public VautFileInfo file_info { get; set; }
        public int deploy_time { get; set; }
    }
    public class VautFileInfo
    {
        public string extension { get; set; }
        public string origin_file_name { get; set; }
        public string download_url { get; set; }
    }
    public class Vaut
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<VautDatum> data { get; set; }
    }
}
