using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    //標籤40mm 60mm 列印相關資料結構
    public class pmData
    {
        public string product_code { get; set; }
        public string memo { get; set; }

        public pmData()
        {
            product_code = "";
            memo = "";
        }
    }
    public class product_memo
    {
        public List<pmData> data { get; set; }

        public product_memo()
        {
            data = new List<pmData>();
        }
    }
}
