using System;
using System.Data;

namespace VPOS
{
    public class Tax_fun//稅金計算
    {
        public static double calculate(double dblprice, double dbltax_rate, int intMode)
        {
            double dblResult = 0;
            switch (intMode)
            {
                case 0:
                    dblResult = dblprice * dbltax_rate / 100;
                    break;
            }
            return Math.Round(dblResult, MidpointRounding.AwayFromZero);//四捨五入
        }
    }

    public class Discount_fun//折扣讓計算
    {
        public static double calculate(double dblprice, double dbldiscount_rate, int intMode,String StrCalcType)
        {
            double dblResult = 0;
            switch (intMode)
            {
                case 0:
                    dblResult = dblprice * (100-dbldiscount_rate) / 100;//折扣後商品價格
                    break;
                case 1:
                    dblResult = dblprice;//折讓的金額
                    break;
            }

            if(StrCalcType=="S")//店家有利 - 先四捨五入之後再計算
            {
                //30*0.75(75折)=22.5元 -> 23元
                //折扣金額=30-23=7元
                dblResult = dblprice - Math.Round(dblResult, MidpointRounding.AwayFromZero);
            }
            else//客戶有利 - 先計算之後再四捨五入
            {
                //30*0.75(75折)=22.5元
                //折扣金額 = 30-22.5=7.5 -> 8元
                dblResult = Math.Round((dblprice-dblResult), MidpointRounding.AwayFromZero);
            }

            return dblResult;
        }
    }

    public class Service_fun//服務費計算
    {
        public static double calculate(double dblprice, double dbldiscount_rate, int intMode)
        {
            double dblResult = 0;
            switch (intMode)
            {
                case 0:
                    //店家有利 - 先四捨五入之後再計算
                    //30*0.75(75折)=22.5元 -> 23元
                    //折扣金額=30-23=7元
                    dblResult = dblprice * (100 - dbldiscount_rate) / 100;//折扣後商品價格
                    dblResult = dblprice - Math.Round(dblResult, MidpointRounding.AwayFromZero);
                    break;
                case 1:
                    dblResult = dblprice;
                    break;
            }
            return dblResult;//四捨五入
        }
    }

    public class Promotion_fun//促銷計算
    {
        public static String m_StrOrderNo = "";
        public static List<discount_custom_target> m_discount_custom_targetList=new List<discount_custom_target>();//個別產品的對應數值

        public static void TriggerTypeAmount(int index,ref double[,] dblResult)//觸發金額依據(trigger_amount_result): 訂單總額(total) / 折扣後金額(single)
        {
            String SQL = String.Format("SELECT subtotal,discount_fee,promotion_fee,amount FROM order_data WHERE order_no='{0}' LIMIT 0,1", m_StrOrderNo);    
            DataTable order_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
            if (order_dataDataTable.Rows.Count > 0)
            {
                if(SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.trigger_amount_result=="Y") //折扣後金額
                {
                    dblResult[0, 0] = Convert.ToDouble(order_dataDataTable.Rows[0][0].ToString()) - Convert.ToDouble(order_dataDataTable.Rows[0][1].ToString());
                }
                else//N 訂單總額
                {
                    dblResult[0, 0] = Convert.ToDouble(order_dataDataTable.Rows[0][0].ToString());
                }

                order_dataDataTable.Rows.Clear();
                order_dataDataTable = null;
            }
        }

        public static void DiscountTarget(int index)//折扣對象(discount_type): product/custom
        {
            double dblBuf = 0;
            m_discount_custom_targetList.Clear();
            String SQL = "";
            if (SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.discount_target == "custom")
            {
                String StrProductSID = "";
                for (int i = 0; i < SQLDataTableModel.m_promotion_join_dataList[index].m_ListProductID.Count; i++)
                {
                    if (i == 0)
                    {
                        StrProductSID += "'" + SQLDataTableModel.m_promotion_join_dataList[index].m_ListProductID[i] + "'";
                    }
                    else
                    {
                        StrProductSID += "," + "'" + SQLDataTableModel.m_promotion_join_dataList[index].m_ListProductID[i] + "'";
                    }
                }

                if (StrProductSID.Length > 0)
                {
                    SQL = String.Format("SELECT item_cost,(item_cost+condiment_price) AS total,item_sid,item_count FROM order_content_data WHERE order_no='{0}' AND del_flag='N' AND (item_type='P' OR item_type='T') AND (parent_item_no='0')  AND (item_sid IN ({1})) ORDER BY item_cost,total ", m_StrOrderNo, StrProductSID);
                }
                else
                {
                    SQL = String.Format("SELECT item_cost,(item_cost+condiment_price) AS total,item_sid,item_count FROM order_content_data WHERE order_no='{0}' AND del_flag='N' AND (item_type='P' OR item_type='T') AND (parent_item_no='0') ORDER BY item_cost,total ", m_StrOrderNo);
                }
            }
            else
            {
                SQL = String.Format("SELECT item_cost,(item_cost+condiment_price) AS total,item_sid,item_count FROM order_content_data WHERE order_no='{0}' AND del_flag='N' AND (item_type='P' OR item_type='T') AND (parent_item_no='0') ORDER BY item_cost,total ", m_StrOrderNo);
            }
            
            DataTable order_content_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
            if (order_content_dataDataTable.Rows.Count > 0)
            {
                for (int i = 0; i < order_content_dataDataTable.Rows.Count;i++)
                {
                    if (SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.include_condiment == "Y")//包含配料金額計算
                    {
                        dblBuf = Convert.ToDouble(order_content_dataDataTable.Rows[i][1].ToString());
                    }
                    else//N - 不包含配料金額計算
                    {
                        dblBuf = Convert.ToDouble(order_content_dataDataTable.Rows[i][0].ToString());
                    }

                    for (int j = 0; j < Convert.ToInt32(order_content_dataDataTable.Rows[i][3].ToString()); j++)
                    {
                        discount_custom_target discount_custom_targetBuf = new discount_custom_target();
                        discount_custom_targetBuf.sid = Convert.ToInt32(order_content_dataDataTable.Rows[i][2].ToString());
                        discount_custom_targetBuf.count = 1;
                        discount_custom_targetBuf.value = dblBuf;
                        discount_custom_targetBuf.cost = Convert.ToDouble(order_content_dataDataTable.Rows[i][0].ToString());
                        discount_custom_targetBuf.total = Convert.ToDouble(order_content_dataDataTable.Rows[i][1].ToString());

                        m_discount_custom_targetList.Add(discount_custom_targetBuf);
                    }                    
                }

                order_content_dataDataTable.Rows.Clear();
                order_content_dataDataTable = null;
            }
        }

