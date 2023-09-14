using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace VPOS
{
    /*
    新增 JSON:(expense_new)
    {
	    "expense_no": "2023020800001",
	    "action_time": 1675838076,
	    "action_user": "admin",
	    "account_code": "PETTY",
	    "account_name": "零用金收入",
	    "account_type": "R",
	    "money": 100,
	    "payment_code": "CASH",
	    "payment_name": "現金",
	    "remark": null,
	    "del_flag": "N",
	    "del_time": 0,
	    "data_type": "NEP",
	    "company_sid": 7,
	    "terminal_sid": "VT-POS-2020-00002"
    }

    刪除 JSON:(expense_cancel)
    {
        "expense_no": "2023020800001",
	    "del_flag": "Y",
	    "del_time": 1675838355,
	    "data_type": "DEP",
	    "company_sid": 7,
	    "terminal_sid": "VT-POS-2020-00002"
    }

    API:
	    upload/expense/{{status}}  
		    status = new -> 新增
		    status = cancel -> 刪除
    */

    public class expense_new
    {
        public string expense_no { get; set; }
        public int action_time { get; set; }
        public string action_user { get; set; }
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string account_type { get; set; }
        public int money { get; set; }
        public string payment_code { get; set; }
        public string payment_name { get; set; }
        public object remark { get; set; }
        public string del_flag { get; set; }
        public int del_time { get; set; }
        public string data_type { get; set; }
        public int company_sid { get; set; }
        public string terminal_sid { get; set; }
    }

    public class expense_cancel
    {
        public string expense_no { get; set; }
        public string del_flag { get; set; }
        public int del_time { get; set; }
        public string data_type { get; set; }
        public int company_sid { get; set; }
        public string terminal_sid { get; set; }
    }

    public class DB2expense_new
    {
        public static void expense_new2Var(String data_no, ref expense_new expense_newBuf)
        {
            String SQL = $"SELECT * FROM expense_data WHERE expense_no= '{data_no}'";
            DataTable expense_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
            if (expense_dataDataTable.Rows.Count > 0)
            {
                expense_newBuf.expense_no = data_no;
                expense_newBuf.action_time = Convert.ToInt32(expense_dataDataTable.Rows[0]["action_time"].ToString());
                expense_newBuf.action_user = expense_dataDataTable.Rows[0]["action_user"].ToString();
                expense_newBuf.account_code = expense_dataDataTable.Rows[0]["account_code"].ToString();
                expense_newBuf.account_name = expense_dataDataTable.Rows[0]["account_name"].ToString();
                expense_newBuf.account_type = expense_dataDataTable.Rows[0]["account_type"].ToString();
                expense_newBuf.money = Convert.ToInt32(expense_dataDataTable.Rows[0]["money"].ToString()); ;
                expense_newBuf.payment_code = expense_dataDataTable.Rows[0]["payment_code"].ToString();
                expense_newBuf.payment_name = expense_dataDataTable.Rows[0]["payment_name"].ToString();
                expense_newBuf.remark = expense_dataDataTable.Rows[0]["remark"].ToString();
                expense_newBuf.del_flag = expense_dataDataTable.Rows[0]["del_flag"].ToString();
                expense_newBuf.del_time = Convert.ToInt32(expense_dataDataTable.Rows[0]["del_time"].ToString());
                expense_newBuf.data_type = "NEP";
                expense_newBuf.company_sid = Int32.Parse(SqliteDataAccess.m_terminal_data[0].company_sid);
                expense_newBuf.terminal_sid = SqliteDataAccess.m_terminal_data[0].SID;
            }
        }
    }

    public class DB2expense_cancel
    {
        public static void expense_cancel2Var(String data_no, ref expense_cancel expense_cancelBuf)
        {
            String SQL = $"SELECT * FROM expense_data WHERE expense_no= '{data_no}'";
            DataTable expense_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
            if (expense_dataDataTable.Rows.Count > 0)
            {
                expense_cancelBuf.expense_no = data_no;
                expense_cancelBuf.del_flag = expense_dataDataTable.Rows[0]["del_flag"].ToString();
                expense_cancelBuf.del_time = Convert.ToInt32(expense_dataDataTable.Rows[0]["del_time"].ToString());
                expense_cancelBuf.data_type = "DEP";
                expense_cancelBuf.company_sid = Int32.Parse(SqliteDataAccess.m_terminal_data[0].company_sid);
                expense_cancelBuf.terminal_sid = SqliteDataAccess.m_terminal_data[0].SID;
            }
        }
    }
}