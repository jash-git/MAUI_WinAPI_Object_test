using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{

    public class GTEPDatum
    {
        public string key { get; set; }
        public Object value { get; set; }// 由於 該欄位存放資料已經無法找到對應規則，因此從本來的特定類別修該成Object藉此避開資料分析的動作 public GTEPValue value { get; set; }
    }

    public class get_terminal_env_params
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GTEPDatum> data { get; set; }
    }

    /*原始資料類別 現在直接被Object取代
    public class GTEPValue
    {
        public string PAYMENT_API_URL { get; set; }
        public string VDES_API_URL { get; set; }
        public int? num_len { get; set; }
        public string num_mode { get; set; }
        public int? num_start { get; set; }
        public int? num_end { get; set; }
        public string reset_mode { get; set; }
    }
    */
}
