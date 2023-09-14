

namespace VPOS
{
    public class LinePayAPI
    {
        //---
        //API 函數區
        //付款請求
        private static bool PaymentsRequest(String StrOrderId,int intAmount,ref LinePayRequestOut refLinePayRequestOut)//付款請求
        {
            bool blnResult = false;
            if (!HttpsFun.WebRequestTest(ref HttpsFun.m_intNetworkLevel))//確認網路狀態
            {
                return blnResult;
            }

            if (VTEAMCloudAPI.Authentication())
            {
                String StrDomain = HttpsFun.setDomainMode(2);//vdes
                String[] StrHeaderName = new String[] { "Authorization", "envType", "channelId", "channelSecret" };
                String[] StrHeaderValue = new String[] { "Bearer " + VTEAMCloudAPI.m_Straccess_token,
                                                         (m_LinePayModuleParams.env_type=="T"? "sandbox":"production"),
                                                          m_LinePayModuleParams.client_id,
                                                          m_LinePayModuleParams.client_secret};
                LinePayRequestIn LinePayRequestInBuf = new LinePayRequestIn();
                LinePayRequestInBuf.orderId = StrOrderId;
                LinePayRequestInBuf.productName = "VTEAM";
                LinePayRequestInBuf.amount = intAmount;
                LinePayRequestInBuf.currency = "TWD";
                LinePayRequestInBuf.oneTimeKey = m_StrPayCode;
                String StrLinePayRequestIn = JsonClassConvert.LinePayRequestIn2String(LinePayRequestInBuf);

                String StrResult = HttpsFun.RESTfulAPI_postBody(StrDomain,"/api/line_pay/payments/request", StrLinePayRequestIn, StrHeaderName, StrHeaderValue);
                m_intStep = 1;//修改狀態紀錄

                refLinePayRequestOut = JsonClassConvert.LinePayRequestOut2Class(StrResult);
                if ((refLinePayRequestOut != null) && (refLinePayRequestOut.returnCode=="0000"))
                {
                    blnResult = true;
                }
                else if(refLinePayRequestOut != null)
                {
                    blnResult = false;
                }
                else
                {
                    blnResult = false;
                    refLinePayRequestOut=new LinePayRequestOut();
                    refLinePayRequestOut.returnCode = "";
                }

            }
            return blnResult;
        }

        //付款確認
        private static bool PaymentsConfirm(String StrChannelId, String StrtransactionId)//付款確認 [測試模式下使用]
        {
            bool blnResult = false;
            if (!HttpsFun.WebRequestTest(ref HttpsFun.m_intNetworkLevel))//確認網路狀態
            {
                return blnResult;
            }

            if (VTEAMCloudAPI.Authentication())
            {
                String StrDomain = HttpsFun.setDomainMode(2);//vdes
                LinePayConfirmIn LinePayConfirmInBuf = new LinePayConfirmIn();
                LinePayConfirmInBuf.channelId = StrChannelId;
                LinePayConfirmInBuf.transactionId = StrtransactionId;
                String StrLinePayConfirmIn = JsonClassConvert.LinePayConfirmIn2String(LinePayConfirmInBuf);

                String StrResult = HttpsFun.RESTfulAPI_postBody(StrDomain, "/api/line_pay/payments/confirm", StrLinePayConfirmIn, "Authorization", "Bearer " + VTEAMCloudAPI.m_Straccess_token);
                
                LinePayConfirmOut LinePayConfirmOutBuf = JsonClassConvert.LinePayConfirmOut2Class(StrResult);            
                if( (LinePayConfirmOutBuf!=null) && (LinePayConfirmOutBuf.returnCode == "0000"))
                {
                    blnResult = true;
                }
                else
                {
                    blnResult = false;
                }

            }
            return blnResult;
        }

