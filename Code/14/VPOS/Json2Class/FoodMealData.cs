using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class FMDAattribute
    {
        public int attribute_sid { get; set; }
        public string attribute_name { get; set; }
    }

    public class FMDCondiment
    {
        public int m_intImgId;//圖示
        public int m_intSID;//DB唯一值
        public String m_StrName;//名稱
        public double m_dblPrice;//價格
        public int m_intAmount;//數量
        public double m_dblSum;//總和
        public String m_Strcondiment_code;
        public int m_intitem_no;
        public int m_intparent_item_no;
        public String m_Strdel_flag;
        public String m_Strdel_time;

        public String m_Stritem_type;//類型

        //public int m_intdiscount_type;// 折扣/折讓 旗標
        //public String m_Strdiscount_name;// 折扣/折讓 說明文字
        //public String m_Strdiscount_code;
        //public int m_intdiscount_rate;// 折扣率
        //public int m_intdiscount_fee;// 折讓金額
        //public String m_Strdiscount_info; //JSON

        //public int m_inttax_sid; //稅率編號 ; 產品若沒有稅率資料[m_tax_sid=0]，就直接從company和tax_data取出預設值
        //public int m_inttax_rate; //稅率
        //public String m_Strtax_type;//稅率類型
        //public int m_inttax_fee; //稅率金額

        //public String m_Strsubtotal_flag;//小計旗標
        //public int m_intsubtotal_item_no;//執行小計的編號
        public FMDCondiment()
        {
            m_intImgId = -1;
            m_intSID = -1;
            m_StrName = "";
            m_dblPrice = 0.0;
            m_intAmount = 0;
            m_dblSum = 0.0;
            m_Strcondiment_code = "";
            m_intitem_no = -1;
            m_intparent_item_no = -1;
            m_Strdel_flag = "";
            m_Strdel_time = "";

            m_Stritem_type = "";//類型

            //m_intdiscount_type = -1;// 折扣/折讓 旗標
            //m_Strdiscount_name = "";// 折扣/折讓 說明文字
            //m_Strdiscount_code = "";
            //m_intdiscount_rate = 0;// 折扣率
            //m_intdiscount_fee = 0;// 折讓金額
            //m_Strdiscount_info = ""; //JSON

            //m_inttax_sid = -1; //稅率編號
            //m_inttax_rate = 0; //稅率
            //m_Strtax_type = "";//稅率類型
            //m_inttax_fee = 0; //稅率金額

            //m_Strsubtotal_flag = "N";//小計旗標
            //m_intsubtotal_item_no = 0;//執行小計的編號
        }
    }

    public class FMDProduct
    {
        public String m_Strmain_flag;//主要商品(Y:主要商品、N:可更換商品)
        public String m_Strdefault_flag;//預設商品旗標(Y:預設商品、N:非預設)
        //public int m_intImgId;//圖示
        public int m_intSID;//DB唯一值
        public String m_StrName;//名稱
        public double m_dblPrice;//價格
        public int m_intAmount;//數量
        public double m_dblSum;//總和

        //public String m_Stritem_type;//類型

        public String m_Strproduct_code;
        //public int m_intitem_no;
        public String m_Strdel_flag;
        public String m_Strdel_time;

        //public String m_Strdiscount_type;// 折扣/折讓 旗標
        //public String m_Strdiscount_name;// 折扣/折讓 說明文字
        //public String m_Strdiscount_code;
        //public int m_intdiscount_rate;// 折扣率
        //public int m_intdiscount_fee;// 折讓金額
        //public String m_Strdiscount_info; //JSON

        //public int m_inttax_sid; //稅率編號 ; 產品若沒有稅率資料[m_tax_sid=0]，就直接從company和tax_data取出預設值
        //public int m_inttax_rate; //稅率
        //public String m_Strtax_type;//稅率類型
        //public int m_inttax_fee; //稅率金額

        //public String m_Strsubtotal_flag;//小計旗標
        //public int m_intsubtotal_item_no;//執行小計的編號

        public List<FMDCondiment> m_ListCondiment;

        public FMDProduct()
        {
            m_Strmain_flag="Y";//主要商品(Y:主要商品、N:可更換商品)
            m_Strdefault_flag="Y";//預設商品旗標(Y:預設商品、N:非預設)
            //m_intImgId = -1;
            m_intSID = -1;
            m_StrName = "";
            m_dblPrice = 0.0;
            m_intAmount = 0;
            m_dblSum = 0.0;
            m_Strproduct_code = "";
            //m_intitem_no = -1;
            m_Strdel_flag = "";
            m_Strdel_time = "";

            //m_Stritem_type = "";//類型

            //m_Strdiscount_type = "N";// 折扣/折讓 旗標
            //m_Strdiscount_name = "";// 折扣/折讓 說明文字
            //m_Strdiscount_code = "";
            //m_intdiscount_rate = 0;// 折扣率
            //m_intdiscount_fee = 0;// 折讓金額
            //m_Strdiscount_info = ""; //JSON

            //m_inttax_sid = -1; //稅率編號
            //m_inttax_rate = 0; //稅率
            //m_Strtax_type = "";//稅率類型
            //m_inttax_fee = 0; //稅率金額

            //m_Strsubtotal_flag = "N";//小計旗標
            //m_intsubtotal_item_no = 0;//執行小計的編號

            m_ListCondiment = new List<FMDCondiment>();
        }
    }

    public class FMDElement//元素
    {
        /*
	    set_attribute_data:
		    1		558				主商品		C					0			0					B					5			0					Y					4			Y			0		2022-11-01 10:49:17.000	2022-11-01 10:49:17.000
		    1		559				主選		C					0			0					A					5			10					Y					2			Y			0		2022-11-01 10:52:59.000	2022-11-01 10:52:59.000		
	    *元素號		元素上層id		別名		預設商品計價設定	預設價格	最高加價			更換商品計價設定	預設價格	最高加價			必選旗標(Y、N)		必選數量	重複選擇	排序
	    預設商品計價規則(A:差價計算、B固定金額、C:不加價)
	    可更換商品計價規則(A:差價計算、B固定金額、C:不加價)
        */

        public int m_intSID;
        public String m_Strattribute_name;
        public String m_Strmain_price_type;//預設商品計價規則(A:差價計算、B固定金額、C:不加價)
        public double m_dblmain_price;
        public double m_dblmain_max_price;
        public string m_Strsub_price_type;//可更換商品計價規則(A:差價計算、B固定金額、C:不加價)
        public double m_dblsub_price;
        public double m_dblsub_max_price;
        public String m_Strrequired_flag;
        public int m_intlimit_count;
        public String m_Strrepeat_flag;
        public int m_intsort;
        public List<FMDProduct> m_ListProduct;
        public FMDElement()
        {
            m_Strattribute_name = "";
            m_Strmain_price_type="C";
            m_dblmain_price = 0;
            m_dblmain_max_price = 0;
            m_Strsub_price_type = "C";
            m_dblsub_price = 0;
            m_dblsub_max_price = 0;
            m_Strrequired_flag = "N";
            m_intlimit_count = 0;
            m_Strrepeat_flag = "N";
            m_ListProduct=new List<FMDProduct>();
        }
    }

    public class FoodMealData//套餐
    {
        /*
		558	7	FatMan		肥宅套餐		T	F	300	0	0	0		N	1900-01-01	N	1900-01-01	2022-10-31 14:33:25.000	2022-10-31 14:33:25.000	2022-10-31 14:33:25.000			2022-10-31 11:45:43.000	2022-10-31 14:33:25.000
		559	7	HealthCare	養生飲套餐		T	F	300	0	0	0		N	1900-01-01	N	1900-01-01	2022-11-01 10:52:59.000	2022-11-01 10:52:59.000	2022-11-01 10:52:59.000			2022-11-01 10:52:59.000	2022-11-01 10:52:59.000
		*id							product_type	價格 
        */
        public int m_intSid;
        public String m_StrCode;
        public int m_intAmount;
        public double m_dblPrice;
        public double m_dblAdds;
        public double m_dblSum;
        public String m_StrName;
        public List<FMDElement> m_ListElement;//元素
        public FoodMealData()
        {
            m_intAmount = 1;
            m_intSid = 0;
            m_dblPrice = 0;
            m_dblAdds = 0;
            m_dblSum = (m_dblPrice + m_dblAdds) * m_intAmount;
            m_StrCode = "";
            m_StrName = "";
            m_ListElement=new List<FMDElement>();
        }
        public double CalculateSubtotal()
        {
            double dblAllMainPrice = 0;
            double dblAllSubPrice = 0;
            double dblAllCMainPrice = 0;
            double dblAllCSubPrice = 0;
            m_dblAdds = 0;

            for (int i = 0; i < m_ListElement.Count; i++)
            {
                dblAllMainPrice = 0;
                dblAllSubPrice = 0;
                dblAllCMainPrice = 0;
                dblAllCSubPrice = 0;

                if (m_ListElement[i] != null)
                {
                    //---
                    //產品計算
                    for(int j=0;j< m_ListElement[i].m_ListProduct.Count;j++)
                    {
                        if (m_ListElement[i].m_ListProduct[j].m_Strdel_flag!="Y")
                        {
                            if(m_ListElement[i].m_ListProduct[j].m_Strmain_flag=="Y")
                            {
                                dblAllMainPrice += m_ListElement[i].m_ListProduct[j].m_dblPrice;
                                dblAllCMainPrice += m_ListElement[i].m_ListProduct[j].m_dblSum - m_ListElement[i].m_ListProduct[j].m_dblPrice;
                                //dblAllMainPrice += m_ListElement[i].m_ListProduct[j].m_dblSum;
                            }
                            else
                            {
                                dblAllSubPrice += m_ListElement[i].m_ListProduct[j].m_dblPrice;
                                dblAllCSubPrice += m_ListElement[i].m_ListProduct[j].m_dblSum - m_ListElement[i].m_ListProduct[j].m_dblPrice;
                                //dblAllSubPrice += m_ListElement[i].m_ListProduct[j].m_dblSum;
                            }
                        }
                    }
                    //---產品計算
                    
                    if((m_ListElement[i].m_Strmain_price_type=="A") && (m_ListElement[i].m_dblmain_max_price>0))//A:差價計算
                    {
                        dblAllMainPrice = (dblAllMainPrice > m_ListElement[i].m_dblmain_max_price) ? m_ListElement[i].m_dblmain_max_price : dblAllMainPrice;
                    }
                    else if((m_ListElement[i].m_Strmain_price_type == "B") && (m_ListElement[i].m_dblmain_max_price > 0))//B固定金額
                    {
                        dblAllMainPrice = (dblAllMainPrice > m_ListElement[i].m_dblmain_max_price) ? m_ListElement[i].m_dblmain_max_price : dblAllMainPrice;
                    }
                    else//C:不加價
                    {
                        dblAllMainPrice = dblAllMainPrice;
                    }

                    if ((m_ListElement[i].m_Strsub_price_type == "A") && (m_ListElement[i].m_dblsub_max_price>0))//A:差價計算
                    {
                        dblAllSubPrice = (dblAllSubPrice > m_ListElement[i].m_dblsub_max_price) ? m_ListElement[i].m_dblsub_max_price : dblAllSubPrice;
                    }
                    else if ((m_ListElement[i].m_Strsub_price_type == "B") && (m_ListElement[i].m_dblsub_max_price > 0))//B固定金額
                    {
                        dblAllSubPrice = (dblAllSubPrice > m_ListElement[i].m_dblsub_max_price) ? m_ListElement[i].m_dblsub_max_price : dblAllSubPrice;
                    }
                    else//C:不加價
                    {
                        dblAllSubPrice = dblAllSubPrice;
                    }
                }

                //m_dblAdds += (dblAllMainPrice + dblAllSubPrice);
                m_dblAdds += (dblAllMainPrice + dblAllSubPrice + dblAllCMainPrice + dblAllCSubPrice);
            }

            m_dblSum = (m_dblPrice + m_dblAdds) * m_intAmount;

            return m_dblSum;
        }
    }

}
