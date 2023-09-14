using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class func_mainData
    {
        public String m_StrSID;
        public String m_StrName;
        public bool m_blnEenabe;//啟用與否
        public func_mainData(String StrSID, String StrName, bool blnEenabe=false)
        {
            m_StrSID = StrSID;
            m_StrName = StrName;
            m_blnEenabe = blnEenabe;
        }
    }

    public class GTFMDatum
    {
        public string SID { get; set; }
        public string func_type { get; set; }
        public string parent_func_sid { get; set; }
        public string func_name { get; set; }
        public object content { get; set; }
        public int sort { get; set; }
        public string stop_flag { get; set; }
        public string stop_time { get; set; }
        public string del_flag { get; set; }
        public string del_time { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_terminal_func_main
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GTFMDatum> data { get; set; }
    }
}
