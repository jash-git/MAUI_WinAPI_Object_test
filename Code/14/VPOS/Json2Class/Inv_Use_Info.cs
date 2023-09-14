using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class Inv_Use_Info
    {
        public string Business_Id { get; set; }//營業人統一編號
        public string Period { get; set; }//發票年月(期別)，Ex. 202302
        public string Track { get; set; }//發票字軌，兩碼英文字大寫
        public string Begin_No { get; set; }//發票起始編號
        public string End_No { get; set; }//發票結束編號
        public long Get_Time { get; set; }//發票字軌取回時間(UnixTime)
        public object Total_Count { get; set; }//發票張數
        public object Remaining_Count { get; set; }//剩餘張數
        public object Total_Remaining_Count { get; set; }//總共剩餘未使用發票張數，包含其他捲
        public object Last_Use_No { get; set; }//目前已使用發票號碼
        public object Batch_Num { get; set; }//發票開立批號
        public string Random_Code { get; set; }//發票隨機碼
        public string Ret_Code { get; set; }//執行代碼
        public string Ret_Msg { get; set; }//執行訊息
    }
}
