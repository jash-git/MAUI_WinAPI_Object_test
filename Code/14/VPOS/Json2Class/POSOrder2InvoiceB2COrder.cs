using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class POIBOItem
    {
        public int Sequence_Num { get; set; }
        public string Product_Code { get; set; }
        public string Product_Name { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public int Tax_Type { get; set; }
        public double Tax_Rate { get; set; }
        public int Tax_Fee { get; set; }
        public string Remark { get; set; }
    }
    public class POSOrder2InvoiceB2COrder
    {
        public string Format_Ver { get; set; }
        public string Platform_Code { get; set; }
        public string Sandbox { get; set; }
        public string Business_Id { get; set; }
        public string Branch_No { get; set; }
        public string Reg_Id { get; set; }
        public string Pos_Id { get; set; }
        public string Pos_No { get; set; }
        public string PName { get; set; }
        public string Operator_Id { get; set; }
        public string Operator_Name { get; set; }
        public string Order_No { get; set; }
        public string Period { get; set; }
        public string Track { get; set; }
        public string Inv_No { get; set; }
        public int Inv_Time { get; set; }
        public string Inv_Type { get; set; }
        public string Buyer_Vat_Id { get; set; }
        public string Main_Remark { get; set; }
        public string Donate_Mark { get; set; }
        public string Donate_Code { get; set; }
        public string Carrier_Type { get; set; }
        public string Carrier_Code_1 { get; set; }
        public string Carrier_Code_2 { get; set; }
        public int Batch_Num { get; set; }
        public string Random_Code { get; set; }
        public int Sale_Amount { get; set; }
        public int Free_Tax_Sale_Amount { get; set; }
        public int Zero_Tax_Sale_Amount { get; set; }
        public int Tax_Type { get; set; }
        public double Tax_Rate { get; set; }
        public int Tax_Amount { get; set; }
        public int Total_Amount { get; set; }
        public int Item_Count { get; set; }
        public List<POIBOItem> Items { get; set; }
        public string Customs_Clearance_Marker_Num { get; set; }
        public string Print_Mark { get; set; }
        public string QRCode_Value_1 { get; set; }
        public string QRCode_Value_2 { get; set; }
        public string BarCode_Value { get; set; }
        public string Invalid_Flag { get; set; }
        public string Ret_Code { get; set; }
        public string Ret_Msg { get; set; }

        public POSOrder2InvoiceB2COrder()
        {
            Items=new List<POIBOItem>();
        }
    }
}
