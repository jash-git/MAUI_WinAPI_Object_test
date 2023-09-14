using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace VPOS
{
    //---
    //信用卡回傳結果JSON物件
    public class ECR_Config
    {
        public String Comport { get; set; }//3
        public String BaudRate { get; set; }//9600
        public String Format { get; set; }//8NS1
        public String TimeOut { get; set; }//90sec
        public String RetriesCount { get; set; }//3

        public ECR_Config(String StrComport)
        {
            Comport = StrComport;//3
            BaudRate = "9600";
            Format = "8NS1";
            TimeOut = "90";
            RetriesCount = "3";
        }
    }
    public class CreditCardJosn
    {
        public String Print_Receipt { get; set; }//紀錄小額交易是否列印持卡人存根
        public String Device_Titel { get; set; }//聯合信用卡小額交易持卡人存根
        public String ECR_Response_Code { get; set; }
        public String ECR_Response_Msg { get; set; }
        public String ECR_Indicator { get; set; }
        public String ECR_Version_Date { get; set; }
        public String Trans_Type_Indicator { get; set; }
        public String Trans_Type { get; set; }
        public String CUP_SP_ESVC_Indicator { get; set; }
        public String Host_ID { get; set; }
        public String Receipt_No { get; set; }
        public String Card_No { get; set; }
        public String Card_Expire_Date { get; set; }
        public String Trans_Amount { get; set; }
        public String Trans_Date { get; set; }
        public String Trans_Time { get; set; }
        public String Approval_No { get; set; }
        public String Wave_Card_Indicator { get; set; }
        public String Merchant_ID { get; set; }
        public String Terminal_ID { get; set; }
        public String Exp_Amount { get; set; }
        public String Store_Id { get; set; }
        public String Installment_Redeem_Indicator { get; set; }
        public String RDM_Paid_Amt { get; set; }
        public String RDM_Point { get; set; }
        public String Points_of_Balance { get; set; }
        public String Redeem_Amt { get; set; }
        public String Installment_Period { get; set; }
        public String Down_Payment_Amount { get; set; }
        public String Installment_Payment_Amount { get; set; }
        public String Formality_Fee { get; set; }
        public String Card_Type { get; set; }
        public String Batch_No { get; set; }
        public String Start_Trans_Type { get; set; }
        public String MP_Flag { get; set; }
        public String SP_ISSUE_ID { get; set; }
        public String ESVC_Origin_Date { get; set; }
        public String SP_Origin_RRN { get; set; }
        public String Pay_Item { get; set; }
        public String Card_No_Hash_Value { get; set; }
        public String MP_Response_Code { get; set; }
        public String ASM_Award_flag { get; set; }
        public String MCP_Indicator { get; set; }
        public String Bank_Code { get; set; }
        public String Reserved { get; set; }
        public String HG_Data { get; set; }
        public ECR_Config ECR_Config { get; set; }

        public CreditCardJosn()
        {
            ECR_Config=new ECR_Config("");
        }
    }
    //---信用卡回傳結果JSON物件
 }