        //確認交易結果
        private static bool PaymentsInfo(String StrChannelId, String StrtransactionId,ref LinePayInfoOut refLinePayInfoOut)
        {
            bool blnResult = false;
            if (!HttpsFun.WebRequestTest(ref HttpsFun.m_intNetworkLevel))//確認網路狀態
            {
                return blnResult;

            }

            if (VTEAMCloudAPI.Authentication())
            {
                String StrDomain = HttpsFun.setDomainMode(2);//vdes

                String StrOutput = HttpsFun.RESTfulAPI_get(StrDomain, $"/api/line_pay/payments/info/{StrChannelId}/{StrtransactionId}", "", "Authorization", "Bearer " + VTEAMCloudAPI.m_Straccess_token);
                m_intStep = 2;//修改狀態紀錄

                refLinePayInfoOut = JsonClassConvert.LinePayInfoOut2Class(StrOutput);
                if((refLinePayInfoOut != null) && (refLinePayInfoOut.returnCode == "0000"))
                {
                    blnResult = true;
                }
                else if(refLinePayInfoOut != null)
                {
                    blnResult = false;
                }
                else
                {
                    blnResult = false;
                    refLinePayInfoOut = new LinePayInfoOut();
                    refLinePayInfoOut.returnCode = "";
                }
            }
            return blnResult;
        }

        //退款
        public static bool Refund(String StrChannelId, String StrtransactionId, ref LinePayRefundOut refLinePayRefundOut)
        {
            bool blnResult = false;
            LinePayRefundIn LinePayRefundInBuf = new LinePayRefundIn();
            LinePayRefundInBuf.channelId = StrChannelId;
            LinePayRefundInBuf.transactionId = StrtransactionId;
            String StrLinePayRefundIn = JsonClassConvert.LinePayRefundIn2String(LinePayRefundInBuf);

            if (VTEAMCloudAPI.Authentication())
            {
                String StrDomain = HttpsFun.setDomainMode(2);//vdes
                /*
                String[] StrHeaderName = new String[] { "Authorization", "envType", "channelId", "channelSecret" };
                String[] StrHeaderValue = new String[] { "Bearer " + VTEAMCloudAPI.m_Straccess_token,
                                                         (m_LinePayModuleParams.env_type=="T"? "sandbox":"production"),
                                                          m_LinePayModuleParams.client_id,
                                                          m_LinePayModuleParams.client_secret};
                */

                String StrOutput = HttpsFun.RESTfulAPI_postBody(StrDomain, $"/api/line_pay/payments/refund", StrLinePayRefundIn, "Authorization", "Bearer " + VTEAMCloudAPI.m_Straccess_token);
                m_intStep = 1;//修改狀態紀錄

                refLinePayRefundOut = JsonClassConvert.LinePayRefundOut2Class(StrOutput);
                if ((refLinePayRefundOut != null) && (refLinePayRefundOut.returnCode == "0000"))
                {
                    blnResult = true;
                }
                else if (refLinePayRefundOut != null)
                {
                    blnResult = false;
                }
                else
                {
                    blnResult = false;
                    refLinePayRefundOut = new LinePayRefundOut();
                    refLinePayRefundOut.returnCode = "";
                }
            }

            return blnResult;
        }

