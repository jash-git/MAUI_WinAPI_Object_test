using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace VPOS
{
    public class CustomerDisplayUDP
    {
        //---
        //
        private static int m_intUdpPotr = 8888;
        //---

        //---
        //
        public static CustomerDisplay m_CustomerDisplay = new CustomerDisplay();
        public static int m_intApiState = -1;//更新客顯示訊0;結帳完成時1;清除客顯2
        //---

        private static bool ShopCart2CustomerDisplay()
        {
            bool blnResult=false;            
            
            try
            {
                switch (m_intApiState)
                {
                    case 0://更新客顯示訊
                        break;
                    case 1://結帳完成時
                        m_CustomerDisplay.ItemInfo = null;
                        break;
                    case 2://清除客顯
                        m_CustomerDisplay.OrderInfo.ClearFlag = "Y";
                        m_CustomerDisplay.ItemInfo = null;
                        break;
                }

                blnResult = true;

            }
            catch(Exception ex) 
            {
                LogFile.Write("CustomerDisplayUDP.ShopCart2CustomerDisplay ERROR ; " + ex.ToString());
            }

            return blnResult;
        }

        public static void ToUdp()
        {
            if ((SQLDataTableModel.m_cust_display_param != null) && (SQLDataTableModel.m_cust_display_param.Rows.Count > 0) && (SQLDataTableModel.m_cust_display_param.Rows[0]["display_show"].ToString() == "Y"))
            {
                if (ShopCart2CustomerDisplay())
                {
                    try
                    {
                        String StrData = JsonClassConvert.CustomerDisplay2String(m_CustomerDisplay);
                        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), m_intUdpPotr);
                        UdpClient uc = new UdpClient();
                        LogFile.Write("CustomerDisplayUDP.ToUdp; " + StrData);
                        byte[] b = System.Text.Encoding.UTF8.GetBytes(StrData);
                        uc.Send(b, b.Length, ipep);

                        m_CustomerDisplay = null; ;
                        m_intApiState = -1;
                    }
                    catch (Exception ex)
                    {
                        LogFile.Write("CustomerDisplayUDP.ToUdp ERROR ; " + ex.ToString());
                    }
                }
            }
        }
    }
}
;