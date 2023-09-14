using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace VPOS
{
    public class LinePayModuleParams
    {
        public string env_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }

    //---
    //付款請求 [LinePayRequest]
    /*
    {
	    "orderId": "",
	    "productName":"VTEAM",
	    "amount": 10,
	    "currency": "TWD",
	    "oneTimeKey": ""
    }

    {
	    "returnCode": "9999",
	    "returnMessage": "Init LinePay Info Fail =>Channel_Id or Channed_Secret is invalid.",
	    "info":
	    {
		    "transactionId":"",
		    "paymentAccessToken":""
	    }
    }

    {
	    "returnCode": "0000",
	    "returnMessage": "Success.",
	    "info": {
		    "transactionId": 2023022200750020810,
		    "paymentAccessToken": "830142658095"
	    },
	    "AccessIP": "R:[203.69.151.102] ",
	    "paymentSID": "0ACE75A0B27811ED898DA753A39D19B5",
	    "channelId": "1656001986"
    }
    */
    public class LinePayRequestIn
    {
        public string orderId { get; set; }
        public string productName { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string oneTimeKey { get; set; }
    }
    public class LPROInfo
    {
        public object transactionId { get; set; }
        public string paymentAccessToken { get; set; }
    }

    public class LinePayRequestOut
    {
        public string returnCode { get; set; }
        public string returnMessage { get; set; }
        public LPROInfo info { get; set; }
        public string AccessIP { get; set; }
        public string paymentSID { get; set; }
        public string channelId { get; set; }
        public LinePayRequestOut() 
        {
            returnCode = "network";
            returnMessage = "";
            info = new LPROInfo();
            AccessIP = "";
            paymentSID = "";
            channelId = "";
        }
    }
    //---付款請求

    //---
    //確認交易結果 [LinePayInfo]
    /*
    {
        "channelId":"",
        "transactionId":""
    }

    {
        "returnCode": "9999",
        "returnMessage": "Init LinePay Info Fail =>Channel_Id or Channed_Secret is invalid.",
        "paymentStep":"",
        "info":
        {
            "transactionId":"",
            "orderId":"",
            "payInfo":[]
        }
    } 
    */
    /*
    {
     "returnCode":"0000",
     "returnMessage":"成功",
     "info":{
      "transactionId":"202306198850434xxxx",
      "orderId":"VTPOSA0100060-2023061xxxxx",
      "payInfo":[
       {
        "method":"CREDIT_CARD",
        "amount":35,
        "maskedCreditCardNumber":"************2344"
       }
      ],
      "merchantReference":{
        "affiliateCards":[
          {
           "cardType":"MOBILE_CARRIER",
           "cardId":"\/3WMF7CFG"
          }
        ]
      }
     }
    }
    */
    public class LPIOMerchantReference//電子發票載具
    {
        public List<LPIOAffiliateCard> affiliateCards { get; set; }
    }

    public class LPIOAffiliateCard
    {
        public string cardType { get; set; }
        public string cardId { get; set; }//電子發票載具
    }

    public class LinePayInfoIn
    {
        public string channelId { get; set; }
        public string transactionId { get; set; }
    }

    public class LPIOPayInfo
    {
        public string method { get; set; }
        public long amount { get; set; }
    }

    public class LPIOInfo
    {
        public object transactionId { get; set; }
        public string orderId { get; set; }
        public List<LPIOPayInfo> payInfo { get; set; }
        public LPIOMerchantReference merchantReference { get; set; }

        public LPIOInfo()
        {
            transactionId = "";
            orderId = "";
            payInfo = new List<LPIOPayInfo>();
            merchantReference= new LPIOMerchantReference();//電子發票載具
        }
    }

    public class LinePayInfoOut
    {
        public string returnCode { get; set; }
        public string returnMessage { get; set; }
        public LPIOInfo info { get; set; }
        public string paymentSID { get; set; }
        public string channelId { get; set; }
        public string AccessIP { get; set; }
        public string paymentStep { get; set; }
        

        public LinePayInfoOut()
        {
            returnCode = "network";
            returnMessage = "";
            info = new LPIOInfo();
            paymentSID = "";
            channelId = "";
            AccessIP = "";
            paymentStep = "";
        }
    }
    //---確認交易結果

    //---
    //退款 [LinePayRefund]
    /*
    {
	    "channelId":"",
	    "transactionId":""
    }

    {
	    "returnCode": "9999",
	    "returnMessage": "Init LinePay Info Fail =>Channel_Id or Channed_Secret is invalid.",
	    "info":
	    {
		    "refundTransactionId":"",
		    "refundTransactionDate":""
	    }
    }

    {
	    "returnCode": "0000",
	    "returnMessage": "Success.",
	    "info": {
		    "refundTransactionId": 2023030200810761611,
		    "refundTransactionDate": "2023-03-02T08:54:54Z"
	    },
	    "paymentSID": "565C6FC0B80D11EDAB0ED3671D9BF1BA",
	    "channelId": "1656001986",
	    "AccessIP": "R:[203.69.151.102] "
    }
    */
    public class LinePayRefundIn
    {
        public string channelId { get; set; }
        public string transactionId { get; set; }
    }

    public class LPROInfo2
    {
        public object refundTransactionId { get; set; }
        public object refundTransactionDate { get; set; }

    }

    public class LinePayRefundOut
    {
        public string returnCode { get; set; }
        public string returnMessage { get; set; }
        public LPROInfo2 info { get; set; }
        public string paymentSID { get; set; }
        public string channelId { get; set; }
        public string AccessIP { get; set; }

        public LinePayRefundOut()
        {
            returnCode = "network";
            returnMessage = "";
            info= new LPROInfo2();
            paymentSID = "";
            channelId = "";
            AccessIP = "";
        }
    }
    //---退款

    //---
    //付款確認 [測試環境使用] [LinePayConfirm]
    /*
    {
	    "channelId":"",
	    "transactionId":""
    }

    {
	    "returnCode": "9999",
	    "returnMessage": "Init LinePay Info Fail =>Channel_Id or Channed_Secret is invalid."
    }
    */
    public class LinePayConfirmIn
    {
        public string channelId { get; set; }
        public string transactionId { get; set; }
    }
    public class LinePayConfirmOut
    {
        public string returnCode { get; set; }
        public string returnMessage { get; set; }
        public LinePayConfirmOut()
        {
            returnCode = "network";
            returnMessage = "";
        }
    }
    //---付款確認 [測試環境使用]
}
