using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class SyncDBFun
    {
        public static DataTable m_upload_dataDataTable = new DataTable();
        public static int m_intUploadRowsCount = 0;
        public static int UploadRowsCount()//取得 背景同步(上傳)資料筆數
        {
            m_intUploadRowsCount = 0;
            String SQL = "SELECT SID,data_no,upload_type,try_count FROM upload_data WHERE upload_state='N' AND (LENGTH(upload_msg)=0 OR (upload_msg IS NULL)) ORDER BY created_time ASC LIMIT 0,1;";//每次都取還未嘗試超過100次的，防止資料卡住
            m_upload_dataDataTable = SQLDataTableModel.GetDataTable("Synchronize", SQL);
            if((m_upload_dataDataTable != null)&&(m_upload_dataDataTable.Rows.Count>0))
            {
                m_intUploadRowsCount = m_upload_dataDataTable.Rows.Count;
            }
            else
            {
                SQL = "UPDATE upload_data SET upload_msg='',try_count = (try_count + 1) WHERE upload_state='N' AND (LENGTH(upload_msg)>0) AND (try_count<100)";
                SQLDataTableModel.SQLiteInsertUpdateDelete("Synchronize", SQL);
            }
            return m_intUploadRowsCount;
        }

        private static int m_intSIDCount = 1;
        private static String SIDCreate()
        {
            bool blnrepeat = true;

            String StrResult = "";
            String StrNowDay = DateTime.Now.ToString("yyyyMMdd");

            do
            {               
                StrResult = String.Format("{0}{1:0000}", StrNowDay, m_intSIDCount);
                
                String SQL = String.Format("SELECT SID FROM upload_data WHERE SID='{0}' LIMIT 0,1;", StrResult);
                DataTable upload_dataDataTable = SQLDataTableModel.GetDataTable("Synchronize", SQL);
                if ((upload_dataDataTable != null) && (upload_dataDataTable.Rows.Count > 0))
                {
                    m_intSIDCount++;
                    upload_dataDataTable.Clear();
                    StrResult = "";
                    blnrepeat = true;
                }
                else
                {
                    blnrepeat = false;
                    break;
                }

            } while (m_intSIDCount < 9999);

            return StrResult;
        }

        public static void dataInsert(String upload_type,String data_no)
        {
            /*
            VTEAM_POS [upload_type] 類型:
            "NOD" => 新訂單(已結帳) -> order_state
            "DOD" => 取消訂單(未結帳) -> del_flag
            "COD" => 作廢訂單(已結帳後作廢) -> cancel_flag
            "CRP" => 交班報表 -> butt01_Click(object sender, EventArgs e)//營業關帳按鈕
            "DRP" => 關帳報表 -> butt01_Click(object sender, EventArgs e)//營業關帳按鈕
            "NEP" => 新增收支紀錄 -> PayFun_Click(object sender, EventArgs e)//支付方式按鈕事件
            "DEP" => 刪除收支紀錄 -> butt02_Click(object sender, EventArgs e)//刪除按鈕事件
            "ATTENDANCE" => 打卡紀錄
            "INV_B2C_SALE" => 電子發票交易資料 //資料結構 API: POS_Order_2_Invoice_B2C_Order
            "INV_B2C_CANCEL" => 電子發票作廢資料 //資料結構 API: POS_Order_2_Invoice_B2C_Order
            "INV_B2C_REPORT" => 電子發票日結資料 //資料結構 API: POS_Report_2_Invoice_B2C_Summary
             */
            String SID = SIDCreate();
            if((SID.Length>0) && (upload_type.Length>0) && (data_no.Length>0))
            {
                String SQL = String.Format("INSERT INTO upload_data (SID,upload_type,data_no,created_time,updated_time) VALUES ('{0}', '{1}','{2}','{3}','{3}');", SID, upload_type, data_no, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                SQLDataTableModel.SQLiteInsertUpdateDelete("Synchronize", SQL);
            }

        }

        public static void dataUpdate(String SID, String data_no,String upload_state,String upload_msg,int try_count)
        {
            String SQL = String.Format("UPDATE upload_data SET upload_state='{0}',upload_msg='{1}',try_count='{2}',upload_time='{3}',updated_time='{3}' WHERE SID='{4}' AND data_no='{5}';", upload_state, Cryption.Base64_encode(upload_msg), 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), SID, data_no);
            SQLDataTableModel.SQLiteInsertUpdateDelete("Synchronize", SQL);
        }
    }//SyncDBFun
}
