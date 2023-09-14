using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class orders_cancel
    {
        public string order_no { get; set; }
        public int cancel_time { get; set; }
        public string cancel_class_name { get; set; }
        public string refund { get; set; }
        public string refund_type_sid { get; set; }
        public int company_sid { get; set; }
        public string terminal_sid { get; set; }
        public string pos_no { get; set; }
        public string employee_no { get; set; }
        public string license_type { get; set; }
    }

    public class DB2orders_cancel
    {
        public static void order_data2Var(String data_no, ref orders_cancel orders_cancelBuf)
        {
            String SQL = "";
            SQL = String.Format("SELECT * FROM order_data WHERE order_no='{0}' LIMIT 0,1", data_no);
            DataTable order_dataDataTable = SQLDataTableModel.GetDataTable(SQL);

            try
            {
                if (order_dataDataTable.Rows.Count > 0)
                {
                    orders_cancelBuf.cancel_class_name = order_dataDataTable.Rows[0]["class_name"].ToString();//班別
                    orders_cancelBuf.cancel_time = (int)TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(order_dataDataTable.Rows[0]["cancel_time"].ToString()));
                    orders_cancelBuf.employee_no = order_dataDataTable.Rows[0]["employee_no"].ToString();
                    orders_cancelBuf.order_no = data_no;
                    orders_cancelBuf.pos_no = order_dataDataTable.Rows[0]["pos_no"].ToString();
                    orders_cancelBuf.terminal_sid = order_dataDataTable.Rows[0]["terminal_sid"].ToString();
                    orders_cancelBuf.refund = "0";
                    orders_cancelBuf.refund_type_sid = "0";
                    orders_cancelBuf.license_type = "POS";
                    orders_cancelBuf.company_sid = Convert.ToInt32(SqliteDataAccess.m_terminal_data[0].company_sid);
                }
            }
            catch(Exception ex)
            {
                LogFile.Write("DB2orders_cancel.order_data2Var ERROR ; " + ex.ToString());
            }

        }
    }//DB2orders_cancel
}
