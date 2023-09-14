using System.Collections;
using System.Data;

namespace VPOS
{
    public class PrintData//資料物件
    {
        /*
        觸發的事件類型
        1:錢箱事件
        2:關帳/交班列印事件
        3:結帳列印事件
        4:補印-訂單列印事件
        5:補印-標籤列印事件
        6:補印-工作票列印事件
        7:外賣接單列印事件
        8:外賣接單列印發票
        9:悠遊卡退費帳單
        10:發票補印
        11:發票作廢列印
        12:Qrorder_BILL
        13:Qrorder_WORK_TICKET
        14:Qrorder_LABEL
        15:Qrorder_QR_CODE
        */
        public int m_index;//觸發的事件類型
        public String m_Data;//SQL 抓取資料依據 或者是相關資料組合用分號隔開 
        public PrintData() {
            m_index = 0;
            m_Data = "";
        }
        public PrintData(int index,String DataNo)
        {
            m_index = index;
            m_Data = DataNo;
        }
    }

    public class PrintThread
    {
        public static bool m_blnRunLoop = false;//執行序執行與否狀態變數

        //---
        //FIFO 變數區
        /*
        佇列功能編號 對應 印表機
            0: 運算產生列印資料
            1: 錢箱
            2: 報表
            3: 貼紙
            4: 發票
            5: 收據
            6: 智能食譜
            7: 號碼單
            8: 工作票
            9: 掃碼點單
        */
        public const int FIFOSIZE = 10; //印表機數量+1
        public static int [] m_intQueueSequence = new int[FIFOSIZE];//存放執行順序陣列,預留事後外部指定用
        public static object []m_FIFOLock = new object[FIFOSIZE];//執行序控制器
        public static Queue[]m_Queue = new Queue[FIFOSIZE];//FIFO 實體
        /*
        佇列(Queue)常用的方法如下
            名稱        說明
            Count       取得佇列中目前的項目數量
            Dequeue     從佇列前端取出一個項目，同時將其移除
            Enqueue     從佇列尾端加入一個項目
            Peek        從佇列前端取出一個項目，但不移除
        */
        //---FIFO 變數區 

        //---
        //SQL 相關變數區
        /*
        //非佇列 ~ 印表機編號
        [0]:錢箱
        [1]:收據
        [2]:發票
        [3]:智能
        [4]:報表
        [5]:貼紙
        [6]:號碼單
        [7]:工作票
        [8]:掃碼點單
         */
        public static bool[] m_blnPrinterOn = new bool[9];//紀錄印表機啟用狀態
        public static int[] m_intPIndex = new int[9];//紀錄印表機陣列索引值
        //---SQL 相關變數區

        public static void VarInit()//列印執行序變數初始化
        {
            //---
            //FIFO 變數區
            for (int i=0; i<FIFOSIZE; i++)
            {
                m_intQueueSequence[i] = i;//建立預設執行順序
                m_FIFOLock[i] = new object();
                m_Queue[i] = new Queue();
            }
            //---FIFO 變數區
        }

        public static void main()//執行序的主函數
        {
            m_blnRunLoop = true;

            do
            {
                for(int i=0; i<FIFOSIZE;i++)
                {
                    int intIndex = m_intQueueSequence[i];
                    if (m_Queue[intIndex].Count>0)
                    {
                        switch(intIndex)
                        {
                            case 0://0: 運算產生列印資料
                                PrintData();
                                break;
                            default://單純列印
                                PrintData2Device(intIndex);
                                break;
                        }
                    }
                }
                Thread.Sleep(500);
            }while(m_blnRunLoop);
        }//public static void main()

