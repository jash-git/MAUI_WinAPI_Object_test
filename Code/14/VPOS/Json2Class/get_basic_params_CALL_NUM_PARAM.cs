using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    /*
	終端取餐號碼 {"num_len":3,"num_mode":"S","num_start":5,"num_end":99,"reset_mode":"D"}
		結帳後產生 - 使用時機
		num_mode":"S" - S:自訂模式;O:訂單號
		"num_len":3 - 取餐號長度
		"num_start":5,"num_end":99 - 起訖號設定
		"reset_mode":"D" - 自動歸0 D為天 ;N為不歸0 
    */
    public class get_basic_params_CALL_NUM_PARAM
    {
        public int num_len { get; set; }
        public string num_mode { get; set; }
        public int num_start { get; set; }
        public int num_end { get; set; }
        public string reset_mode { get; set; }
    }

}
