

namespace VPOS
{
    public class EasyCardAPI
    {
        public static void SingnOn()//悠遊卡 Singn On
        {
            bool blnRun = false;
            if((SqliteDataAccess.m_company_payment_type!=null) &&(SqliteDataAccess.m_company_payment_type.Count>0))
            {
                for(int i=0; i<SqliteDataAccess.m_company_payment_type.Count;i++)
                {
                    if (SqliteDataAccess.m_company_payment_type[i].payment_module_code== "EASY_CARD")
                    {
                        if(Directory.Exists(POS_ECMAPI.m_StrSysPath))//對應模組目錄也存在
                        {
                            blnRun = true;//支付方式有悠遊卡，才需要初始化悠遊卡機
                        }
                        else
                        {
                            blnRun = false;
                        }
                        break;
                    }
                }
            }

            if ((HttpsFun.m_intNetworkLevel > 0) && (blnRun) && (POS_ECMAPI.m_blnRunInit) && (POS_ECMAPI.m_EASY_CARDModule != null))
            {
                WaitUIThread.ShowWaitInfo("悠遊卡機【Sign On】中，請稍後...", POS_ECMAPI.InitDevice);//透過執行序進行等待支付結果確認
                if (!POS_ECMAPI.m_blnAPIResult)
                {
                    //Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
                    //MsgBuf.ShowDialog();//顯示失敗原因
                }
            }
        }

        public static bool GetCardInfo(ref String StrResult)//悠遊卡 卡片讀取 
        {
            bool blnResult = false;
            if (POS_ECMAPI.m_EASY_CARDModule != null)
            {
                WaitUIThread.ShowWaitInfo("悠遊卡 卡片讀取中，請稍後...", POS_ECMAPI.GetCardInfo);
                if (POS_ECMAPI.m_blnAPIResult)
                {
                    //Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
                    //MsgBuf.ShowDialog();//顯示餘額
                }
                else
                {
                    //Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
                    //MsgBuf.ShowDialog();//顯示失敗原因
                }

                blnResult = POS_ECMAPI.m_blnAPIResult;
                StrResult = POS_ECMAPI.m_StrAPIJsonResult;
            }

            return blnResult;
        }

        public static bool CashAddValue(int intAmount, ref String StrResult)//悠遊卡 卡片加值
        {
            bool blnResult = false;
            if (POS_ECMAPI.m_EASY_CARDModule != null)
            {
                POS_ECMAPI.m_intAmount = intAmount;//卡片加值金額
                WaitUIThread.ShowWaitInfo($"悠遊卡 【卡片加值】 {POS_ECMAPI.m_intAmount}元中，請稍後...", POS_ECMAPI.CashAddValue);
                if (POS_ECMAPI.m_blnAPIResult)
                {

                    //Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
                    //MsgBuf.ShowDialog();
                }
                else
                {
                    //Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
                    //MsgBuf.ShowDialog();
                }

                blnResult = POS_ECMAPI.m_blnAPIResult;
                StrResult = POS_ECMAPI.m_StrAPIJsonResult;
            }

            return blnResult;
        }