        public static void PrintData2Device(int intIndex)//單純列印
        {
            try
            {
                //---
                //取出列印資料
                PT_CommandOutput PT_CommandOutputs = new PT_CommandOutput();//輸出

                lock (m_FIFOLock[intIndex])
                {
                    PT_CommandOutputs = (PT_CommandOutput)m_Queue[intIndex].Dequeue();//取出值
                }
                //---取出列印資料

                if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0))
                {
                    printer_config printer_configBuf = new printer_config();

                    //---
                    //讀取印表機設定 進行連線+列印

                    //讀取設定
                    switch (intIndex)//FIFO順序
                    {
                        case 1://1: 錢箱
                            printer_configBuf = SQLDataTableModel.m_printer_valueList[m_intPIndex[0]].printer_config_value;
                            break;
                        case 2://2: 報表
                            printer_configBuf = SQLDataTableModel.m_printer_valueList[m_intPIndex[4]].printer_config_value;
                            break;
                        case 3://3: 貼紙
                            printer_configBuf = SQLDataTableModel.m_printer_valueList[m_intPIndex[5]].printer_config_value;
                            break;
                        case 4://4: 發票
                            printer_configBuf = SQLDataTableModel.m_printer_valueList[m_intPIndex[2]].printer_config_value;
                            break;
                        case 5://5: 收據
                            printer_configBuf = SQLDataTableModel.m_printer_valueList[m_intPIndex[1]].printer_config_value;
                            break;
                        case 6://6: 智能食譜
                            printer_configBuf = SQLDataTableModel.m_printer_valueList[m_intPIndex[3]].printer_config_value;
                            break;
                        case 7://7: 號碼單
                            printer_configBuf = SQLDataTableModel.m_printer_valueList[m_intPIndex[6]].printer_config_value;
                            break;
                        case 8://8: 工作單
                            printer_configBuf = SQLDataTableModel.m_printer_valueList[m_intPIndex[7]].printer_config_value;
                            break;
                        case 9://9: 掃描點單
                            printer_configBuf = SQLDataTableModel.m_printer_valueList[m_intPIndex[8]].printer_config_value;
                            break;
                    }

                    string[] strs;
                    switch (printer_configBuf.conn_type)
                    {
                        case "RS232":
                            strs = printer_configBuf.conn_data.Split(";");
                            PrinterModule.RS232Mode(strs[0], Int32.Parse(strs[1]), PT_CommandOutputs);
                            break;
                        case "TCP/IP":
                            strs = printer_configBuf.conn_data.Split(";");
                            PrinterModule.TcpMode(strs[0], Int32.Parse(strs[1]), PT_CommandOutputs);
                            break;
                        case "Dirver":
                            if (intIndex == 3)
                            {
                                PrinterModule.DriveMode(printer_configBuf.conn_data, PT_CommandOutputs, 1);//標籤機
                            }
                            else
                            {
                                PrinterModule.DriveMode(printer_configBuf.conn_data, PT_CommandOutputs);//一般印表
                            }
                            break;
                    }

                    //---讀取印表機設定 進行連線+列印
                }
            }
            catch (Exception e) 
            {
                LogFile.Write("PrintThread.PrintData2Device [Error] ; " + e.ToString());
            }

        }

        public static void PrintData()//0: 運算產生列印資料
        {
            
            PrintData PrintDataBuf = null;

            //---
            //取得印表機啟用狀態
            for (int i = 0; i < m_blnPrinterOn.Length; i++)
            {
                m_blnPrinterOn[i] = false;
            }
            for (int i=0;i < SQLDataTableModel.m_printer_valueList.Count; i++)
            {
                /*
                //非佇列 ~ 印表機編號
                [0]:錢箱
                [1]:收據
                [2]:發票
                [3]:智能
                [4]:報表
                [5]:貼紙
                [6]:號碼單
                [7]:工作票
                [8]:掃碼點單
                 */

                switch (SQLDataTableModel.m_printer_valueList[i].template_type)
                {
                    case "CASH_BOX"://0:錢箱
                        m_blnPrinterOn[0] = (SQLDataTableModel.m_printer_valueList[i].printer_config_value.on_off_state=="Y") ? true: false;
                        m_intPIndex[0] = i;
                        break;
                    case "BILL"://1:收據
                        m_blnPrinterOn[1] = (SQLDataTableModel.m_printer_valueList[i].printer_config_value.on_off_state == "Y") ? true : false;
                        m_intPIndex[1] = i;
                        break;
                    case "INVOICE"://2:發票
                        m_blnPrinterOn[2] = (SQLDataTableModel.m_printer_valueList[i].printer_config_value.on_off_state == "Y") ? true : false;
                        m_intPIndex[2] = i;
                        break;
                    case "SMART"://3:智能
                        m_blnPrinterOn[3] = (SQLDataTableModel.m_printer_valueList[i].printer_config_value.on_off_state == "Y") ? true : false;
                        m_intPIndex[3] = i;
                        break;
                    case "REPORT"://4:報表
                        m_blnPrinterOn[4] = (SQLDataTableModel.m_printer_valueList[i].printer_config_value.on_off_state == "Y") ? true : false;
                        m_intPIndex[4] = i;
                        break;
                    case "LABLE"://5:貼紙   //case "W":
                        m_blnPrinterOn[5] = (SQLDataTableModel.m_printer_valueList[i].printer_config_value.on_off_state == "Y") ? true : false;
                        m_intPIndex[5] = i;
                        break;
                    case "NUMBER"://6:號碼單
                        m_blnPrinterOn[6] = (SQLDataTableModel.m_printer_valueList[i].printer_config_value.on_off_state == "Y") ? true : false;
                        m_intPIndex[6] = i;
                        break;
                    case "WORK_TICKET"://7:工作單
                        m_blnPrinterOn[7] = (SQLDataTableModel.m_printer_valueList[i].printer_config_value.on_off_state == "Y") ? true : false;
                        m_intPIndex[7] = i;
                        break;
                    case "QRORDER"://8:掃碼點單
                        m_blnPrinterOn[8] = (SQLDataTableModel.m_printer_valueList[i].printer_config_value.on_off_state == "Y") ? true : false;
                        m_intPIndex[8] = i;
                        break;
                }
                
            }
            //---取得印表機啟用狀態

            lock (m_FIFOLock[0])
            {
                PrintDataBuf = (PrintData) m_Queue[0].Dequeue();//取出值
            }

            if(PrintDataBuf != null)
            {
                /*
                觸發的事件類型
                1:錢箱事件
                2:關帳/交班列印事件
                3:結帳列印事件
                4:補印-訂單列印事件
                5:補印-標籤列印事件
                6:補印-工作票列印事件
                7:外賣接單列印事件
                8:外賣接單列印發票
                9:悠遊卡退費帳單
                10:發票補印
                11:發票作廢列印
                12:Qrorder_BILL
                13:Qrorder_WORK_TICKET
                14:Qrorder_LABEL
                15:Qrorder_QR_CODE
                */
                switch (PrintDataBuf.m_index)
                {
                    case 1://1:錢箱事件
                        if(m_blnPrinterOn[0])
                        {
                            CashBox(PrintDataBuf);//1: 錢箱   
                        }            
                        break;
                    case 2://2:關帳/交班事件
                        if(m_blnPrinterOn[4])
                        {
                            Report(PrintDataBuf);//2: 報表
                        }                    
                        break;
                    case 3://3:結帳事件
                        if(m_blnPrinterOn[5])
                        {
                            Label(PrintDataBuf);//3: 貼紙
                        }
                        if(m_blnPrinterOn[2])
                        {
                            if (SqliteDataAccess.m_terminal_data[0].invoice_active_state == "Y")//判斷是否要開立電子發票
                            {
                                m_blnReprintInvoice = false;//預設為非補印
                                Invoice(PrintDataBuf);//4: 發票
                            }
                        }
                        if(m_blnPrinterOn[1])
                        {
                            Bill(PrintDataBuf,true,true);//5: 收據
                        }
                        if(m_blnPrinterOn[3])
                        {
                            Smart(PrintDataBuf);//6: 智能食譜
                        }
                        if(m_blnPrinterOn[6])
                        {
                            Number(PrintDataBuf);//7: 號碼單
                        }
                        if (m_blnPrinterOn[7])
                        {
                            Work(PrintDataBuf);//8: 工作單
                        }
                        break;
                    case 4://4:補印-訂單列印
                        if (m_blnPrinterOn[1])
                        {
                            Bill(PrintDataBuf,false,true);//5: 收據
                        }
                        if (m_blnPrinterOn[6])
                        {
                            Number(PrintDataBuf);//7: 號碼單
                        }
                        break;
                    case 5://5:補印-標籤列印
                        if (m_blnPrinterOn[5])
                        {
                            Label(PrintDataBuf,true);//3: 貼紙
                        }
                        break;
                    case 6://6:補印-工作票列印
                        if (m_blnPrinterOn[7])
                        {
                            Work(PrintDataBuf, true);//8: 工作單
                        }
                        break;
                    case 7://7:外賣接單列印
                        if (m_blnPrinterOn[5])
                        {
                            Label(PrintDataBuf);//3: 貼紙
                        }
                        if (m_blnPrinterOn[1])
                        {
                            Bill(PrintDataBuf,false,true);//5: 收據
                        }
                        if (m_blnPrinterOn[3])
                        {
                            Smart(PrintDataBuf);//6: 智能食譜
                        }
                        if (m_blnPrinterOn[6])
                        {
                            Number(PrintDataBuf);//7: 號碼單
                        }
                        if (m_blnPrinterOn[7])
                        {
                            Work(PrintDataBuf);//8: 工作單
                        }
                        break;
                    case 8://8:外賣接單發票
                        if (m_blnPrinterOn[2])
                        {
                            if (SqliteDataAccess.m_terminal_data[0].invoice_active_state == "Y")//判斷是否要開立電子發票
                            {
                                m_blnReprintInvoice = false;//預設為非補印
                                Invoice(PrintDataBuf);//4: 發票
                            }
                        }
                        break;
                    case 9://9:悠遊卡退費帳單
                        if (m_blnPrinterOn[1])
                        {
                            Bill(PrintDataBuf, true, false);//5: 收據
                        }
                        break;
                    case 10://發票補印
                        if (m_blnPrinterOn[2])
                        {
                            if (SqliteDataAccess.m_terminal_data[0].invoice_active_state == "Y")//判斷是否要開立電子發票
                            {
                                m_blnReprintInvoice = true;//補印旗標設定
                                Invoice(PrintDataBuf);//4: 發票
                            }
                        }
                        break;
                    case 11://11:發票作廢列印
                        if (m_blnPrinterOn[2])
                        {
                            if (SqliteDataAccess.m_terminal_data[0].invoice_active_state == "Y")//判斷是否要開立電子發票
                            {
                                m_blnReprintInvoice = false;//預設為非補印
                                Invoice(PrintDataBuf);//4: 發票
                            }
                        }
                        break;
                    case 12://12:Qrorder_BILL
                        if (m_blnPrinterOn[1])
                        {
                            QrOrder_Bill(PrintDataBuf);
                        }
                        break;
                    case 13://13:Qrorder_WORK_TICKET
                        if (m_blnPrinterOn[7])
                        {
                            QrOrder_Work(PrintDataBuf);
                        }
                        break;
                    case 14://14:Qrorder_LABEL
                        if (m_blnPrinterOn[5])
                        {
                            QrOrder_Label(PrintDataBuf);
                        }
                        break;
                    case 15://15:Qrorder_QR_CODE
                        if (m_blnPrinterOn[8])
                        {
                            QrOrder_QrCode(PrintDataBuf);
                        }
                        break;

                }
            }

        }

        public static void PT_CommandOutput2Queue(int intIndex,PT_CommandOutput PT_CommandOutputs)//將印表運算結果放置FIFO
        {
            lock (m_FIFOLock[intIndex])
            {
                m_Queue[intIndex].Enqueue(PT_CommandOutputs);//塞入值
            }
        }

        public static void CashBox(PrintData PrintDataBuf)//1: 錢箱印表運算
        {
            PT_CommandOutput PT_CommandOutputs = new PT_CommandOutput();//輸出
            //---
            //運算

            //產生列印資料
            PrinterTemplate.ESCPOS_Cash(out PT_CommandOutputs);

            //---運算
            
            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[0]].printer_config_value.new_bill_no_print == "N"))
            {
                for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[0]].printer_config_value.bill_print_count.ToString()); i++)
                {
                    PT_CommandOutput2Queue(1, PT_CommandOutputs);
                }
                LogFile.Write("PrintThread.CashBox ; " + "Main Print");
            }
        }
        
        public static void Report(PrintData PrintDataBuf)//2: 報表印表運算
        {
            PT_CommandOutput PT_CommandOutputs = new PT_CommandOutput();//輸出

            //---
            //悠遊卡報表印表
            PT_CommandOutputs = new PT_CommandOutput();//輸出
            
            String SQL = "";
            SQL = String.Format("SELECT easycard_checkout_info FROM daily_report WHERE report_no='{0}' LIMIT 0,1", PrintDataBuf.m_Data);
            DataTable order_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
            if (order_dataDataTable.Rows.Count > 0)
            {
                //產生JSON
                String StrEasyCardInput = order_dataDataTable.Rows[0][0].ToString();

                if((StrEasyCardInput!=null) && (StrEasyCardInput.Length>0))
                {
                    //產生列印資料
                    PrinterTemplate.EasyCard_Report(StrEasyCardInput, out PT_CommandOutputs);

                    if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[4]].printer_config_value.new_bill_no_print == "N"))
                    {
                        for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[4]].printer_config_value.bill_print_count.ToString()); i++)
                        {
                            PT_CommandOutput2Queue(2, PT_CommandOutputs);
                        }
                        LogFile.Write("PrintThread.Report ; " + "EasyCard_Report Print ; " + StrEasyCardInput);
                    }
                }//if (order_dataDataTable.Rows.Count > 0)
            }//if (order_dataDataTable.Rows.Count > 0)      
            //---悠遊卡報表印表

            //---
            //POS 標準報表印表

            PT_CommandOutputs = new PT_CommandOutput();//輸出
            
            //產生JSON
            daily_report daily_reportBuf = new daily_report();
            DB2daily_report.daily_report2Var(PrintDataBuf.m_Data, ref daily_reportBuf);
            String Strdaily_report = JsonClassConvert.daily_report2String(daily_reportBuf);

            //產生列印資料
            PrinterTemplate.ESCPOS_Report(Strdaily_report, out PT_CommandOutputs);

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[4]].printer_config_value.new_bill_no_print == "N"))
            {
                for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[4]].printer_config_value.bill_print_count.ToString()); i++)
                {
                    PT_CommandOutput2Queue(2, PT_CommandOutputs);
                }
                LogFile.Write("PrintThread.Report ; " + "Main Print ; " + Strdaily_report);
            }
            //---POS 標準報表印表


        }

        public static void QrOrder_Label(PrintData PrintDataBuf)
        {
            PT_CommandOutput PT_CommandOutputs = new PT_CommandOutput();//輸出

            String Strorders_new = PrintDataBuf.m_Data;
            String Strproduct_memo = "";
            //產生列印資料
            PrinterTemplate.ESCPOS_Lable(Strorders_new, Strproduct_memo, out PT_CommandOutputs);

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[5]].printer_config_value.new_bill_no_print == "N"))
            {
                for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[5]].printer_config_value.bill_print_count.ToString()); i++)
                {
                    PT_CommandOutput2Queue(3, PT_CommandOutputs);
                }
                LogFile.Write("PrintThread.QrOrder_Label ; " + "Main Print ; " + Strorders_new);
            }

        }

        public static void Label(PrintData PrintDataBuf, bool Afterward = false)//3: 貼紙印表運算
        {
            PT_CommandOutput PT_CommandOutputs = new PT_CommandOutput();//輸出
            //---
            //運算

            //產生JSON
            orders_new orders_newBuf = new orders_new();
            DB2orders_new.company2Var(ref orders_newBuf);
            String StrDataNo = "";
            string[] strsProductNoLists = null;
            if (Afterward)
            {
                StrDataNo = PrintDataBuf.m_Data.Split(";")[0];
                strsProductNoLists = PrintDataBuf.m_Data.Split(";")[1].Split(",");
            }
            else
            {
                StrDataNo = PrintDataBuf.m_Data;
            }
            DB2orders_new.order_data2Var(StrDataNo, ref orders_newBuf);
            DB2orders_new.order_content_data2Var(StrDataNo, ref orders_newBuf);
            DB2orders_new.order_payment_data2Var(StrDataNo, ref orders_newBuf);
            DB2orders_new.order_invoice_data2Var(StrDataNo, ref orders_newBuf);
            DB2orders_new.material2Var(ref orders_newBuf);
            if ((strsProductNoLists != null) && (strsProductNoLists.Length > 0))
            {
                List<OrderItem> orderItemBuf = new List<OrderItem>();
                orderItemBuf.Clear();

                for (int i = 0; i < orders_newBuf.order_items.Count; i++)
                {
                    for (int j = 0; j < strsProductNoLists.Length; j++)
                    {
                        if (orders_newBuf.order_items[i].product_code == strsProductNoLists[j])
                        {
                            OrderItem orderItem = orders_newBuf.order_items[i];
                            orderItemBuf.Add(orderItem);
                            break;
                        }
                    }
                }

                orders_newBuf.order_items = orderItemBuf;
            }
            orders_newBuf.printer_groupFilter(Int32.Parse(orders_newBuf.order_type), SQLDataTableModel.m_printer_valueList[m_intPIndex[5]].SID);//貼紙
            String Strorders_new_groupFilter = JsonClassConvert.ordersnew2String(orders_newBuf);

            product_memo product_memoBuf = new product_memo();
            DB2orders_new.product_memo2Var(PrintDataBuf.m_Data, ref product_memoBuf);
            String Strproduct_memo = JsonClassConvert.product_memo2String(product_memoBuf);

            //產生列印資料
            PrinterTemplate.ESCPOS_Lable(Strorders_new_groupFilter, Strproduct_memo, out PT_CommandOutputs);

            //---運算

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[5]].printer_config_value.new_bill_no_print == "N"))
            {
                for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[5]].printer_config_value.bill_print_count.ToString()); i++)
                {
                    PT_CommandOutput2Queue(3, PT_CommandOutputs);
                }
                LogFile.Write("PrintThread.Label ; " + "Main Print ; " + Strorders_new_groupFilter);
            }
        }

        public static bool m_blnReprintInvoice = false;//預設為非補印
        public static void Invoice(PrintData PrintDataBuf)//4: 發票印表運算
        {
            PT_CommandOutput PT_CommandOutputs = new PT_CommandOutput();//輸出
            //---
            //運算
            orders_new orders_newBuf = new orders_new();
            DB2orders_new.company2Var(ref orders_newBuf);
            String StrDataNo = PrintDataBuf.m_Data;
            DB2orders_new.order_data2Var(StrDataNo, ref orders_newBuf);
            DB2orders_new.order_content_data2Var(StrDataNo, ref orders_newBuf);
            DB2orders_new.order_payment_data2Var(StrDataNo, ref orders_newBuf);
            DB2orders_new.order_invoice_data2Var(StrDataNo, ref orders_newBuf);
            DB2orders_new.material2Var(ref orders_newBuf);
            if((orders_newBuf.invoice_data.carrier_code_1!="") && (orders_newBuf.invoice_data.donate_code!=""))
            {
                return;//有載具不用列印發票
            }

            string Strorders_new = SyncThread.Create_NOD_DOD_Json(StrDataNo);//直接使用訂單上傳VTEAM_CLOUD的JSON資料完整結構，包含訂單主檔，子檔、支付，載具等資訊
            string StrAPIResult = "";

            if (POS_INVAPI.POS_Order_2_Invoice_B2C_Order(Strorders_new, ref StrAPIResult))//轉換後的資料是拿來列印或上傳到 cloud的
            {
                PrinterTemplate.ESCPOS_Invoice(Strorders_new, StrAPIResult, out PT_CommandOutputs);
            }
            //---運算

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[5]].printer_config_value.new_bill_no_print == "N"))
            {
                for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[5]].printer_config_value.bill_print_count.ToString()); i++)
                {
                    PT_CommandOutput2Queue(4, PT_CommandOutputs);
                }
                LogFile.Write("PrintThread.Invoice ; " + StrAPIResult);
            }
            m_blnReprintInvoice = false;//預設為非補印
        }

        public static void QrOrder_Bill(PrintData PrintDataBuf)//5: 收據印表運算
        {
            PT_CommandOutput PT_CommandOutputs = new PT_CommandOutput();//輸出

            PT_CommandOutputs = new PT_CommandOutput();//輸出
            String Strorders_new = PrintDataBuf.m_Data;

            //產生列印資料
            PrinterTemplate.ESCPOS_Bill(Strorders_new, out PT_CommandOutputs);

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[1]].printer_config_value.new_bill_no_print == "N"))
            {
                for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[1]].printer_config_value.bill_print_count.ToString()); i++)
                {
                    PT_CommandOutput2Queue(5, PT_CommandOutputs);
                }
                LogFile.Write("PrintThread.QrOrder_Bill ; " + "Main Print ; " + Strorders_new);
            }
            //---POS 標準收據印表
        }
        public static void Bill(PrintData PrintDataBuf,bool blnPaymentModule = true,bool blnNormal=true)//5: 收據印表運算
        {
            /*
            true,true   -> 印悠遊卡(購貨),印一般收據
            true,false  -> 印悠遊卡(退貨),不印一般收據
            false,true  -> 不印悠遊卡,印一般收據
            flase,false -> 不印悠遊卡,不印一般收據 [不會出現]
            */
            PT_CommandOutput PT_CommandOutputs = new PT_CommandOutput();//輸出

            //---
            //支付收據印表
            if (blnPaymentModule)
            {
                //產生JSON
                String SQL = "";
                if (blnNormal)
                {//(購貨)
                    SQL = String.Format("SELECT payment_info,payment_module_code FROM order_payment_data WHERE order_no='{0}'", PrintDataBuf.m_Data);
                }
                else
                {//(退貨)
                    SQL = String.Format("SELECT refund_info,payment_module_code FROM order_payment_data WHERE order_no='{0}'", PrintDataBuf.m_Data);
                }

                DataTable order_payment_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
                if (order_payment_dataDataTable.Rows.Count > 0)
                {
                    for (int j = 0; j < order_payment_dataDataTable.Rows.Count; j++)
                    {
                        //悠遊卡收據印表
                        if (order_payment_dataDataTable.Rows[j][1].ToString()== "EASY_CARD")//悠遊卡額外列印
                        {
                            String StrEasyCardInput = order_payment_dataDataTable.Rows[j][0].ToString();
                            PT_CommandOutputs = new PT_CommandOutput();

                            if (StrEasyCardInput.Length > 0)//確定悠遊卡有支付成功
                            {
                                //產生列印資料
                                PrinterTemplate.EasyCard_Bill(StrEasyCardInput, out PT_CommandOutputs);

                                if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[1]].printer_config_value.new_bill_no_print == "N"))
                                {
                                    for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[1]].printer_config_value.bill_print_count.ToString()); i++)
                                    {
                                        PT_CommandOutput2Queue(5, PT_CommandOutputs);
                                    }
                                    LogFile.Write("PrintThread.Bill ; " + "EasyCard_Bill Print ; " + StrEasyCardInput);
                                }
                            }//if (StrEasyCardInput.Length > 0)
                        }//if(order_payment_dataDataTable.Rows[j][1].ToString()== "EASY_CARD")

                        //聯信收據印表
                        if (order_payment_dataDataTable.Rows[j][1].ToString() == "NCCC_CREDIT_CARD")//聯合信用卡小額交易額外列印
                        {
                            String StrNCCCInput = order_payment_dataDataTable.Rows[j][0].ToString();
                            PT_CommandOutputs = new PT_CommandOutput();
                            if(StrNCCCInput.Length>0)
                            {
                                CreditCardJosn CreditCardJosnBuf = JsonClassConvert.CreditCardJosn2Class(StrNCCCInput);
                                if((CreditCardJosnBuf != null) && (CreditCardJosnBuf.Print_Receipt!=null) && (CreditCardJosnBuf.Print_Receipt=="Y"))
                                {
                                    //產生列印資料
                                    PrinterTemplate.NCCC_Bill(StrNCCCInput, out PT_CommandOutputs);
                                }

                                if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[1]].printer_config_value.new_bill_no_print == "N"))
                                {
                                    for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[1]].printer_config_value.bill_print_count.ToString()); i++)
                                    {
                                        PT_CommandOutput2Queue(5, PT_CommandOutputs);
                                    }
                                    LogFile.Write("PrintThread.Bill ; " + "NCCC_Bill Print ; " + StrNCCCInput);
                                }
                            }//if(StrNCCCInput.Length>0)
                        }//if (order_payment_dataDataTable.Rows[j][1].ToString() == "NCCC_CREDIT_CARD")

                    }//for(int j=0;j<order_payment_dataDataTable.Rows.Count;j++)

                }//if (order_payment_dataDataTable.Rows.Count > 0)
            }
            //---支付收據印表

            //---
            //POS 標準收據印表
            if (blnNormal)
            {
                PT_CommandOutputs = new PT_CommandOutput();//輸出

                //產生JSON
                orders_new orders_newBuf = new orders_new();
                DB2orders_new.company2Var(ref orders_newBuf);
                DB2orders_new.order_data2Var(PrintDataBuf.m_Data, ref orders_newBuf);
                DB2orders_new.order_content_data2Var(PrintDataBuf.m_Data, ref orders_newBuf);
                DB2orders_new.order_payment_data2Var(PrintDataBuf.m_Data, ref orders_newBuf);
                DB2orders_new.order_invoice_data2Var(PrintDataBuf.m_Data, ref orders_newBuf);
                DB2orders_new.material2Var(ref orders_newBuf);
                String Strorders_new = JsonClassConvert.ordersnew2String(orders_newBuf);

                //產生列印資料
                PrinterTemplate.ESCPOS_Bill(Strorders_new, out PT_CommandOutputs);

                if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[1]].printer_config_value.new_bill_no_print == "N"))
                {
                    for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[1]].printer_config_value.bill_print_count.ToString()); i++)
                    {
                        PT_CommandOutput2Queue(5, PT_CommandOutputs);
                    }
                    LogFile.Write("PrintThread.Bill ; " + "Main Print ; " + Strorders_new);
                }

            }
            //---POS 標準收據印表
        }

        public static void Smart(PrintData PrintDataBuf)//6: 智能食譜印表運算
        {
            PT_CommandOutput PT_CommandOutputs = new PT_CommandOutput();//輸出
            //---
            //運算

            //產生JSON
            orders_new orders_newBuf = new orders_new();
            DB2orders_new.company2Var(ref orders_newBuf);
            DB2orders_new.order_data2Var(PrintDataBuf.m_Data, ref orders_newBuf);
            DB2orders_new.order_content_data2Var(PrintDataBuf.m_Data, ref orders_newBuf);
            DB2orders_new.order_payment_data2Var(PrintDataBuf.m_Data, ref orders_newBuf);
            DB2orders_new.order_invoice_data2Var(PrintDataBuf.m_Data, ref orders_newBuf);
            DB2orders_new.material2Var(ref orders_newBuf);
            String Strorders_new = JsonClassConvert.ordersnew2String(orders_newBuf);

            //產生列印資料
            PrinterTemplate.ESCPOS_Smart(Strorders_new, out PT_CommandOutputs);

            //---運算

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[3]].printer_config_value.new_bill_no_print == "N"))
            {
                for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[3]].printer_config_value.bill_print_count.ToString()); i++)
                {
                    PT_CommandOutput2Queue(6, PT_CommandOutputs);
                }
                LogFile.Write("PrintThread.Smart ; " + "Main Print ; " + Strorders_new);
            }
        }

        public static void Number(PrintData PrintDataBuf)//7: 號碼單印表運算
        {
            PT_CommandOutput PT_CommandOutputs = new PT_CommandOutput();//輸出
            //---
            //運算

            //產生JSON
            orders_new orders_newBuf = new orders_new();
            DB2orders_new.company2Var(ref orders_newBuf);            
            DB2orders_new.order_data2Var(PrintDataBuf.m_Data, ref orders_newBuf);
            DB2orders_new.order_content_data2Var(PrintDataBuf.m_Data, ref orders_newBuf);
            DB2orders_new.order_payment_data2Var(PrintDataBuf.m_Data, ref orders_newBuf);
            DB2orders_new.order_invoice_data2Var(PrintDataBuf.m_Data, ref orders_newBuf);
            DB2orders_new.material2Var(ref orders_newBuf);
            orders_newBuf.printer_groupFilter(Int32.Parse(orders_newBuf.order_type), SQLDataTableModel.m_printer_valueList[m_intPIndex[6]].SID, false);//號碼單
            String Strorders_new_groupFilter = JsonClassConvert.ordersnew2String(orders_newBuf);

            //產生列印資料
            PrinterTemplate.ESCPOS_Number(Strorders_new_groupFilter, out PT_CommandOutputs);
            //---運算

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[6]].printer_config_value.new_bill_no_print == "N"))
            {
                for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[6]].printer_config_value.bill_print_count.ToString()); i++)
                {
                    PT_CommandOutput2Queue(7, PT_CommandOutputs);
                }
                LogFile.Write("PrintThread.Number ; " + "Main Print ; " + Strorders_new_groupFilter);
            }
        }

        public static void QrOrder_Work(PrintData PrintDataBuf)
        {
            PT_CommandOutput PT_CommandOutputs = new PT_CommandOutput();//輸出

            String Strorders_new = PrintDataBuf.m_Data;
            //產生列印資料
            PrinterTemplate.ESCPOS_Work(Strorders_new, out PT_CommandOutputs);

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[7]].printer_config_value.new_bill_no_print == "N"))
            {
                for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[7]].printer_config_value.bill_print_count.ToString()); i++)
                {
                    PT_CommandOutput2Queue(8, PT_CommandOutputs);
                }
                LogFile.Write("PrintThread.QrOrder_Work ; " + "Main Print ; " + Strorders_new);
            }
        }
        public static void Work(PrintData PrintDataBuf, bool Afterward = false)//8: 工作單印表運算
        {
            PT_CommandOutput PT_CommandOutputs = new PT_CommandOutput();//輸出
            //---
            //運算

            //產生JSON
            orders_new orders_newBuf = new orders_new();
            DB2orders_new.company2Var(ref orders_newBuf);
            String StrDataNo = "";
            string[] strsProductNoLists = null;
            if (Afterward)
            {
                StrDataNo = PrintDataBuf.m_Data.Split(";")[0];
                strsProductNoLists = PrintDataBuf.m_Data.Split(";")[1].Split(",");
            }
            else
            {
                StrDataNo = PrintDataBuf.m_Data;
            }
            DB2orders_new.order_data2Var(StrDataNo, ref orders_newBuf);
            DB2orders_new.order_content_data2Var(StrDataNo, ref orders_newBuf);
            DB2orders_new.order_payment_data2Var(StrDataNo, ref orders_newBuf);
            DB2orders_new.order_invoice_data2Var(StrDataNo, ref orders_newBuf);
            DB2orders_new.material2Var(ref orders_newBuf);
            if ((strsProductNoLists != null) && (strsProductNoLists.Length > 0))
            {
                List<OrderItem> orderItemBuf = new List<OrderItem>();
                orderItemBuf.Clear();

                for (int i = 0; i < orders_newBuf.order_items.Count; i++)
                {
                    for (int j = 0; j < strsProductNoLists.Length; j++)
                    {
                        if (orders_newBuf.order_items[i].product_code == strsProductNoLists[j])
                        {
                            OrderItem orderItem = orders_newBuf.order_items[i];
                            orderItemBuf.Add(orderItem);
                            break;
                        }
                    }
                }

                orders_newBuf.order_items = orderItemBuf;
            }

            //orders_newBuf.printer_groupFilter(Int32.Parse(orders_newBuf.order_type), SQLDataTableModel.m_printer_valueList[m_intPIndex[7]].SID);//工作單
            orders_newBuf.mergeItems();
            String Strorders_new_groupFilter = JsonClassConvert.ordersnew2String(orders_newBuf);

            //產生列印資料
            PrinterTemplate.ESCPOS_Work(Strorders_new_groupFilter, out PT_CommandOutputs);
            //---運算

            if ((PT_CommandOutputs!=null) && (PT_CommandOutputs.value!=null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[7]].printer_config_value.new_bill_no_print == "N"))
            {
                for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[7]].printer_config_value.bill_print_count.ToString()); i++)
                {
                    PT_CommandOutput2Queue(8, PT_CommandOutputs);
                }
                LogFile.Write("PrintThread.Work ; " + "Main Print ; " + Strorders_new_groupFilter);
            }
        }

        public static void QrOrder_QrCode(PrintData PrintDataBuf)
        {
            PT_CommandOutput PT_CommandOutputs = new PT_CommandOutput();//輸出

            String Strorders_new = PrintDataBuf.m_Data;
            //產生列印資料
            PrinterTemplate.ESCPOS_QrCode(Strorders_new, out PT_CommandOutputs);

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.value != null) && (PT_CommandOutputs.value.Count > 0) && (SQLDataTableModel.m_printer_valueList[m_intPIndex[8]].printer_config_value.new_bill_no_print == "N"))
            {
                for (int i = 0; i < Int32.Parse(SQLDataTableModel.m_printer_valueList[m_intPIndex[8]].printer_config_value.bill_print_count.ToString()); i++)
                {
                    PT_CommandOutput2Queue(9, PT_CommandOutputs);
                }
                LogFile.Write("PrintThread.QrOrder_QrCode ; " + "Main Print ; " + Strorders_new);
            }
        }

    }//public class PrintThread
}