        public static double DiscountValue(int index, String StrCalcType, int intSID = 0)//折扣數(discount_value)
        {
            double dblResult = 0;//折扣金額
            //X件(100 - Y) / 100折
            int intX = ((SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.trigger_value!=null) && (SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.trigger_value.Length>0)) ? Convert.ToInt32(SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.trigger_value) : 0;//條件數量
            int intY = ((SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.discount_value!=null) && (SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.discount_value.Length>0)) ? Convert.ToInt32(SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.discount_value) : 0;//折扣數
            int intDiscountCustomNum = ((SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.discount_custom_num != null) && (SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.discount_custom_num.Length > 0)) ? Convert.ToInt32(SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.discount_custom_num) : intX;//運算數量
            Double dblSumPrice = 0;//X件商品總金額

            if(!((intX>0)&&(intY>0)))
            {
                return dblSumPrice;
            }

            for (int i = 0; i < m_discount_custom_targetList.Count; i++)
            {
                if (m_discount_custom_targetList[i].count == 1)
                {
                    if (intSID != 0)
                    {
                        if (m_discount_custom_targetList[i].sid == intSID)
                        {
                            intX--;
                            if(intDiscountCustomNum>0)
                            {
                                dblSumPrice += m_discount_custom_targetList[i].value;
                                intDiscountCustomNum--;
                            }                            
                            m_discount_custom_targetList[i].count = 0;
                            if(intX<=0)
                            {
                                break;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        intX--;
                        if (intDiscountCustomNum > 0)
                        {
                            dblSumPrice += m_discount_custom_targetList[i].value;
                            intDiscountCustomNum--;
                        }
                        m_discount_custom_targetList[i].count = 0;
                        if (intX <= 0)
                        {
                            break;
                        }
                    }
                }//if (m_discount_custom_targetList[i].count == 1)
                else
                {
                    continue;
                }
            }//for (int i = 0; i < m_discount_custom_targetList.Count; i++)

            dblResult = dblSumPrice * intY / 100;
            return dblResult;
        }
        public static double DiscountCustomTarget(int index, int intSID=0)//折扣對象依據(discount_custom_num): min/max/avg
        {
            double dblResult = 0;
            double dblmin = 0, dblmax = 0, dblavg = 0;
            double dblsun = 0;
            int intmin = 0, intmax = 0,intavg = 0;
            int j = 0;
            double [] dblBuf = new double [m_discount_custom_targetList.Count]; 
            for (int i= 0; i < m_discount_custom_targetList.Count;i++)
            {
                if (m_discount_custom_targetList[i].count==1)
                {
                    if(intSID!=0)
                    {
                        if (m_discount_custom_targetList[i].sid == intSID)
                        {
                            if(j==0)
                            {
                                dblmin = m_discount_custom_targetList[i].value;
                                dblmax = m_discount_custom_targetList[i].value;
                                intmin = i;
                                intmax = i;

                            }
                            else
                            {
                                if (m_discount_custom_targetList[i].value < dblmin)
                                {
                                    dblmin = m_discount_custom_targetList[i].value;
                                    intmin = i;
                                }

                                if (m_discount_custom_targetList[i].value > dblmax)
                                {
                                    dblmax = m_discount_custom_targetList[i].value;
                                    intmax = i;
                                }
                            }
                            j++;
                            dblsun += m_discount_custom_targetList[i].value;
                            dblBuf[i] = m_discount_custom_targetList[i].value;
                        }
                        else
                        {
                            dblBuf[i] = -1;
                            continue;
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            dblmin = m_discount_custom_targetList[i].value;
                            dblmax = m_discount_custom_targetList[i].value;
                            intmin = i;
                            intmax = i;
                        }
                        else
                        {
                            if (m_discount_custom_targetList[i].value < dblmin)
                            {
                                dblmin = m_discount_custom_targetList[i].value;
                                intmin = i;
                            }

                            if (m_discount_custom_targetList[i].value > dblmax)
                            {
                                dblmax = m_discount_custom_targetList[i].value;
                                intmax = i;
                            }
                        }
                        j++;
                        dblsun += m_discount_custom_targetList[i].value;
                        dblBuf[i] = m_discount_custom_targetList[i].value;
                    }

                }
                else
                {
                    dblBuf[i] = -1;
                }
            }

            if(j>0)
            {
                dblavg = dblsun / j;
            }

            switch (SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.discount_custom_target)
            {
                case "min"://取最低價位
                    m_discount_custom_targetList[intmin].count = 0;
                    dblResult = dblmin;
                    break;
                case "max"://取最高價位
                    m_discount_custom_targetList[intmax].count = 0;
                    dblResult = dblmax;
                    break;
                case "avg":
                    dblmin = 0;
                    int k = 0;
                    for (int i = 0; i < dblBuf.Length; i++)
                    {
                        if (dblBuf[i]<0)
                        {
                            continue;
                        }
                        if (k == 0)
                        {
                            intavg = i;
                            dblmin = Math.Abs(dblBuf[i] - dblavg);
                        }
                        else
                        {
                            if (dblmin > Math.Abs(dblBuf[i] - dblavg))
                            {
                                intavg = i;
                                dblmin = Math.Abs(dblBuf[i] - dblavg);
                            }
                        }
                        k++;
                    }
                    m_discount_custom_targetList[intavg].count = 0;
                    dblResult = dblBuf[intavg];
                    break;
                case "round"://促銷計算功能，擴充增加[取平均(四捨五入)]的計算參數
                    m_discount_custom_targetList[intmin].count = 0;
                    dblResult = Math.Round(dblavg, MidpointRounding.AwayFromZero);
                    break;
                default://(single)
                    m_discount_custom_targetList[intmin].count = 0;
                    dblResult = dblmin;
                    break;

            }

            return dblResult;
        }
        public static void TriggerTypeNum(int index,out double[,] dblResult)//觸發數量依據(trigger_num_target):總數量(total)/同一商品數量(single)
        {
            //---
            //取出購物車內各產品ID和數量
            double[,] productValue = null;//個別產品的對應數值

            String SQL = "";
            if (SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.discount_target == "custom")
            {
                String StrProductSID = "";
                for (int i = 0; i < SQLDataTableModel.m_promotion_join_dataList[index].m_ListProductID.Count; i++)
                {
                    if (i == 0)
                    {
                        StrProductSID += "'" + SQLDataTableModel.m_promotion_join_dataList[index].m_ListProductID[i] + "'";
                    }
                    else
                    {
                        StrProductSID += "," + "'" + SQLDataTableModel.m_promotion_join_dataList[index].m_ListProductID[i] + "'";
                    }
                }

                if (StrProductSID.Length > 0)
                {
                    SQL = String.Format("SELECT item_sid,SUM(item_count) FROM order_content_data WHERE order_no='{0}' AND del_flag='N' AND (item_type='P' OR item_type='T') AND (parent_item_no='0') AND (item_sid IN ({1})) GROUP BY item_sid", m_StrOrderNo, StrProductSID);
                }
                else
                {
                    SQL = String.Format("SELECT item_sid,SUM(item_count) FROM order_content_data WHERE order_no='{0}' AND del_flag='N' AND (item_type='P' OR item_type='T') AND (parent_item_no='0') GROUP BY item_sid", m_StrOrderNo);
                }
            }
            else
            {
                SQL = String.Format("SELECT item_sid,SUM(item_count) FROM order_content_data WHERE order_no='{0}' AND del_flag='N' AND (item_type='P' OR item_type='T') AND (parent_item_no='0') GROUP BY item_sid", m_StrOrderNo);
            }

            DataTable order_content_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
            if (order_content_dataDataTable.Rows.Count > 0)
            {
                productValue = new double[order_content_dataDataTable.Rows.Count, 2];
                for (int i = 0; i < order_content_dataDataTable.Rows.Count; i++)
                {
                    productValue[i, 0] = Convert.ToInt32(order_content_dataDataTable.Rows[i][0].ToString());//ID
                    productValue[i, 1] = Convert.ToInt32(order_content_dataDataTable.Rows[i][1].ToString());//Value
                }
            }
            //---取出購物車內各產品ID和數量

            if((productValue==null)|| productValue.GetLength(0)==0)
            {
                dblResult = new double[1, 1] { { 0 } };
                return;
            }

            if (SQLDataTableModel.m_promotion_join_dataList[index].m_promotion_data_rule.trigger_num_target== "total")
            {
                dblResult = new double[1, 1] { { 0 } };
                for(int i = 0; i < productValue.GetLength(0); i++)
                {
                    dblResult[0,0]+=productValue[i,1];
                }
            }
            else//single
            {
                dblResult = new double[productValue.GetLength(0), 2];
                Array.Copy(productValue, 0, dblResult, 0, productValue.Length);
            }

            if(order_content_dataDataTable.Rows.Count > 0)
            {
                order_content_dataDataTable.Rows.Clear();
                order_content_dataDataTable = null;
            }
        }

        public static double Rounding(double dblprice, double dbldiscount_rate, String StrCalcType)//四捨五入機制
        {
            double dblResult = dblprice * (100 - dbldiscount_rate) / 100;//折扣後商品價格

            if (StrCalcType == "S")//店家有利 - 先四捨五入之後再計算
            {
                //30*0.75(75折)=22.5元 -> 23元
                //折扣金額=30-23=7元
                dblResult = dblprice - Math.Round(dblResult, MidpointRounding.AwayFromZero);
            }
            else//客戶有利 - 先計算之後再四捨五入
            {
                //30*0.75(75折)=22.5元
                //折扣金額 = 30-22.5=7.5 -> 8元
                dblResult = Math.Round((dblprice - dblResult), MidpointRounding.AwayFromZero);
            }

            return dblResult;
        }

        public static double calculate(int ordertype, String Strorder_no,ref ODPromotionValue ODPromotionValueBuf, String StrCalcType= "S")
        {
            double dblResult = 0;

            m_StrOrderNo = Strorder_no;
            if (SQLDataTableModel.m_promotion_join_dataList.Count == 0)
            {
                return dblResult;//沒有促銷規則
            }
            else
            {
                for (int i = 0; i < SQLDataTableModel.m_promotion_join_dataList.Count; i++)
                {
                    //---
                    //訂單類型別判斷
                    bool blnOrderTypeCheck = false;
                    if (SQLDataTableModel.m_promotion_join_dataList[i].m_ListOrderTypeID.Count > 0)
                    {
                        for (int j = 0; j < SQLDataTableModel.m_promotion_join_dataList[i].m_ListOrderTypeID.Count; j++)
                        {
                            if (SQLDataTableModel.m_promotion_join_dataList[i].m_ListOrderTypeID[j] == ordertype)
                            {
                                blnOrderTypeCheck = true;
                                break;
                            }
                        }
                    }
                    else
                    {//無訂單資訊表示無訂單限制
                        blnOrderTypeCheck = true;
                    }
                    //---訂單類型別判斷

                    if (blnOrderTypeCheck & ((SQLDataTableModel.m_promotion_join_dataList[i].m_DTstart_time == SQLDataTableModel.m_promotion_join_dataList[i].m_DTend_time) ||
                        ((SQLDataTableModel.m_promotion_join_dataList[i].m_DTstart_time <= DateTime.Now) && (SQLDataTableModel.m_promotion_join_dataList[i].m_DTend_time >= DateTime.Now))))
                    {
                        /*
                        [discount_target & include_condiment] ~ order_content_data
                            “product”(訂單所有商品) & 包含配料金額	[“Y”]
                            “custom”(指定對象) &  包含配料金額	[“Y”]
                            “product”(訂單所有商品) & 包含配料金額	[“N”]
                            “custom”(指定對象) & 包含配料金額	[“N”]                    
                        */
                        DiscountTarget(i);//折扣對象依據(discount_custom_num): min/max/avg

                        //---
                        //判斷 trigger_type 欄位
                        /*
                        [trigger_type]
                            “empty”    無條件觸發
	                        “amount”    訂單金額
                                [trigger_amount_result]
		                            “Y”	折扣後金額 ~ order_data 的 (subtotal-discount_fee)
		                            “N”	訂單總額 ~ order_data 的 subtotal
	                        “num”	    商品數量
                                [trigger_num_target & discount_target] ~ order_content_data的item_sid,item_count
		                            “total”(總數量) & “product”(訂單所有商品)
		                            “total”(總數量) & “custom”(指定對象)
		                            “single”(同一商品數量) & “product”(訂單所有商品)
		                            “single”(同一商品數量) & “custom”(指定對象)
                        */
                        double[,] dblTriggerTypeValue = null;//記錄在trigger_type階段取抓取的運算參數
                        switch (SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.trigger_type)
                        {
                            case "empty"://無條件觸發
                                dblTriggerTypeValue = new double[,] { { 0 } };//無運算參數
                                if((m_discount_custom_targetList!=null))
                                {
                                    for(int j=0;j<m_discount_custom_targetList.Count;j++)
                                    {
                                        dblTriggerTypeValue[0, 0] += m_discount_custom_targetList[j].value;
                                    }                                   
                                }
                                break;
                            case "amount"://訂單金額
                                dblTriggerTypeValue = new double[,] { { 0 } };//觸發金額依據(trigger_amount_result): 訂單總額(total) / 折扣後金額(single)
                                TriggerTypeAmount(i, ref dblTriggerTypeValue);//觸發金額依據(trigger_amount_result): 訂單總額(total) / 折扣後金額(single)
                                break;
                            case "num"://商品數量
                                TriggerTypeNum(i, out dblTriggerTypeValue);//觸發數量依據(trigger_num_target):總數量(total)/同一商品數量(single)
                                break;
                        }
                        //---判斷 trigger_type 欄位

                        /*
                        [trigger_value]    金額(數量)條件(X)
                        [discount_custom_num]    折扣對象數量(Y)
                        */
                        double dblTriggerValue = ((SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.trigger_value!=null) && (SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.trigger_value.Length > 0)) ? Convert.ToDouble(SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.trigger_value) : 0;//金額(數量)條件(X)
                        int intDiscountCustomNum = ((SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.discount_custom_num!=null) &&(SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.discount_custom_num.Length > 0)) ? Convert.ToInt32(SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.discount_custom_num) : (int)dblTriggerValue;//折扣對象數量(Y)
                        double dblDiscountLimit = ((SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.discount_limit!=null) &&(SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.discount_limit.Length>0)) ? Convert.ToDouble(SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.discount_limit) : -1;//該筆促銷規則折扣上限金額
                        
                        //---
                        //dblDiscountVars
                        /*
                        [discount_type] & [discount_value]
                            “rate”    折扣(%)
                            “fee”    折價(元)
                            “price”    固定價(元)
                        [discount_limit]
                        */
                        double[] dblDiscountVars = new double[3];//discount_type: {[折扣類型(discount_type):(折扣rate)/(折價fee)/(固定價price)],[折扣數(discount_value)],[折扣上限金額(discount_limit)]}
                        switch (SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.discount_type)
                        {
                            case "rate"://折扣rate
                                dblDiscountVars[0] = 0;
                                break;
                            case "fee"://折價fee
                                dblDiscountVars[0] = 1;
                                break;
                            case "price"://固定價price
                                dblDiscountVars[0] = 2;
                                break;
                        }
                        dblDiscountVars[1] = ((SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.discount_value!=null)&&(SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.discount_value.Length>0)) ? Convert.ToDouble(SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.discount_value) : -1;//折扣數(discount_value)
                        dblDiscountVars[2] = ((SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.discount_limit!=null)&&(SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.discount_limit.Length>0)) ? Convert.ToDouble(SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.discount_limit) : -1;//折扣上限金額(discount_limit)
                        //---dblDiscountVars
               
                        /*
                        [trigger_repeat]
                            “Y”    可重複折扣
                            “N”    否重複折扣                        
                        */
                        bool blnTriggerRepeat = (SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.trigger_repeat == "Y") ? true : false;//重複觸發旗標  


                        //---
                        //實際計算
                        bool blnXCheck = true;
                        int intODPVCount = 0;
                        double dblODPVResult = 0;
                        do
                        {
                            //---
                            //[trigger_type]
                            switch (SQLDataTableModel.m_promotion_join_dataList[i].m_promotion_data_rule.trigger_type)
                            {
                                case "empty"://無條件觸發
                                    if(blnXCheck)
                                    {
                                        if (dblDiscountVars[0] == 0)//折扣rate
                                        {
                                            dblODPVResult += dblTriggerTypeValue[0, 0] * dblDiscountVars[1] / 100;
                                            intODPVCount += 1;
                                        }
                                        else if (dblDiscountVars[0] == 1)//折價fee
                                        {
                                            dblODPVResult += dblDiscountVars[1];
                                            intODPVCount += 1;
                                        }
                                        else//2 固定價price
                                        {
                                            if (dblTriggerTypeValue.GetLength(1) == 1)
                                            {
                                                for (int k = 0; k < intDiscountCustomNum; k++)
                                                {
                                                    double dblBuf = DiscountCustomTarget(i);//總數量(total)
                                                    //dblResult += dblBuf;
                                                    if (dblDiscountVars[1] > 0)
                                                    {
                                                        dblODPVResult += (dblBuf - dblDiscountVars[1]);
                                                    }
                                                    else
                                                    {
                                                        dblODPVResult += dblBuf;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                int j = 0;
                                                for (int k = 0; k < intDiscountCustomNum; k++)
                                                {
                                                    double dblBuf = DiscountCustomTarget(i, (int)dblTriggerTypeValue[j, 0]);//同一商品數量(single)
                                                    //dblResult += dblBuf;
                                                    if (dblDiscountVars[1] > 0)
                                                    {
                                                        dblODPVResult += (dblBuf - dblDiscountVars[1]);
                                                    }
                                                    else
                                                    {
                                                        dblODPVResult += dblBuf;
                                                    }
                                                }
                                            }
                                            intODPVCount += intDiscountCustomNum;
                                        }
                                        blnXCheck = false;
                                    }
                                    break;
                                case "amount"://訂單金額
                                    for (int j = 0; j < dblTriggerTypeValue.GetLength(0); j++)
                                    {
                                        //---
                                        //金額(數量)條件(X) 判斷
                                        if (dblTriggerTypeValue.GetLength(1) == 1)
                                        {
                                            if (dblTriggerValue <= dblTriggerTypeValue[j, 0])
                                            {
                                                blnXCheck = true;//金額(數量)條件(X) 滿足
                                                dblTriggerTypeValue[j, 0] = dblTriggerTypeValue[j, 0] - dblTriggerValue;
                                            }
                                            else
                                            {
                                                blnXCheck = false;//金額(數量)條件(X) 不滿足
                                            }
                                        }

                                        if (blnXCheck)
                                        {
                                            if (dblDiscountVars[0] == 0)//折扣rate
                                            {
                                                dblODPVResult += dblTriggerValue * dblDiscountVars[1] / 100;
                                                intODPVCount += 1;
                                            }
                                            else if (dblDiscountVars[0] == 1)//折價fee
                                            {
                                                dblODPVResult += dblDiscountVars[1];
                                                intODPVCount += 1;
                                            }
                                            else//2 固定價price
                                            {
                                                if (dblTriggerTypeValue.GetLength(1) == 1)
                                                {
                                                    for (int k = 0; k < intDiscountCustomNum; k++)
                                                    {
                                                        double dblBuf = DiscountCustomTarget(i);//總數量(total)
                                                                                                //dblResult += dblBuf;
                                                        if (dblDiscountVars[1] > 0)
                                                        {
                                                            dblODPVResult += (dblBuf - dblDiscountVars[1]);
                                                        }
                                                        else
                                                        {
                                                            dblODPVResult += dblBuf;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    for (int k = 0; k < intDiscountCustomNum; k++)
                                                    {
                                                        double dblBuf = DiscountCustomTarget(i, (int)dblTriggerTypeValue[j, 0]);//同一商品數量(single)
                                                                                                                                //dblResult += dblBuf;
                                                        if (dblDiscountVars[1] > 0)
                                                        {
                                                            dblODPVResult += (dblBuf - dblDiscountVars[1]);
                                                        }
                                                        else
                                                        {
                                                            dblODPVResult += dblBuf;
                                                        }
                                                    }
                                                }
                                                intODPVCount += intDiscountCustomNum;
                                            }
                                        }
                                    }
                                    break;
                                case "num"://商品數量
                                    for(int j=0;j< dblTriggerTypeValue.GetLength(0);j++)
                                    {
                                        //---
                                        //金額(數量)條件(X) 判斷
                                        if (dblTriggerTypeValue.GetLength(1)==1)
                                        {
                                            if(dblTriggerValue<= dblTriggerTypeValue[j,0])
                                            {
                                                blnXCheck = true;//金額(數量)條件(X) 滿足
                                                dblTriggerTypeValue[j, 0] = dblTriggerTypeValue[j, 0] - dblTriggerValue;
                                            }
                                            else
                                            {
                                                blnXCheck = false;//金額(數量)條件(X) 不滿足
                                            }
                                        }
                                        else
                                        {
                                            if (dblTriggerValue <= dblTriggerTypeValue[j, 1])
                                            {
                                                blnXCheck = true;//金額(數量)條件(X) 滿足
                                                dblTriggerTypeValue[j, 1] = dblTriggerTypeValue[j, 1] - dblTriggerValue;
                                            }
                                            else
                                            {
                                                blnXCheck = false;//金額(數量)條件(X) 不滿足
                                            }
                                        }
                                        //---金額(數量)條件(X) 判斷

                                        if (blnXCheck)
                                        {
                                            if(dblDiscountVars[0] == 0)//折扣rate
                                            {
                                                if (dblTriggerTypeValue.GetLength(1) == 1)
                                                {
                                                    double dblBuf = DiscountValue(i, StrCalcType);
                                                    //dblResult += dblBuf;
                                                    dblODPVResult += dblBuf;
                                                }
                                                else
                                                {
                                                    double dblBuf = DiscountValue(i, StrCalcType, (int)dblTriggerTypeValue[j, 0]);
                                                    //dblResult += dblBuf;
                                                    dblODPVResult += dblBuf;
                                                }
                                                intODPVCount += (int)intDiscountCustomNum;
                                            }
                                            else if (dblDiscountVars[0]==1)//折價fee
                                            {
                                                //dblResult += dblBuf;
                                                dblODPVResult += (dblDiscountVars[1]);
                                                intODPVCount++;
                                            }
                                            else//2 固定價price
                                            {
                                                if (dblTriggerTypeValue.GetLength(1) == 1)
                                                {
                                                    for (int k = 0; k < intDiscountCustomNum; k++)
                                                    {
                                                        double dblBuf = DiscountCustomTarget(i);//總數量(total)
                                                        //dblResult += dblBuf;
                                                        if (dblDiscountVars[1] > 0)
                                                        {
                                                            dblODPVResult += (dblBuf - dblDiscountVars[1]);
                                                        }
                                                        else
                                                        {
                                                            dblODPVResult += dblBuf;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    for (int k = 0; k < intDiscountCustomNum; k++)
                                                    {
                                                        double dblBuf = DiscountCustomTarget(i, (int)dblTriggerTypeValue[j, 0]);//同一商品數量(single)
                                                        //dblResult += dblBuf;
                                                        if(dblDiscountVars[1]>0)
                                                        {
                                                            dblODPVResult += (dblBuf- dblDiscountVars[1]);
                                                        }
                                                        else
                                                        {
                                                            dblODPVResult += dblBuf;
                                                        }                                                        
                                                    }
                                                }
                                                intODPVCount += intDiscountCustomNum;
                                            }
                                        }
                                    }
                                    break;
                            }
                            //---[trigger_type]
                        } while (blnXCheck & blnTriggerRepeat);
                        //---實際計算

                        //---
                        //discount_limit
                        if((dblDiscountLimit>0) && (dblODPVResult > dblDiscountLimit))
                        {
                            dblODPVResult = dblDiscountLimit;
                        }
                        dblResult += dblODPVResult;
                        //---discount_limit

                        //---
                        //ODPromotionValue
                        ODPVArray ODPVArrayBuf = new ODPVArray();
                        ODPVArrayBuf.promotion_sid = SQLDataTableModel.m_promotion_join_dataList[i].m_intSID;
                        ODPVArrayBuf.promotion_name = SQLDataTableModel.m_promotion_join_dataList[i].m_StrName;
                        ODPVArrayBuf.promotion_info = $"共計折扣 {dblODPVResult}元({intODPVCount}次)";
                        ODPVArrayBuf.promotion_count = intODPVCount;
                        ODPVArrayBuf.promotion_fee = (int)dblODPVResult;

                        ODPromotionValueBuf.Array.Add(ODPVArrayBuf);
                        //---ODPromotionValue

                        //---
                        //該促銷是獨立使用，因此之後的促銷都不能運算
                        if (SQLDataTableModel.m_promotion_join_dataList[i].m_Strcoexist_flag == "N")
                        {
                            break;//for (int i=0;i< SQLDataTableModel.m_promotion_join_dataList.Count;i++)
                        }
                        //---該促銷是獨立使用，因此之後的促銷都不能運算
                    }
                    else
                    {//該筆促銷條件已超過時效或者訂單類型不合,直接略過
                        continue;
                    }
                }//for (int i=0;i< SQLDataTableModel.m_promotion_join_dataList.Count;i++)

                return Math.Round(dblResult, MidpointRounding.AwayFromZero);//return dblResult;
            }//if (SQLDataTableModel.m_promotion_join_dataList.Count==0) else
        }

    }

    public class Refund_fun//退款相關
    {
        public static void OrderCancel(String StrOrderNo)//訂單作廢
        {
            String SQL = String.Format("UPDATE order_data SET cancel_flag='Y',cancel_time='{1}',updated_time='{1}' WHERE order_no='{0}';", StrOrderNo, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

            SyncDBFun.dataInsert("COD", StrOrderNo);

            //---
            //電子發票做廢
            DataTable order_invoice_dataDataTable = new DataTable();
            SQL = $"SELECT order_no FROM order_invoice_data WHERE order_no='{StrOrderNo}'";
            order_invoice_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
            if (order_invoice_dataDataTable != null && order_invoice_dataDataTable.Rows.Count > 0)
            {
                //---
                //產生對應列印運算FIFO值
                lock (PrintThread.m_FIFOLock[0])
                {
                    PrintData PrintDataBuf = new PrintData(11, StrOrderNo);//11:發票作廢列印
                    PrintThread.m_Queue[0].Enqueue(PrintDataBuf);//塞入值
                }
                //---產生對應列印運算FIFO值
                SyncDBFun.dataInsert("INV_B2C_CANCEL", StrOrderNo);
            }
            //---電子發票做廢
        }

        public static bool Refund(String StrOrderNo,ref String StrMsg,String Strpayment_code = "")//退費
        {
            bool blnResult = false;
            String StrInvoiceDate = "";
            String SQL = "";
            String StrCondition = (Strpayment_code.Length > 0) ? $" AND payment_code='{Strpayment_code}'" : "";


            SQL = $"SELECT order_time FROM order_data WHERE order_no='{StrOrderNo}' LIMIT 0,1";
            DataTable order_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
            if((order_dataDataTable!=null) && (order_dataDataTable.Rows.Count>0) && (order_dataDataTable.Rows[0][0].ToString().Length>0)) 
            {
                StrInvoiceDate = order_dataDataTable.Rows[0][0].ToString();
                //StrInvoiceDate = "";
                if (CancelInvoiceCheck(StrInvoiceDate))
                {
                    SQL = $"SELECT amount,item_no,payment_name,payment_code,payment_module_code,payment_module_params,payment_info FROM order_payment_data WHERE del_flag!='Y' AND order_no='{StrOrderNo}'{StrCondition}";
                    DataTable order_payment_dataDataTable = SQLDataTableModel.GetDataTable(SQL);
                    if((order_payment_dataDataTable != null) && (order_payment_dataDataTable.Rows.Count>0))
                    {
                        blnResult = true;//預設可以被退款成功
                        for (int i=0;i< order_payment_dataDataTable.Rows.Count;i++)
                        {
                            String amount = order_payment_dataDataTable.Rows[i]["amount"].ToString();
                            String item_no = order_payment_dataDataTable.Rows[i]["item_no"].ToString();
                            String payment_name = order_payment_dataDataTable.Rows[i]["payment_name"].ToString();
                            String payment_code = order_payment_dataDataTable.Rows[i]["payment_code"].ToString();
                            String payment_module_code = order_payment_dataDataTable.Rows[i]["payment_module_code"].ToString();
                            String payment_module_params = order_payment_dataDataTable.Rows[i]["payment_module_params"].ToString();
                            String payment_info = order_payment_dataDataTable.Rows[i]["payment_info"].ToString();
                            String StrResult = "";

                            if ((payment_module_code!=null) && (payment_module_code.Length>0) && (payment_module_params!=null) && (payment_module_params.Length>=0) && (payment_module_code!=null) && (payment_module_code.Length>0))
                            {
                                switch(payment_module_code)
                                {
                                    case "LINE_PAY"://LinePay
                                        LinePayInfoOut LinePayInfoOutBuf = JsonClassConvert.LinePayInfoOut2Class(payment_info);
                                        LinePayRefundOut LinePayRefundOutBuf = new LinePayRefundOut();
                                        if(LinePayInfoOutBuf!=null)
                                        {
                                            if(LinePayAPI.Refund(LinePayInfoOutBuf.channelId, LinePayInfoOutBuf.info.transactionId.ToString(), ref LinePayRefundOutBuf))
                                            {
                                                StrMsg += $"{payment_name}退款成功\n";
                                                blnResult &= true;
                                                String StrLinePayRefundOut = JsonClassConvert.LinePayRefundOut2String(LinePayRefundOutBuf);
                                                String StrNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                                SQL = $"UPDATE order_payment_data SET refund_info='{StrLinePayRefundOut}',refund_time='{StrNowTime}',updated_time='{StrNowTime}' WHERE order_no='{StrOrderNo}' AND payment_code='{payment_code}' AND item_no='{item_no}'";
                                                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                                            }
                                            else
                                            {
                                                StrMsg += $"{LinePayAPI.PaymentCode2Info(LinePayRefundOutBuf.returnCode)},{payment_name}退款失敗\n";
                                                blnResult &= false;
                                            }
                                        }
                                        else
                                        {
                                            StrMsg += $"{payment_name}退款失敗\n";
                                            blnResult &= false;
                                        }
                                        break;
                                    case "EASY_CARD"://悠遊卡
                                        StrResult = "";
                                        if (EasyCardAPI.Refund(Convert.ToInt32(amount), ref StrResult)) 
                                        {
                                            StrMsg += $"{payment_name}退款成功\n";
                                            blnResult &= true;

                                            String StrLinePayRefundOut = StrResult;
                                            String StrNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                            SQL = $"UPDATE order_payment_data SET refund_info='{StrLinePayRefundOut}',refund_time='{StrNowTime}',updated_time='{StrNowTime}' WHERE order_no='{StrOrderNo}' AND payment_code='{payment_code}' AND item_no='{item_no}'";
                                            SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);

                                            //---
                                            //產生對應列印運算FIFO值
                                            lock (PrintThread.m_FIFOLock[0])
                                            {
                                                PrintData PrintDataBuf = new PrintData(9, StrOrderNo);//9:悠遊卡退費帳單
                                                PrintThread.m_Queue[0].Enqueue(PrintDataBuf);//塞入值
                                            }
                                            //---產生對應列印運算FIFO值
                                        }
                                        else 
                                        {
                                            blnResult &= false;
                                        }
                                        break;
                                    case "NCCC_CREDIT_CARD"://聯合信用卡
                                        StrResult = "";
                                        CreditCardJosn CreditCardJosnBuf = JsonClassConvert.CreditCardJosn2Class(payment_info);
                                        String StrQMsg = $"請跟顧客核對下列資訊，資料正確才可順利退款:\n卡號:【 {CreditCardJosnBuf.Card_No} 】?\n授權碼:【 {CreditCardJosnBuf.Approval_No} 】?\n退款金額:{Convert.ToInt32(CreditCardJosnBuf.Trans_Amount)/100}元?";
                                        //QuestionMsg QuestionMsgBuf = new QuestionMsg(StrQMsg);
                                        //QuestionMsgBuf.ShowDialog();
                                        if(true)//if(QuestionMsgBuf.m_blnRun)
                                        {
                                            if (NCCCAPI.Refund(Convert.ToInt32(CreditCardJosnBuf.Trans_Amount) / 100, CreditCardJosnBuf.Approval_No, ref StrResult))
                                            {
                                                StrMsg += $"{payment_name}退款成功\n";
                                                blnResult &= true;

                                                String StrLinePayRefundOut = StrResult;
                                                String StrNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                                SQL = $"UPDATE order_payment_data SET refund_info='{StrLinePayRefundOut}',refund_time='{StrNowTime}',updated_time='{StrNowTime}' WHERE order_no='{StrOrderNo}' AND payment_code='{payment_code}' AND item_no='{item_no}'";
                                                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                                            }
                                            else
                                            {
                                                blnResult &= false;
                                            }
                                        }
                                        else
                                        {
                                            blnResult &= false;
                                        }
                                        break;
                                    case "EASY_WALLET"://悠遊付
                                        
                                        break;
                                }
                            }
                            else
                            {//沒有模組 都可以直接視為退款成功
                                StrMsg += $"{payment_name}退款成功\n";
                                String StrPayRefundOut = $"[{payment_code}]Refund.";
                                String StrNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                SQL = $"UPDATE order_payment_data SET refund_info='{StrPayRefundOut}',refund_time='{StrNowTime}',updated_time='{StrNowTime}' WHERE order_no='{StrOrderNo}' AND payment_code='{payment_code}' AND item_no='{item_no}'";
                                SQLDataTableModel.SQLiteInsertUpdateDelete(SQL);
                            }
                        }
                    }
                    else
                    {
                        blnResult = true;
                    }
                }
                else
                {
                    StrMsg = "發票不能作廢，因此訂單不能作廢";
                }
            }
            else
            {
                StrMsg = "查無訂單資訊，因此訂單不能作廢";
            }
            return blnResult;
        }

        private static bool CancelInvoiceCheck(String StrInvoiceDate)//判斷發票是否可作廢
        {
            /*
            同期別 可作廢
            不同期別 15日之前 可作廢
            不同期別 15日之後 不可作廢

            發票期別
            1-2   ->2月
            3-4   ->4月
            5-6   ->6月
            7-8   ->8月
            9-10  ->10月
            11-12 ->12月
            */
            bool blnResult = false;

            int intYearNow = DateTime.Now.Year;
            int intMonthNow = DateTime.Now.Month;
            int intDayNow = DateTime.Now.Day;
            int intNowPeriod = (DateTime.Now.Month%2 == 0) ? DateTime.Now.Month : (DateTime.Now.Month+1);//本月期別
            int intInvoicePeriod = 0;//發票期別
            DateTime DTInvoiceDate;

            try
            {
                DTInvoiceDate = Convert.ToDateTime(StrInvoiceDate);
                intInvoicePeriod = (DTInvoiceDate.Month % 2 == 0) ? DTInvoiceDate.Month : (DTInvoiceDate.Month+1);
            }
            catch
            {
                return blnResult;//不可作廢
            }

            if((intYearNow - DTInvoiceDate.Year)<=1)//年度檢核[0/1]
            {
                if( intNowPeriod == intInvoicePeriod)//同期可作廢
                {
                    blnResult = true;
                }
                else//不同期
                {
                    if(intDayNow<15)
                    {
                        blnResult = true;//15日之前 可作廢
                    }
                    else
                    {
                        blnResult = false;//15日之前 不可作廢
                    }
                }
            }
            else
            {
                blnResult = false;
            }

            return blnResult;
        }

        private static bool CancelInvoiceCheck_Old(String StrInvoiceDate)//判斷發票是否可作廢
        {
            /*
            今天(奇數月15日之前): 該月往前2個月+該月份發票 可作廢
            今天(奇數月15日之後): 該月份的發票 可作廢
            今天(偶數月):該月往前1個月+該月份發票 可作廢
             */
            bool blnResult = false;
            int intYearNow = DateTime.Now.Year;
            int intMonthNow = DateTime.Now.Month;
            int intDayNow = DateTime.Now.Day;
            DateTime DTInvoiceDate;
            try
            {
                DTInvoiceDate = Convert.ToDateTime(StrInvoiceDate);
                TimeSpan ts = DateTime.Now - DTInvoiceDate;
                if (ts.TotalSeconds<0)
                {
                    return blnResult;
                }
            }
            catch
            {
                return blnResult;
            }

            if (((intYearNow - DTInvoiceDate.Year) == 0) || ((intYearNow - DTInvoiceDate.Year) == 1))//發票日期
            {
                switch (intMonthNow % 2)
                {
                    case 0://偶數月
                        if(intYearNow == DTInvoiceDate.Year)//為同年
                        {
                            if(((intMonthNow- DTInvoiceDate.Month)==0) || ((intMonthNow - DTInvoiceDate.Month) == 1))
                            {
                                blnResult = true;
                            }
                            else
                            {
                                blnResult = false;
                            }
                        }
                        else//不為同年
                        {
                            blnResult = false;
                        }
                        break;
                    case 1://奇數月
                        if (intDayNow < 15)//奇數月15日之前
                        {
                            if((intYearNow - DTInvoiceDate.Year) == 1)
                            {
                                if((DTInvoiceDate.Month==11) || (DTInvoiceDate.Month == 12) || (DTInvoiceDate.Month == 1))
                                {
                                    blnResult = true;
                                }
                                else
                                {
                                    blnResult = false;
                                }
                            }
                            else
                            {
                                if(((intMonthNow-DTInvoiceDate.Month)>=0) && ((intMonthNow - DTInvoiceDate.Month) <2))
                                {
                                    blnResult = true;
                                }
                                else
                                {
                                    blnResult = false;
                                }
                            }
                        }
                        else//奇數月15日之後
                        {
                            if (((intYearNow - DTInvoiceDate.Year) == 0) && ((intMonthNow - DTInvoiceDate.Month) == 0))
                            {
                                blnResult = true;
                            }
                            else
                            {
                                blnResult = false;
                            }
                        }
                        break;
                }
            }
            else
            {
                blnResult = false;
            }

            return blnResult;
        }
    }
}