        //代碼訊息對應查詢
        public static String PaymentCode2Info(String StrCode)
        {
            String StrResult = "";
            switch (StrCode)
            {
                case "0000":
                    StrResult = "成功";
                    break;
                case "1101":
                    StrResult = "買家不是 LINE Pay 會員";
                    break;
                case "1102":
                    StrResult = "買方被停止交易";
                    break;
                case "1104":
                    StrResult = "找不到商家";
                    break;
                case "1105":
                    StrResult = "此商家無法使用 LINE Pay";
                    break;
                case "1106":
                    StrResult = "標頭資訊錯誤";
                    break;
                case "1110":
                    StrResult = "無法使用的信用卡";
                    break;
                case "1124":
                    StrResult = "金額錯誤 (scale)";
                    break;
                case "1133":
                    StrResult = "非有效之 oneTimeKey";
                    break;
                case "1141":
                    StrResult = "付款帳戶狀態錯誤";
                    break;
                case "1142":
                    StrResult = "餘額不足";
                    break;
                case "1145":
                    StrResult = "正在進行付款";
                    break;
                case "1150":
                    StrResult = "找不到交易記錄";
                    break;
                case "1152":
                    StrResult = "已有既存付款";
                    break;
                case "1153":
                    StrResult = "付款reserve時的金額與申請的金額不一致";
                    break;
                case "1155":
                    StrResult = "交易編號不符合退款資格";
                    break;
                case "1159":
                    StrResult = "無付款申請資訊";
                    break;
                case "1163":
                    StrResult = "可退款日期已過無法退款";
                    break;
                case "1164":
                    StrResult = "超過退款額度";
                    break;
                case "1165":
                    StrResult = "交易已進行退款";
                    break;
                case "1169":
                    StrResult = "付款confirm所需要資訊錯誤（在LINE Pay）";
                    break;
                case "1170":
                    StrResult = "使用者帳戶的餘額有變動";
                    break;
                case "1172":
                    StrResult = "已有同一訂單編號的交易履歷";
                    break;
                case "1177":
                    StrResult = "超過允許擷取的交易數目 (100)";
                    break;
                case "1178":
                    StrResult = "不支援的貨幣";
                    break;
                case "1179":
                    StrResult = "無法處理狀態";
                    break;
                case "1183":
                    StrResult = "付款金額必須大於 0";
                    break;
                case "1184":
                    StrResult = "付款金額比付款申請時候的金額還大";
                    break;
                case "1198":
                    StrResult = "正在處理請求…";
                    break;
                case "1199":
                    StrResult = "內部請求錯誤";
                    break;
                case "1264":
                    StrResult = "一卡通MONEY相關錯誤";
                    break;
                case "1280":
                    StrResult = "信用卡付款時候發生了臨時錯誤";
                    break;
                case "1281":
                    StrResult = "信用卡付款錯誤";
                    break;
                case "1282":
                    StrResult = "信用卡授權錯誤";
                    break;
                case "1283":
                    StrResult = "因疑似詐騙，拒絕付款";
                    break;
                case "1284":
                    StrResult = "暫時無法以信用卡付款";
                    break;
                case "1285":
                    StrResult = "信用卡資訊不完整";
                    break;
                case "1286":
                    StrResult = "信用卡付款資訊不正確";
                    break;
                case "1287":
                    StrResult = "信用卡已過期";
                    break;
                case "1288":
                    StrResult = "信用卡的額度不足";
                    break;
                case "1289":
                    StrResult = "超過信用卡付款金額上限";
                    break;
                case "1290":
                    StrResult = "超過一次性付款的額度";
                    break;
                case "1291":
                    StrResult = "此信用卡已被掛失";
                    break;
                case "1292":
                    StrResult = "此信用卡已被停卡";
                    break;
                case "1293":
                    StrResult = "信用卡驗證碼 (CVN) 無效";
                    break;
                case "1294":
                    StrResult = "此信用卡已被列入黑名單";
                    break;
                case "1295":
                    StrResult = "信用卡號無效";
                    break;
                case "1296":
                    StrResult = "無效的金額";
                    break;
                case "1298":
                    StrResult = "信用卡付款遭拒";
                    break;
                case "1900":
                    StrResult = "暫時錯誤，請稍候再試";
                    break;
                case "1901":
                    StrResult = "暫時錯誤，請稍候再試";
                    break;
                case "1902":
                    StrResult = "暫時錯誤，請稍候再試";
                    break;
                case "1903":
                    StrResult = "暫時錯誤，請稍候再試";
                    break;
                case "1999":
                    StrResult = "嘗試呼叫的資訊與前一次的資訊不符";
                    break;
                case "2101":
                    StrResult = "參數錯誤";
                    break;
                case "2102":
                    StrResult = "JSON 資料格式錯誤";
                    break;
                case "2103":
                    StrResult = "錯誤請求。請確認 returnMesage";
                    break;
                case "2104":
                    StrResult = "錯誤請求。請確認 returnMesage";
                    break;
                case "9000":
                    StrResult = "內部錯誤";
                    break;
                case "network":
                    StrResult = "網路異常";
                    break;
                default:
                    StrResult = "其他未定義錯誤";
                    break;
            }
            return StrResult;
        }
        //---API 函數區

