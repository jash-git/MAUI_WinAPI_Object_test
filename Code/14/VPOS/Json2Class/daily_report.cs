using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    /*
    報表json格式 從測試機DB(DB_DATABASE=vteam-cloud-db DB_TABLE=upload_queue_data)取得
	{
		"version": "1.6.5.4",
		"device_code": "",
		"report_no": "DR-202302004",
		"report_time": 1675388227,
		"business_day": 1675353600,
		"class_name": "早班",
		"employee_no": "vteam-1",
		"order_start_time": 1675388169,
		"order_end_time": 1675388189,
		"order_count": 1,
		"discount_total": 0,
		"promotion_total": 0,
		"coupon_total": 0,
		"tax_total": 2,
		"service_total": 0,
		"stock_push_amount": 0,
		"stock_pull_amount": 0,
		"sale_total": 45,
		"sale_amount": 45,
		"sale_item_count": 1,
		"sale_item_avg_cost": 45,
		"payment_cash_total": 45,
		"expense_cash_total": 60,
		"trans_reversal": 0,
		"over_paid": 0,
		"cash_total": 45,
		"cancel_count": 0,
		"cancel_total": 0,
		"other_cancel_count": 0,
		"other_cancel_total": 0,
		"refund_cash_total": 0,
		"payment_info": [
			{
				"payment_sid": 1,
				"payment_code": "CASH",
				"payment_name": "現金",
				"payment_amount": 45,
				"total_count": 1
			}
		],
		"coupon_info": [],
		"expense_info": [
			{
				"account_code": "PETTY",
				"account_name": "零用金收入",
				"account_type": "R",
				"payment_code": "CASH",
				"payment_name": "現金",
				"money": 100
			},
			{
				"account_code": "A",
				"account_name": "水電費用",
				"account_type": "E",
				"payment_code": "CASH",
				"payment_name": "現金",
				"money": 50
			},
			{
				"account_code": "B",
				"account_name": "進貨費用",
				"account_type": "E",
				"payment_code": "CASH",
				"payment_name": "現金",
				"money": 10
			},
			{
				"account_code": "Z",
				"account_name": "回收物變賣",
				"account_type": "R",
				"payment_code": "CASH",
				"payment_name": "現金",
				"money": 20
			}
		],
		"inv_summery_info": {
			"version": "1.6.5.4",
			"device_code": "",
			"business_id": "28537502",
			"branch_no": "001",
			"pos_no": "001",
			"total_upload_inv": 1,
			"total_upload_cancel_inv": 0,
			"sale_quantity": 1,
			"sale_amount": 45,
			"cancel_quantity": 0,
			"cancel_amount": 0,
			"details": [
				{
					"inv_type": 1,
					"period": "11202",
					"track": "LC",
					"begin_no": "10028510",
					"end_no": "10028510",
					"quantity": 1,
					"amount": 45
				}
			]
		},
		"report_type": "D",
		"company_sid": 7,
		"terminal_sid": "VT-POS-2020-00002"
	}
    */
    public class DRDetail
    {
        public int inv_type { get; set; }
        public string period { get; set; }
        public string track { get; set; }
        public string begin_no { get; set; }
        public string end_no { get; set; }
        public int quantity { get; set; }
        public int amount { get; set; }
    }

    public class DRExpenseInfo
    {
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string account_type { get; set; }
        public string payment_code { get; set; }
        public string payment_name { get; set; }
        public int money { get; set; }
    }

    public class DRInvSummeryInfo
    {
        public string report_no { get; set; }//電子發票用
        public long report_time { get; set; }//電子發票用
        public string pos_ver { get; set; }//version
        public string device_code { get; set; }
        public string business_id { get; set; }
        public string branch_no { get; set; }
        public string pos_no { get; set; }
        public int total_upload_inv { get; set; }
        public int total_upload_cancel_inv { get; set; }
        public int sale_quantity { get; set; }
        public int sale_amount { get; set; }
        public int cancel_quantity { get; set; }
        public int invalid_quantity { get; set; }//電子發票用
        public int cancel_amount { get; set; }
        public int invalid_amount { get; set; }//電子發票用

        public List<DRDetail> details { get; set; }
        public DRInvSummeryInfo()
        {
            details = new List<DRDetail>();
        }
    }

    public class DRPaymentInfo
    {
        public int payment_sid { get; set; }
        public string payment_code { get; set; }
        public string payment_name { get; set; }
        public int payment_amount { get; set; }
        public int total_count { get; set; }
    }

    public class DRCouponInfo
    {
        public string coupon_issuer { get; set; }
        public string coupon_name { get; set; }
        public int coupon_amount { get; set; }
        public int total_count { get; set; }
    }

    public class DRCategorySalesStatistics
    {
        public String category_code { get; set; }
        public String category_name { get; set; }
        public int sort { get; set; }
        public int discount_fee { get; set; }
        public int quantity { get; set; }
        public int subtotal { get; set; }
        public int amount { get; set; }
    }

    public class DRPromotions_Info
    {
        public string type { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public int amount { get; set; }
    }

    public class daily_report
    {
        public string store_name { get; set; }

        public string pos_ver { get; set; }//version
        public string device_code { get; set; }
        public string report_no { get; set; }
        public long report_time { get; set; }
        public long business_day { get; set; }
        public string class_name { get; set; }
        public string employee_no { get; set; }
        public long order_start_time { get; set; }
        public long order_end_time { get; set; }
        public int order_count { get; set; }
        public int discount_total { get; set; }
        public int promotion_total { get; set; }
        public int coupon_total { get; set; }
        public int tax_total { get; set; }
        public int service_total { get; set; }
        public int stock_push_amount { get; set; }
        public int stock_pull_amount { get; set; }
        public int sale_total { get; set; }
        public int sale_amount { get; set; }
        public int sale_item_count { get; set; }
        public int sale_item_avg_cost { get; set; }
        public int payment_cash_total { get; set; }
        public int expense_cash_total { get; set; }
        public int trans_reversal { get; set; }
        public int over_paid { get; set; }
        public int cash_total { get; set; }
        public int cancel_count { get; set; }
        public int cancel_total { get; set; }
        public int other_cancel_count { get; set; }
        public int other_cancel_total { get; set; }
        public int refund_cash_total { get; set; }
        public List<DRPaymentInfo> payment_info { get; set; }//支付方式資料結構
        public List<DRCouponInfo> coupon_info { get; set; }//優惠/兌換券資料結構
        public List<DRExpenseInfo> expense_info { get; set; }//收支紀錄資料結構
        public DRInvSummeryInfo inv_summery_info { get; set; }//發票資料結構
        public List<DRCategorySalesStatistics> category_sale_info { get; set; }//商品類別銷售統計
        public List<DRPromotions_Info> promotions_info { get; set; }//優惠資料統計的內容
        public string report_type { get; set; }
        public int company_sid { get; set; }
        public string terminal_sid { get; set; }
        public daily_report()
        {
            category_sale_info = new List<DRCategorySalesStatistics>();
            inv_summery_info = new DRInvSummeryInfo();
            expense_info = new List<DRExpenseInfo>();
            coupon_info = new List<DRCouponInfo>();
            payment_info = new List<DRPaymentInfo>();
            promotions_info = new List<DRPromotions_Info>();
        }
    }

    public class DB2daily_report
	{
        public static void daily_report2Var(String data_no, ref daily_report daily_reportBuf)
		{
            String SQL = "";

            SQL = "SELECT SID,company_name FROM company LIMIT 0,1";
            DataTable companyDataTable = SQLDataTableModel.GetDataTable(SQL);
            if (companyDataTable.Rows.Count > 0)
            {
                daily_reportBuf.store_name = companyDataTable.Rows[0]["company_name"].ToString();
            }

            SQL = String.Format("SELECT * FROM daily_report WHERE report_no='{0}' LIMIT 0,1", data_no);
            DataTable daily_reportDataTable = SQLDataTableModel.GetDataTable(SQL);
            if (daily_reportDataTable.Rows.Count > 0)
            {
                //---
                //設定daily_report物件
                daily_reportBuf.pos_ver = MainPage.m_StrVersion;
                daily_reportBuf.device_code = MainPage.m_StrDeviceCode;
                daily_reportBuf.report_no = daily_reportDataTable.Rows[0]["report_no"].ToString();
                daily_reportBuf.report_time = TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(daily_reportDataTable.Rows[0]["report_time"].ToString()));
                daily_reportBuf.business_day = TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(daily_reportDataTable.Rows[0]["business_day"].ToString()));
                daily_reportBuf.class_name = daily_reportDataTable.Rows[0]["class_name"].ToString();
                daily_reportBuf.employee_no = daily_reportDataTable.Rows[0]["employee_no"].ToString();
                daily_reportBuf.order_start_time = TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(daily_reportDataTable.Rows[0]["order_start_time"].ToString()));
                daily_reportBuf.order_end_time = TimeConvert.DateTimeToUnixTimeStamp(Convert.ToDateTime(daily_reportDataTable.Rows[0]["order_end_time"].ToString()));
                daily_reportBuf.order_count = Int32.Parse(daily_reportDataTable.Rows[0]["order_count"].ToString());
                daily_reportBuf.discount_total = Int32.Parse(daily_reportDataTable.Rows[0]["discount_total"].ToString());
                daily_reportBuf.promotion_total = Int32.Parse(daily_reportDataTable.Rows[0]["promotion_total"].ToString());
                daily_reportBuf.coupon_total = Int32.Parse(daily_reportDataTable.Rows[0]["coupon_total"].ToString());
                daily_reportBuf.tax_total = Int32.Parse(daily_reportDataTable.Rows[0]["tax_total"].ToString());
                daily_reportBuf.service_total = Int32.Parse(daily_reportDataTable.Rows[0]["service_total"].ToString());
                daily_reportBuf.stock_push_amount = Int32.Parse(daily_reportDataTable.Rows[0]["stock_push_amount"].ToString());
                daily_reportBuf.stock_pull_amount = Int32.Parse(daily_reportDataTable.Rows[0]["stock_pull_amount"].ToString());
                daily_reportBuf.sale_total = Int32.Parse(daily_reportDataTable.Rows[0]["sale_total"].ToString());
                daily_reportBuf.sale_amount = Int32.Parse(daily_reportDataTable.Rows[0]["sale_amount"].ToString());
                daily_reportBuf.sale_item_count = Int32.Parse(daily_reportDataTable.Rows[0]["sale_item_count"].ToString());
                daily_reportBuf.sale_item_avg_cost = Int32.Parse(daily_reportDataTable.Rows[0]["sale_item_avg_cost"].ToString());
                daily_reportBuf.payment_cash_total = Int32.Parse(daily_reportDataTable.Rows[0]["payment_cash_total"].ToString());
                daily_reportBuf.expense_cash_total = Int32.Parse(daily_reportDataTable.Rows[0]["expense_cash_total"].ToString());
                daily_reportBuf.trans_reversal = Int32.Parse(daily_reportDataTable.Rows[0]["trans_reversal"].ToString());
                daily_reportBuf.over_paid = Int32.Parse(daily_reportDataTable.Rows[0]["over_paid"].ToString());
                daily_reportBuf.cash_total = Int32.Parse(daily_reportDataTable.Rows[0]["cash_total"].ToString());
                daily_reportBuf.cancel_count = Int32.Parse(daily_reportDataTable.Rows[0]["cancel_count"].ToString());
                daily_reportBuf.cancel_total = Int32.Parse(daily_reportDataTable.Rows[0]["cancel_total"].ToString());
                daily_reportBuf.other_cancel_count = Int32.Parse(daily_reportDataTable.Rows[0]["other_cancel_count"].ToString());
                daily_reportBuf.other_cancel_total = Int32.Parse(daily_reportDataTable.Rows[0]["other_cancel_total"].ToString());
                daily_reportBuf.refund_cash_total = Int32.Parse(daily_reportDataTable.Rows[0]["refund_cash_total"].ToString());
                //支付合計列表
                daily_reportBuf.payment_info = JsonClassConvert.DRPaymentInfo2Class(daily_reportDataTable.Rows[0]["payment_info"].ToString());
                //收支項目列表
                daily_reportBuf.expense_info = JsonClassConvert.DRExpenseInfo2Class(daily_reportDataTable.Rows[0]["expense_info"].ToString());
                daily_reportBuf.report_type = daily_reportDataTable.Rows[0]["report_type"].ToString();
                daily_reportBuf.company_sid = Int32.Parse(SqliteDataAccess.m_terminal_data[0].company_sid);
                daily_reportBuf.terminal_sid = SqliteDataAccess.m_terminal_data[0].SID;
                //優惠/兌換券
                daily_reportBuf.coupon_info = JsonClassConvert.DRCouponInfo2Class(daily_reportDataTable.Rows[0]["coupon_info"].ToString());
                //發票資訊
                daily_reportBuf.inv_summery_info = JsonClassConvert.DRInvSummeryInfo2Class(daily_reportDataTable.Rows[0]["inv_summery_info"].ToString());
                //商品類別銷售統計
                daily_reportBuf.category_sale_info = JsonClassConvert.DRCategorySalesStatistics2Class(daily_reportDataTable.Rows[0]["category_sale_info"].ToString());
                //優惠資料統計的內容
                daily_reportBuf.promotions_info = JsonClassConvert.DRPromotionsInfo2Class(daily_reportDataTable.Rows[0]["promotions_info"].ToString());
                //---設定daily_report物件
            }

        }

    }

}
