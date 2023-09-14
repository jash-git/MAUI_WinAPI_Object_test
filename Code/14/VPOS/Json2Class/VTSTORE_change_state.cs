using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class VTSTORE_change_state
    {
        public string status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
    }

    public class VTSTORE_change_input
    {
        public string receiver_id { get; set; }//接收設備ID 
        public long store_completion_time { get; set; }//店家回覆訂單可完成時間 
        public string reason { get; set; }//店家回覆婉拒原因 
    }
}