        //---
        //PaymentMain計算 相關全域變數
        public static LinePayModuleParams m_LinePayModuleParams = new LinePayModuleParams();//載入DB參數
        public static String m_StrPayCode = "";
        public static String m_StrOrderId = "";
        public static int m_intAmount = 0;

        public static LinePayRequestOut m_LinePayRequestOut = new LinePayRequestOut();//暫存支付請求結果變數
        public static LinePayInfoOut m_LinePayInfoOut = new LinePayInfoOut();//暫存確認交易結果變數
        public static int m_intStep = 0;
        public static String m_StrOutputInfo = "";
        public static bool m_blnPaySuccess = false;
        //---PaymentMain計算 相關全域變數

        public static void Payments(object arg)//等待LinePay回應
        {
            m_LinePayRequestOut = new LinePayRequestOut();//暫存支付請求結果變數
            m_LinePayInfoOut = new LinePayInfoOut();//暫存確認交易結果變數
            m_intStep = 0;
            m_StrOutputInfo = "";
            m_blnPaySuccess = false;
            int intCount = 60;//等待60S

            ////ShowInfo d = (ShowInfo)arg; ;
            Thread.Sleep(500);

            //---
            //do something
            if (PaymentsRequest(m_StrOrderId, m_intAmount, ref m_LinePayRequestOut)) //呼叫付款請求
            {//付款請求成功
                m_blnPaySuccess = true;//付款請求 API 執行成功            
                do
                {
                    if (LinePayAPI.PaymentsInfo(m_LinePayRequestOut.channelId, m_LinePayRequestOut.info.transactionId.ToString(), ref m_LinePayInfoOut))//確認交易結果
                    {//確認交易結果 API 執行成功

                        if (m_LinePayInfoOut.paymentStep == "confirm")
                        {//已完成支付 
                            m_blnPaySuccess = true;
                            if((m_LinePayInfoOut.info.merchantReference!=null) && (m_LinePayInfoOut.info.merchantReference.affiliateCards!=null) && (m_LinePayInfoOut.info.merchantReference.affiliateCards.Count>0))
                            {
                                MainPage.m_StrLinePayCarrierNumber = m_LinePayInfoOut.info.merchantReference.affiliateCards[0].cardId;//紀錄m_LinePayInfoOut.info.merchantReference.affiliateCards[0].cardId 結帳電子發票用
                            }
                            
                            m_StrOutputInfo = "";
                            break;
                        }
                        else if (m_LinePayInfoOut.paymentStep == "request")
                        {//支付請求中(等待完成支付)
                            if (LinePayAPI.m_LinePayModuleParams.env_type == "T")//測試環境專用
                            {
                                LinePayAPI.PaymentsConfirm(m_LinePayRequestOut.channelId, m_LinePayRequestOut.info.transactionId.ToString());
                            }
                        }
                        else
                        {//refund : 已退款

                        }
                        m_blnPaySuccess = false;
                        m_StrOutputInfo = $"LinePay確認交易結果失敗({m_LinePayInfoOut.returnCode}):" + "\n" + m_LinePayInfoOut.paymentStep + ";\t" + m_LinePayInfoOut.returnMessage;
                    }
                    else
                    {//確認交易結果 API 執行失敗
                        m_blnPaySuccess = false;
                        m_StrOutputInfo = $"LinePay確認交易結果失敗({m_LinePayInfoOut.returnCode}):" + "\n" + PaymentCode2Info(m_LinePayInfoOut.returnCode);
                        break;
                    }
                    intCount--;
                    //d.m_intCount = intCount;
                    Thread.Sleep(1000);
                } while(intCount > 0);
                //---do something
            }
            else
            {//付款請求 API 執行失敗
                m_blnPaySuccess = false;
                m_StrOutputInfo = $"LinePay付款請求失敗({m_LinePayRequestOut.returnCode}):" + "\n" + LinePayAPI.PaymentCode2Info(m_LinePayRequestOut.returnCode);
            }

            //Payment.m_intLINE_PAYAPICount++;//紀錄該筆交易 LINE_PAY API 呼叫次數

            //d.Invoke(new Action(d.Close));

        }
    }//public class LinePayAPI
}
