namespace VPOS
{
    public class inv_url
    {
        public string api_url { get; set; }
        public string sandbox_url { get; set; }
    }

    public class inv_result
    {
        public string Code { get; set; }
        public string Msg { get; set; }
    }
    public class inv_params//電子發票參數資訊
    {
        public string Platform_Code { get; set; }
        public string Platform_Name { get; set; }
        public string Platform_Params { get; set; }
        public string Sandbox { get; set; }
        public string Client_Id { get; set; }//terminal_data中的[client_id]欄位
        public string Client_Secret { get; set; }//terminal_data中的[client_secret]欄位
        public string Business_Id { get; set; }
        public string Branch_No { get; set; }
        public string Reg_Id { get; set; }
        public string Pos_Id { get; set; }
        public string Pos_No { get; set; }
        public string QRCode_AES_Key { get; set; }
        public string Operator_Name { get; set; }
        public string Operator_Id { get; set; }
        public int Inv_Renew_Count { get; set; }
        public int Booklet { get; set; }
        public string Terminal_SID { get; set; }
        public int Last_Batch_Num { get; set; }

    }
}