        public static bool Payments(int intAmount, ref String StrResult)//悠遊卡 扣款
        {
            bool blnResult = false;
            if (POS_ECMAPI.m_EASY_CARDModule != null)
            {
                POS_ECMAPI.m_intAmount = intAmount;//扣款金額
                POS_ECMAPI.m_blnRetry = false;//重新測試旗標
                POS_ECMAPI.m_intRetryCount = 0;//重新測試次數
                do
                {
                    WaitUIThread.ShowWaitInfo($"悠遊卡 【扣款】 {POS_ECMAPI.m_intAmount}元中，\n請將卡片放在感應區上並稍後片刻...", POS_ECMAPI.Deduct);
                    EasyCardAPIMsg EasyCardAPIMsgBuf = JsonClassConvert.EasyCardAPIMsg2Class(POS_ECMAPI.m_StrAPIJsonResult);
                    if ((POS_ECMAPI.m_blnAPIResult) && (EasyCardAPIMsgBuf.Trans_Success == "Y"))
                    {
                        break;
                    }
                    else
                    {
                        POS_ECMAPI.m_blnRetry = true;
                        POS_ECMAPI.m_intRetryCount++;
                        if ((EasyCardAPIMsgBuf.Retry_Nex_Flag=="Y") && (POS_ECMAPI.m_intRetryCount<=3))
                        {
                            //QuestionMsg QuestionMsgBuf = new QuestionMsg(POS_ECMAPI.m_StrAPIResult + $"\n是否進行第 {POS_ECMAPI.m_intRetryCount}重試扣款");
                            //QuestionMsgBuf.ShowDialog();
                            if(!true)//if (!QuestionMsgBuf.m_blnRun)
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                } while (POS_ECMAPI.m_intRetryCount <= 3);

                blnResult = POS_ECMAPI.m_blnAPIResult;
                StrResult = POS_ECMAPI.m_StrAPIJsonResult;

                //Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
                //MsgBuf.ShowDialog();
            }

            return blnResult;
        }

        public static bool Refund(int intAmount,ref String StrResult)//悠遊卡 退貨
        {
            bool blnResult = false;
            if (POS_ECMAPI.m_EASY_CARDModule != null)
            {
                POS_ECMAPI.m_intAmount = intAmount;//退貨金額
                POS_ECMAPI.m_blnRetry = false;//重新測試旗標
                POS_ECMAPI.m_intRetryCount = 0;//重新測試次數
                do
                {
                    WaitUIThread.ShowWaitInfo($"悠遊卡 【退貨】 {POS_ECMAPI.m_intAmount}元中，\n請將卡片放在感應區上並稍後片刻...", POS_ECMAPI.Refund);
                    EasyCardAPIMsg EasyCardAPIMsgBuf = JsonClassConvert.EasyCardAPIMsg2Class(POS_ECMAPI.m_StrAPIJsonResult);
                    if ((POS_ECMAPI.m_blnAPIResult) && (EasyCardAPIMsgBuf.Trans_Success=="Y"))
                    {
                        break;
                    }
                    else
                    {
                        POS_ECMAPI.m_blnRetry = true;
                        POS_ECMAPI.m_intRetryCount++;
                        if ((EasyCardAPIMsgBuf.Retry_Nex_Flag == "Y") && (POS_ECMAPI.m_intRetryCount <= 3))
                        {
                            //QuestionMsg QuestionMsgBuf = new QuestionMsg(POS_ECMAPI.m_StrAPIResult + $"\n是否進行第 {POS_ECMAPI.m_intRetryCount}重試退貨");
                            //QuestionMsgBuf.ShowDialog();
                            if(!true)//if (!QuestionMsgBuf.m_blnRun)
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                } while (POS_ECMAPI.m_intRetryCount <= 3);

                blnResult = POS_ECMAPI.m_blnAPIResult;
                StrResult = POS_ECMAPI.m_StrAPIJsonResult;

                //Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
                //MsgBuf.ShowDialog();
            }

            return blnResult;
        }

        public static bool GetCardBillInfo(String StrTransSID,ref String StrResult)//讀取悠遊卡交易帳單資訊(JSON)
        {
            bool blnResult = false;

            POS_ECMAPI.m_StrTransSID = StrTransSID;
            WaitUIThread.ShowWaitInfo($"悠遊卡 【讀取悠遊卡交易帳單資訊】 交易序號: {POS_ECMAPI.m_StrTransSID}中，請稍後...", POS_ECMAPI.GetCardBillInfo);
            
            blnResult = POS_ECMAPI.m_blnAPIResult;
            StrResult = POS_ECMAPI.m_StrAPIJsonResult;

            //Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
            //MsgBuf.ShowDialog();

            return blnResult;
        }

        public static bool Checkout(ref String StrResult)//關帳
        {
            bool blnResult = false;
            if (POS_ECMAPI.m_EASY_CARDModule != null)
            {
                POS_ECMAPI.m_blnRetry = false;//重新測試旗標
                POS_ECMAPI.m_intRetryCount = 0;//重新測試次數
                do
                {
                    WaitUIThread.ShowWaitInfo($"悠遊卡 【關帳】中，請稍後...", POS_ECMAPI.Checkout);
                    ECAMCheckout ECAMCheckoutBuf = JsonClassConvert.ECAMCheckout2Class(POS_ECMAPI.m_StrAPIJsonResult);
                    if ((POS_ECMAPI.m_blnAPIResult) && (ECAMCheckoutBuf.Trans_Success=="Y"))
                    {
                        break;
                    }
                    else
                    {
                        POS_ECMAPI.m_blnRetry = true;
                        POS_ECMAPI.m_intRetryCount++;               
                        if ((ECAMCheckoutBuf.Retry_Nex_Flag == "Y") && (POS_ECMAPI.m_intRetryCount <= 3))
                        {
                            //QuestionMsg QuestionMsgBuf = new QuestionMsg(POS_ECMAPI.m_StrAPIResult + $"\n是否進行第 {POS_ECMAPI.m_intRetryCount}【關帳】重試");
                            //QuestionMsgBuf.ShowDialog();
                            if(!true)//if (!QuestionMsgBuf.m_blnRun)
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                } while (POS_ECMAPI.m_intRetryCount <= 3);

                blnResult = POS_ECMAPI.m_blnAPIResult;
                StrResult = POS_ECMAPI.m_StrAPIJsonResult;
                
                if(POS_ECMAPI.m_StrAPIResult.Length>0)
                {
                    //Msg MsgBuf = new Msg(POS_ECMAPI.m_StrAPIResult);
                    //MsgBuf.ShowDialog();
                }
            }

            return blnResult;
        }
    }//public class EasyCardAPI

    /*
        //---
        //悠遊卡 關帳
        String StrResult = "";
        EasyCardAPI.Checkout(ref StrResult);//關帳
        //---悠遊卡 關帳
     */
}
