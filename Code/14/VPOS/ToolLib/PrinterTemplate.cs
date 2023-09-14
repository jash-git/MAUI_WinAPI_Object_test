using Jint;
using System.Data;
using System.IO.Ports;
//出處/原始教學 網站
//https://github.com/sebastienros/jint
//https://www.youtube.com/watch?v=yCs6UmogKEg&t=57s
//https://docs.microsoft.com/zh-tw/shows/code-conversations/sebastien-ros-on-jint-javascript-interpreter-net
//延伸教學 網站
//https://blog.no2don.com/2020/03/cnet-core-c-jint-javascript.html

using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace VPOS
{
    public class PT_CommandOutput
    {
        public int state_code { get; set; }
        public List<string> value { get; set; }
        public PT_CommandOutput()
        {
            state_code = 0;
            value = new List<string>();
        }
    }

    public class PrinterTemplate
    {
        //---
        //共用函數
        public static string UnescapeUnicode(string str)  // 將unicode轉義序列(\uxxxx)解碼為字符串
        {
            //GOOGLE: C#  \uXXXX
            //https://www.cnblogs.com/netlog/p/15911016.html C#字串Unicode轉義序列編解碼
            //https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.unescape?view=net-6.0
            //https://docs.microsoft.com/zh-tw/dotnet/api/system.text.regularexpressions.regex.unescape?view=net-6.0

            return (System.Text.RegularExpressions.Regex.Unescape(str));
        }
        //---共用函數

        public static bool ESCPOS_Work(String StrInput, out PT_CommandOutput PT_CommandOutputs)//工作單
        {
            bool blnResult = false;
            PT_CommandOutputs = null;
            var engine = new Engine();

            if ((SQLDataTableModel.m_printer_templateDataTable != null) && (SQLDataTableModel.m_printer_valueList != null) && (SQLDataTableModel.m_printer_valueList.Count > 0) && (SQLDataTableModel.m_printer_templateDataTable.Rows.Count > 0))//判斷列印範本資料表有資料
            {
                //m_printer_templateDataTable --印表範本資料表全部資料
                //m_printer_valueList --存放所有UI印表機設定值

                //---
                //從DB載入對應範本到engine中
                String Strprint_templateSID = "-1";//預設值確保DB查不到
                for (int i = 0; i < SQLDataTableModel.m_printer_valueList.Count; i++)
                {
                    if (SQLDataTableModel.m_printer_valueList[i].template_type == "WORK_TICKET")//if (SQLDataTableModel.m_printer_valueList[i].output_type == "W")
                    {
                        /*
                        String Strprint_template = SQLDataTableModel.m_printer_valueList[i].printer_config_value.print_template;//顯示名稱;DB_SID
                        if (Strprint_template.Length > 0)
                        {
                            Strprint_templateSID = Strprint_template.Split(";")[1];
                        }
                        */
                        Strprint_templateSID = SQLDataTableModel.m_printer_valueList[i].template_sid;
                        break;
                    }
                }

                String expression01 = $"SID = '{Strprint_templateSID}'";
                DataRow[] foundRows01 = SQLDataTableModel.m_printer_templateDataTable.Select(expression01);
                if (foundRows01.Length > 0)
                {
                    if (foundRows01[0]["include_command"].ToString() == "Y")//有引用共用函數
                    {
                        String expression02 = $"template_type = 'ESC_COMMAND'";
                        DataRow[] foundRows02 = SQLDataTableModel.m_printer_templateDataTable.Select(expression02);
                        if (foundRows02.Length > 0)
                        {
                            engine.Execute(foundRows02[0]["template_value"].ToString());//共用範本載入
                        }
                    }
                    engine.Execute(foundRows01[0]["template_value"].ToString());//WORK_TICKET(工作單)範本載入
                }
                else
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Work", StrInput, "printer_templateDataTable.foundRows.Count=0");
                    LogFile.Write("JintErrot ; " + StrLog);
                    return blnResult;
                }
                //---從DB載入對應範本到engine中
            }
            else
            {
                String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Work", StrInput, "printer_templateDataTable.Rows.Count=0");
                LogFile.Write("JintErrot ; " + StrLog);
                return blnResult;
            }

            try
            {
                engine.SetValue("input", StrInput);//設定輸入值

                String StrFunName = "Main()";
                var StrJsonResult = engine.Execute(StrFunName).GetCompletionValue();//執行範本運算

                PT_CommandOutputs = new PT_CommandOutput();
                PT_CommandOutputs = JsonSerializer.Deserialize<PT_CommandOutput>(StrJsonResult.AsString());//取回運算結果
            }
            catch (Exception ex)
            {
                PT_CommandOutputs = null;
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Work", StrInput, ex.ToString());
                    LogFile.Write("JintErrot ; " + StrLog);
                }
            }

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0))
            {
                for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                {
                    PT_CommandOutputs.value[i] = UnescapeUnicode(PT_CommandOutputs.value[i]);
                }

                blnResult = true;
            }

            return blnResult;
        }

        public static bool ESCPOS_Number(String StrInput, out PT_CommandOutput PT_CommandOutputs)//號碼單
        {
            bool blnResult = false;
            PT_CommandOutputs = null;
            var engine = new Engine();

            if ((SQLDataTableModel.m_printer_templateDataTable != null) && (SQLDataTableModel.m_printer_valueList != null) && (SQLDataTableModel.m_printer_valueList.Count > 0) && (SQLDataTableModel.m_printer_templateDataTable.Rows.Count > 0))//判斷列印範本資料表有資料
            {
                //m_printer_templateDataTable --印表範本資料表全部資料
                //m_printer_valueList --存放所有UI印表機設定值

                //---
                //從DB載入對應範本到engine中
                String Strprint_templateSID = "-1";//預設值確保DB查不到
                for (int i = 0; i < SQLDataTableModel.m_printer_valueList.Count; i++)
                {
                    if (SQLDataTableModel.m_printer_valueList[i].template_type == "NUMBER")//if (SQLDataTableModel.m_printer_valueList[i].output_type == "N")
                    {
                        /*
                        String Strprint_template = SQLDataTableModel.m_printer_valueList[i].printer_config_value.print_template;//顯示名稱;DB_SID
                        if (Strprint_template.Length > 0)
                        {
                            Strprint_templateSID = Strprint_template.Split(";")[1];
                        }
                        */
                        Strprint_templateSID = SQLDataTableModel.m_printer_valueList[i].template_sid;
                        break;
                    }
                }

                String expression01 = $"SID = '{Strprint_templateSID}'";
                DataRow[] foundRows01 = SQLDataTableModel.m_printer_templateDataTable.Select(expression01);
                if (foundRows01.Length > 0)
                {
                    if (foundRows01[0]["include_command"].ToString() == "Y")//有引用共用函數
                    {
                        String expression02 = $"template_type = 'ESC_COMMAND'";
                        DataRow[] foundRows02 = SQLDataTableModel.m_printer_templateDataTable.Select(expression02);
                        if (foundRows02.Length > 0)
                        {
                            engine.Execute(foundRows02[0]["template_value"].ToString());//共用範本載入
                        }
                    }
                    engine.Execute(foundRows01[0]["template_value"].ToString());//NUMBER(號碼機)範本載入
                }
                else
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Number", StrInput, "printer_templateDataTable.foundRows.Count=0");
                    LogFile.Write("JintErrot ; " + StrLog);
                    return blnResult;
                }
                //---從DB載入對應範本到engine中
            }
            else
            {
                String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Number", StrInput, "printer_templateDataTable.Rows.Count=0");
                LogFile.Write("JintErrot ; " + StrLog);
                return blnResult;
            }

            try
            {
                engine.SetValue("input", StrInput);//設定輸入值

                String StrFunName = "Main()";
                var StrJsonResult = engine.Execute(StrFunName).GetCompletionValue();//執行範本運算

                PT_CommandOutputs = new PT_CommandOutput();
                PT_CommandOutputs = JsonSerializer.Deserialize<PT_CommandOutput>(StrJsonResult.AsString());//取回運算結果
            }
            catch (Exception ex)
            {
                PT_CommandOutputs = null;
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Number", StrInput, ex.ToString());
                    LogFile.Write("JintErrot ; " + StrLog);
                }
            }

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0))
            {
                for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                {
                    PT_CommandOutputs.value[i] = UnescapeUnicode(PT_CommandOutputs.value[i]);
                }

                blnResult = true;
            }

            return blnResult;
        }

        public static bool ESCPOS_Lable(String StrInput, String StrMemo, out PT_CommandOutput PT_CommandOutputs)//標籤
        {
            bool blnResult = false;
            PT_CommandOutputs = null;
            var engine = new Engine();

            if ((SQLDataTableModel.m_printer_templateDataTable != null) && (SQLDataTableModel.m_printer_valueList != null) && (SQLDataTableModel.m_printer_valueList.Count > 0) && (SQLDataTableModel.m_printer_templateDataTable.Rows.Count > 0))//判斷列印範本資料表有資料
            {
                //m_printer_templateDataTable --印表範本資料表全部資料
                //m_printer_valueList --存放所有UI印表機設定值

                //---
                //從DB載入對應範本到engine中
                String Strprint_templateSID = "-1";//預設值確保DB查不到
                for (int i = 0; i < SQLDataTableModel.m_printer_valueList.Count; i++)
                {
                    if (SQLDataTableModel.m_printer_valueList[i].template_type == "LABLE")//if (SQLDataTableModel.m_printer_valueList[i].output_type == "W")
                    {
                        /*
                        String Strprint_template = SQLDataTableModel.m_printer_valueList[i].printer_config_value.print_template;//顯示名稱;DB_SID
                        if (Strprint_template.Length > 0)
                        {
                            Strprint_templateSID = Strprint_template.Split(";")[1];
                        }
                        */
                        Strprint_templateSID = SQLDataTableModel.m_printer_valueList[i].template_sid;
                        break;
                    }
                }

                String expression01 = $"SID = '{Strprint_templateSID}'";
                DataRow[] foundRows01 = SQLDataTableModel.m_printer_templateDataTable.Select(expression01);
                if (foundRows01.Length > 0)
                {
                    if (foundRows01[0]["include_command"].ToString() == "Y")//有引用共用函數
                    {
                        String expression02 = $"template_type = 'ESC_COMMAND'";
                        DataRow[] foundRows02 = SQLDataTableModel.m_printer_templateDataTable.Select(expression02);
                        if (foundRows02.Length > 0)
                        {
                            engine.Execute(foundRows02[0]["template_value"].ToString());//共用範本載入
                        }
                    }
                    engine.Execute(foundRows01[0]["template_value"].ToString());//LABEL(標籤貼紙)範本載入
                }
                else
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Lable", StrInput + ";;" + StrMemo, "printer_templateDataTable.foundRows.Count=0");
                    LogFile.Write("JintErrot ; " + StrLog);
                    return blnResult;
                }
                //---從DB載入對應範本到engine中
            }
            else
            {
                String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Lable", StrInput +";;" + StrMemo, "printer_templateDataTable.Rows.Count=0");
                LogFile.Write("JintErrot ; " + StrLog);
                return blnResult;
            }

            try
            {
                engine.SetValue("input", StrInput);//設定輸入值
                engine.SetValue("memo", StrMemo);//設定輸入值

                String StrFunName = "Main()";
                var StrJsonResult = engine.Execute(StrFunName).GetCompletionValue();//執行範本運算

                PT_CommandOutputs = new PT_CommandOutput();
                PT_CommandOutputs = JsonSerializer.Deserialize<PT_CommandOutput>(StrJsonResult.AsString());//取回運算結果
            }
            catch (Exception ex)
            {
                PT_CommandOutputs = null;
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Lable", StrInput + ";;" + StrMemo, ex.ToString());
                    LogFile.Write("JintErrot ; " + StrLog);
                }
            }

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0))
            {
                for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                {
                    PT_CommandOutputs.value[i] = UnescapeUnicode(PT_CommandOutputs.value[i]);
                }

                blnResult = true;
            }

            return blnResult;
        }

        public static bool EasyCard_Report(String StrInput, out PT_CommandOutput PT_CommandOutputs)//悠遊卡報表
        {
            bool blnResult = false;
            PT_CommandOutputs = null;
            var engine = new Engine();

            if ((SQLDataTableModel.m_printer_templateDataTable != null) && (SQLDataTableModel.m_printer_valueList != null) && (SQLDataTableModel.m_printer_valueList.Count > 0) && (SQLDataTableModel.m_printer_templateDataTable.Rows.Count > 0))//判斷列印範本資料表有資料
            {
                //m_printer_templateDataTable --印表範本資料表全部資料
                //m_printer_valueList --存放所有UI印表機設定值

                //---
                //從DB載入對應範本到engine中
                String Strtemplate_type = "EASYCARD_CHECKOUT";//預設值確保DB查不到

                String expression01 = $"template_type = '{Strtemplate_type}'";
                DataRow[] foundRows01 = SQLDataTableModel.m_printer_templateDataTable.Select(expression01);
                if (foundRows01.Length > 0)
                {
                    if (foundRows01[0]["include_command"].ToString() == "Y")//有引用共用函數
                    {
                        String expression02 = $"template_type = 'ESC_COMMAND'";
                        DataRow[] foundRows02 = SQLDataTableModel.m_printer_templateDataTable.Select(expression02);
                        if (foundRows02.Length > 0)
                        {
                            engine.Execute(foundRows02[0]["template_value"].ToString());//共用範本載入
                        }
                    }
                    engine.Execute(foundRows01[0]["template_value"].ToString());//REPORT(報表)範本載入
                }
                else
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Report", StrInput, "printer_templateDataTable.foundRows.Count=0");
                    LogFile.Write("JintErrot ; " + StrLog);
                    return blnResult;
                }
                //---從DB載入對應範本到engine中
            }
            else
            {
                String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Report", StrInput, "printer_templateDataTable.Rows.Count=0");
                LogFile.Write("JintErrot ; " + StrLog);
                return blnResult;
            }

            try
            {

                String SQL = "SELECT SID,company_name FROM company LIMIT 0,1";
                DataTable companyDataTable = SQLDataTableModel.GetDataTable(SQL);
                if (companyDataTable.Rows.Count > 0)
                {
                    engine.SetValue("store_name", companyDataTable.Rows[0]["company_name"].ToString());
                }

                engine.SetValue("input", StrInput);//設定輸入值

                String StrFunName = "Main()";
                var StrJsonResult = engine.Execute(StrFunName).GetCompletionValue();//執行範本運算

                PT_CommandOutputs = new PT_CommandOutput();
                PT_CommandOutputs = JsonSerializer.Deserialize<PT_CommandOutput>(StrJsonResult.AsString());//取回運算結果
            }
            catch (Exception ex)
            {
                PT_CommandOutputs = null;
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Report", StrInput, ex.ToString());
                    LogFile.Write("JintErrot ; " + StrLog);
                }
            }

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0))
            {
                for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                {
                    PT_CommandOutputs.value[i] = UnescapeUnicode(PT_CommandOutputs.value[i]);
                }

                blnResult = true;
            }

            return blnResult;
        }

        public static bool ESCPOS_Report(String StrInput, out PT_CommandOutput PT_CommandOutputs)//報表
        {
            bool blnResult = false;
            PT_CommandOutputs = null;
            var engine = new Engine();

            if ((SQLDataTableModel.m_printer_templateDataTable != null) && (SQLDataTableModel.m_printer_valueList != null) && (SQLDataTableModel.m_printer_valueList.Count > 0) && (SQLDataTableModel.m_printer_templateDataTable.Rows.Count > 0))//判斷列印範本資料表有資料
            {
                //m_printer_templateDataTable --印表範本資料表全部資料
                //m_printer_valueList --存放所有UI印表機設定值

                //---
                //從DB載入對應範本到engine中
                String Strprint_templateSID = "-1";//預設值確保DB查不到
                for (int i = 0; i < SQLDataTableModel.m_printer_valueList.Count; i++)
                {
                    if (SQLDataTableModel.m_printer_valueList[i].template_type == "REPORT")//if (SQLDataTableModel.m_printer_valueList[i].output_type == "R")
                    {
                        /*
                        String Strprint_template = SQLDataTableModel.m_printer_valueList[i].printer_config_value.print_template;//顯示名稱;DB_SID
                        if (Strprint_template.Length > 0)
                        {
                            Strprint_templateSID = Strprint_template.Split(";")[1];
                        }
                        */
                        Strprint_templateSID = SQLDataTableModel.m_printer_valueList[i].template_sid;
                        break;
                    }
                }

                String expression01 = $"SID = '{Strprint_templateSID}'";
                DataRow[] foundRows01 = SQLDataTableModel.m_printer_templateDataTable.Select(expression01);
                if (foundRows01.Length > 0)
                {
                    if (foundRows01[0]["include_command"].ToString() == "Y")//有引用共用函數
                    {
                        String expression02 = $"template_type = 'ESC_COMMAND'";
                        DataRow[] foundRows02 = SQLDataTableModel.m_printer_templateDataTable.Select(expression02);
                        if (foundRows02.Length > 0)
                        {
                            engine.Execute(foundRows02[0]["template_value"].ToString());//共用範本載入
                        }
                    }
                    engine.Execute(foundRows01[0]["template_value"].ToString());//REPORT(報表)範本載入
                }
                else
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Report", StrInput, "printer_templateDataTable.foundRows.Count=0");
                    LogFile.Write("JintErrot ; " + StrLog);
                    return blnResult;
                }
                //---從DB載入對應範本到engine中
            }
            else
            {
                String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Report", StrInput, "printer_templateDataTable.Rows.Count=0");
                LogFile.Write("JintErrot ; " + StrLog);
                return blnResult;
            }

            try
            {
                engine.SetValue("input", StrInput);//設定輸入值

                String StrFunName = "Main()";
                var StrJsonResult = engine.Execute(StrFunName).GetCompletionValue();//執行範本運算

                PT_CommandOutputs = new PT_CommandOutput();
                PT_CommandOutputs = JsonSerializer.Deserialize<PT_CommandOutput>(StrJsonResult.AsString());//取回運算結果
            }
            catch (Exception ex)
            {
                PT_CommandOutputs = null;
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Report", StrInput, ex.ToString());
                    LogFile.Write("JintErrot ; " + StrLog);
                }
            }

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0))
            {
                for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                {
                    PT_CommandOutputs.value[i] = UnescapeUnicode(PT_CommandOutputs.value[i]);
                }

                blnResult = true;
            }

            return blnResult;
        }


        public static bool ESCPOS_Smart(String StrInput, out PT_CommandOutput PT_CommandOutputs)//智能食譜
        {
            bool blnResult = false;
            PT_CommandOutputs = null;
            var engine = new Engine();

            if ((SQLDataTableModel.m_printer_templateDataTable != null) && (SQLDataTableModel.m_printer_valueList != null) && (SQLDataTableModel.m_printer_valueList.Count > 0) && (SQLDataTableModel.m_printer_templateDataTable.Rows.Count > 0))//判斷列印範本資料表有資料
            {
                //m_printer_templateDataTable --印表範本資料表全部資料
                //m_printer_valueList --存放所有UI印表機設定值

                //---
                //從DB載入對應範本到engine中
                String Strprint_templateSID = "-1";//預設值確保DB查不到
                for (int i = 0; i < SQLDataTableModel.m_printer_valueList.Count; i++)
                {
                    if (SQLDataTableModel.m_printer_valueList[i].template_type == "SMART")//if (SQLDataTableModel.m_printer_valueList[i].output_type == "S")
                    {
                        /*
                        String Strprint_template = SQLDataTableModel.m_printer_valueList[i].printer_config_value.print_template;//顯示名稱;DB_SID
                        if (Strprint_template.Length > 0)
                        {
                            Strprint_templateSID = Strprint_template.Split(";")[1];
                        }
                        */
                        Strprint_templateSID = SQLDataTableModel.m_printer_valueList[i].template_sid;
                        break;
                    }
                }

                String expression01 = $"SID = '{Strprint_templateSID}'";
                DataRow[] foundRows01 = SQLDataTableModel.m_printer_templateDataTable.Select(expression01);
                if (foundRows01.Length > 0)
                {
                    if (foundRows01[0]["include_command"].ToString() == "Y")//有引用共用函數
                    {
                        String expression02 = $"template_type = 'ESC_COMMAND'";
                        DataRow[] foundRows02 = SQLDataTableModel.m_printer_templateDataTable.Select(expression02);
                        if (foundRows02.Length > 0)
                        {
                            engine.Execute(foundRows02[0]["template_value"].ToString());//共用範本載入
                        }
                    }
                    engine.Execute(foundRows01[0]["template_value"].ToString());//SMART(智能食譜)範本載入
                }
                else
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Smart", StrInput, "printer_templateDataTable.foundRows.Count=0");
                    LogFile.Write("JintErrot ; " + StrLog);
                    return blnResult;
                }
                //---從DB載入對應範本到engine中
            }
            else
            {
                String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Smart", StrInput, "printer_templateDataTable.Rows.Count=0");
                LogFile.Write("JintErrot ; " + StrLog);
                return blnResult;
            }

            try
            {
                engine.SetValue("input", StrInput);//設定輸入值

                String StrFunName = "Main()";
                var StrJsonResult = engine.Execute(StrFunName).GetCompletionValue();//執行範本運算

                PT_CommandOutputs = new PT_CommandOutput();
                PT_CommandOutputs = JsonSerializer.Deserialize<PT_CommandOutput>(StrJsonResult.AsString());//取回運算結果
            }
            catch (Exception ex)
            {
                PT_CommandOutputs = null;
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Smart", StrInput, ex.ToString());
                    LogFile.Write("JintErrot ; " + StrLog);
                }
            }

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0))
            {
                for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                {
                    PT_CommandOutputs.value[i] = UnescapeUnicode(PT_CommandOutputs.value[i]);
                }

                blnResult = true;
            }

            return blnResult;
        }

        public static bool EasyCard_Bill(String StrInput, out PT_CommandOutput PT_CommandOutputs)//悠遊卡收據
        {
            bool blnResult = false;
            PT_CommandOutputs = null;
            var engine = new Engine();

            if ((SQLDataTableModel.m_printer_templateDataTable != null) && (SQLDataTableModel.m_printer_valueList != null) && (SQLDataTableModel.m_printer_valueList.Count > 0) && (SQLDataTableModel.m_printer_templateDataTable.Rows.Count > 0))//判斷列印範本資料表有資料
            {
                //m_printer_templateDataTable --印表範本資料表全部資料
                //m_printer_valueList --存放所有UI印表機設定值

                //---
                //從DB載入對應範本到engine中
                String Strtemplate_type = "EASYCARD_BILL";//預設值確保DB查不到

                String expression01 = $"template_type = '{Strtemplate_type}'";
                DataRow[] foundRows01 = SQLDataTableModel.m_printer_templateDataTable.Select(expression01);
                if (foundRows01.Length > 0)
                {
                    if (foundRows01[0]["include_command"].ToString() == "Y")//有引用共用函數
                    {
                        String expression02 = $"template_type = 'ESC_COMMAND'";
                        DataRow[] foundRows02 = SQLDataTableModel.m_printer_templateDataTable.Select(expression02);
                        if (foundRows02.Length > 0)
                        {
                            engine.Execute(foundRows02[0]["template_value"].ToString());//共用範本載入
                        }
                    }
                    engine.Execute(foundRows01[0]["template_value"].ToString());//BILL(收據)範本載入
                }
                else
                {
                    String StrLog = String.Format("{0}: {1};{2}", "EasyCard_Bill", StrInput, "printer_templateDataTable.foundRows.Count=0");
                    LogFile.Write("JintErrot ; " + StrLog);
                    return blnResult;
                }
                //---從DB載入對應範本到engine中
            }
            else
            {
                String StrLog = String.Format("{0}: {1};{2}", "EasyCard_Bill", StrInput, "printer_templateDataTable.Rows.Count=0");
                LogFile.Write("JintErrot ; " + StrLog);
                return blnResult;
            }

            try
            {

                String SQL = "SELECT SID,company_name FROM company LIMIT 0,1";
                DataTable companyDataTable = SQLDataTableModel.GetDataTable(SQL);
                if (companyDataTable.Rows.Count > 0)
                {
                    engine.SetValue("store_name", companyDataTable.Rows[0]["company_name"].ToString());
                }

                engine.SetValue("input", StrInput);//設定輸入值

                String StrFunName = "Main()";
                var StrJsonResult = engine.Execute(StrFunName).GetCompletionValue();//執行範本運算

                PT_CommandOutputs = new PT_CommandOutput();
                PT_CommandOutputs = JsonSerializer.Deserialize<PT_CommandOutput>(StrJsonResult.AsString());//取回運算結果
            }
            catch (Exception ex)
            {
                PT_CommandOutputs = null;
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Bill", StrInput, ex.ToString());
                    LogFile.Write("JintErrot ; " + StrLog);
                }
            }

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0))
            {
                for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                {
                    PT_CommandOutputs.value[i] = UnescapeUnicode(PT_CommandOutputs.value[i]);
                }

                blnResult = true;
            }

            return blnResult;
        }

        public static bool NCCC_Bill(String StrInput, out PT_CommandOutput PT_CommandOutputs)//聯信信用卡小額交易持卡人存根
        {
            bool blnResult = false;
            PT_CommandOutputs = null;
            var engine = new Engine();

            if ((SQLDataTableModel.m_printer_templateDataTable != null) && (SQLDataTableModel.m_printer_valueList != null) && (SQLDataTableModel.m_printer_valueList.Count > 0) && (SQLDataTableModel.m_printer_templateDataTable.Rows.Count > 0))//判斷列印範本資料表有資料
            {
                //m_printer_templateDataTable --印表範本資料表全部資料
                //m_printer_valueList --存放所有UI印表機設定值

                //---
                //從DB載入對應範本到engine中
                String Strtemplate_type = "CREDIE_CARD_RECEIPT";//預設值確保DB查不到

                String expression01 = $"template_type = '{Strtemplate_type}'";
                DataRow[] foundRows01 = SQLDataTableModel.m_printer_templateDataTable.Select(expression01);
                if (foundRows01.Length > 0)
                {
                    if (foundRows01[0]["include_command"].ToString() == "Y")//有引用共用函數
                    {
                        String expression02 = $"template_type = 'ESC_COMMAND'";
                        DataRow[] foundRows02 = SQLDataTableModel.m_printer_templateDataTable.Select(expression02);
                        if (foundRows02.Length > 0)
                        {
                            engine.Execute(foundRows02[0]["template_value"].ToString());//共用範本載入
                        }
                    }
                    engine.Execute(foundRows01[0]["template_value"].ToString());//BILL(收據)範本載入
                }
                else
                {
                    String StrLog = String.Format("{0}: {1};{2}", "NCCC_Bill", StrInput, "printer_templateDataTable.foundRows.Count=0");
                    LogFile.Write("JintErrot ; " + StrLog);
                    return blnResult;
                }
                //---從DB載入對應範本到engine中
            }
            else
            {
                String StrLog = String.Format("{0}: {1};{2}", "NCCC_Bill", StrInput, "printer_templateDataTable.Rows.Count=0");
                LogFile.Write("JintErrot ; " + StrLog);
                return blnResult;
            }

            try
            {

                String SQL = "SELECT SID,company_name FROM company LIMIT 0,1";
                DataTable companyDataTable = SQLDataTableModel.GetDataTable(SQL);
                if (companyDataTable.Rows.Count > 0)
                {
                    engine.SetValue("store_name", companyDataTable.Rows[0]["company_name"].ToString()); 
                }

                engine.SetValue("input", StrInput);//設定輸入值

                String StrFunName = "Main()";
                var StrJsonResult = engine.Execute(StrFunName).GetCompletionValue();//執行範本運算

                PT_CommandOutputs = new PT_CommandOutput();
                PT_CommandOutputs = JsonSerializer.Deserialize<PT_CommandOutput>(StrJsonResult.AsString());//取回運算結果
            }
            catch (Exception ex)
            {
                PT_CommandOutputs = null;
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "NCCC_Bill", StrInput, ex.ToString());
                    LogFile.Write("JintErrot ; " + StrLog);
                }
            }

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0))
            {
                for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                {
                    PT_CommandOutputs.value[i] = UnescapeUnicode(PT_CommandOutputs.value[i]);
                }

                blnResult = true;
            }

            return blnResult;
        }

        public static bool ESCPOS_Bill(String StrInput, out PT_CommandOutput PT_CommandOutputs)//收據
        {
            bool blnResult = false;
            PT_CommandOutputs = null;
            var engine = new Engine();

            if ((SQLDataTableModel.m_printer_templateDataTable != null) && (SQLDataTableModel.m_printer_valueList!=null) && (SQLDataTableModel.m_printer_valueList.Count>0) && (SQLDataTableModel.m_printer_templateDataTable.Rows.Count > 0))//判斷列印範本資料表有資料
            {
                //m_printer_templateDataTable --印表範本資料表全部資料
                //m_printer_valueList --存放所有UI印表機設定值

                //---
                //從DB載入對應範本到engine中
                String Strprint_templateSID = "-1";//預設值確保DB查不到
                for (int i=0;i< SQLDataTableModel.m_printer_valueList.Count;i++)
                {
                    if (SQLDataTableModel.m_printer_valueList[i].template_type == "BILL")//if(SQLDataTableModel.m_printer_valueList[i].output_type=="B")
                    {
                        /*
                        String Strprint_template = SQLDataTableModel.m_printer_valueList[i].printer_config_value.print_template;//顯示名稱;DB_SID
                        if(Strprint_template.Length>0)
                        {
                            Strprint_templateSID = Strprint_template.Split(";")[1];
                        }
                        */
                        Strprint_templateSID = SQLDataTableModel.m_printer_valueList[i].template_sid;
                        break;
                    }
                }

                String expression01 = $"SID = '{Strprint_templateSID}'";
                DataRow[] foundRows01 = SQLDataTableModel.m_printer_templateDataTable.Select(expression01);
                if (foundRows01.Length > 0)
                {
                    if (foundRows01[0]["include_command"].ToString() == "Y")//有引用共用函數
                    {
                        String expression02 = $"template_type = 'ESC_COMMAND'";
                        DataRow[] foundRows02 = SQLDataTableModel.m_printer_templateDataTable.Select(expression02);
                        if (foundRows02.Length > 0)
                        {
                            engine.Execute(foundRows02[0]["template_value"].ToString());//共用範本載入
                        }
                    }
                    engine.Execute(foundRows01[0]["template_value"].ToString());//BILL(收據)範本載入
                }
                else
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Bill", StrInput, "printer_templateDataTable.foundRows.Count=0");
                    LogFile.Write("JintErrot ; " + StrLog);
                    return blnResult;
                }
                //---從DB載入對應範本到engine中
            }
            else
            {
                String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Bill", StrInput, "printer_templateDataTable.Rows.Count=0");
                LogFile.Write("JintErrot ; " + StrLog);
                return blnResult;
            }
        
            try
            {
                engine.SetValue("input", StrInput);//設定輸入值

                String StrFunName = "Main()";
                var StrJsonResult = engine.Execute(StrFunName).GetCompletionValue();//執行範本運算

                PT_CommandOutputs = new PT_CommandOutput();
                PT_CommandOutputs = JsonSerializer.Deserialize<PT_CommandOutput>(StrJsonResult.AsString());//取回運算結果
            }
            catch (Exception ex)
            {
                PT_CommandOutputs = null;
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Bill", StrInput, ex.ToString());
                    LogFile.Write("JintErrot ; " + StrLog);
                }
            }

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0))
            {
                for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                {
                    PT_CommandOutputs.value[i] = UnescapeUnicode(PT_CommandOutputs.value[i]);
                }

                blnResult = true;
            }

            return blnResult;
        }

        public static bool ESCPOS_Cash(out PT_CommandOutput PT_CommandOutputs)//錢箱(ESC指令)
        {
            bool blnResult = false;
            PT_CommandOutputs = null;
            var engine = new Engine();

            if ((SQLDataTableModel.m_printer_templateDataTable != null) && (SQLDataTableModel.m_printer_valueList != null) && (SQLDataTableModel.m_printer_valueList.Count > 0) && (SQLDataTableModel.m_printer_templateDataTable.Rows.Count > 0))//判斷列印範本資料表有資料
            {
                //m_printer_templateDataTable --印表範本資料表全部資料
                //m_printer_valueList --存放所有UI印表機設定值

                //---
                //從DB載入對應範本到engine中
                String Strprint_templateSID = "-1";//預設值確保DB查不到
                for (int i = 0; i < SQLDataTableModel.m_printer_valueList.Count; i++)
                {
                    if (SQLDataTableModel.m_printer_valueList[i].template_type == "CASH")
                    {
                        Strprint_templateSID = SQLDataTableModel.m_printer_valueList[i].template_sid;
                        break;
                    }
                }

                String expression01 = $"SID = '{Strprint_templateSID}'";
                DataRow[] foundRows01 = SQLDataTableModel.m_printer_templateDataTable.Select(expression01);
                if (foundRows01.Length > 0)
                {
                    if (foundRows01[0]["include_command"].ToString() == "Y")//有引用共用函數
                    {
                        String expression02 = $"template_type = 'ESC_COMMAND'";
                        DataRow[] foundRows02 = SQLDataTableModel.m_printer_templateDataTable.Select(expression02);
                        if (foundRows02.Length > 0)
                        {
                            engine.Execute(foundRows02[0]["template_value"].ToString());//共用範本載入
                        }
                    }
                    engine.Execute(foundRows01[0]["template_value"].ToString());//CASH範本載入
                }
                else
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Cash", "", "printer_templateDataTable.foundRows.Count=0");
                    LogFile.Write("JintErrot ; " + StrLog);
                    return blnResult;
                }
                //---從DB載入對應範本到engine中
            }
            else
            {
                String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Cash", "", "printer_templateDataTable.Rows.Count=0");
                LogFile.Write("JintErrot ; " + StrLog);
                return blnResult;
            }

            try
            {
                String StrFunName = "Main()";
                var StrJsonResult = engine.Execute(StrFunName).GetCompletionValue();//執行範本運算

                PT_CommandOutputs = new PT_CommandOutput();
                PT_CommandOutputs = JsonSerializer.Deserialize<PT_CommandOutput>(StrJsonResult.AsString());//取回運算結果
            }
            catch (Exception ex)
            {
                PT_CommandOutputs = null;
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Cash", "", ex.ToString());
                    LogFile.Write("JintErrot ; " + StrLog);
                }
            }

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0))
            {
                for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                {
                    PT_CommandOutputs.value[i] = UnescapeUnicode(PT_CommandOutputs.value[i]);
                }

                blnResult = true;
            }

            return blnResult;
        }

        public static bool ESCPOS_Invoice(String StrInput, String StrInvoice, out PT_CommandOutput PT_CommandOutputs)//電子發票
        {
            bool blnResult = false;
            PT_CommandOutputs = null;
            var engine = new Engine();

            if ((SQLDataTableModel.m_printer_templateDataTable != null) && (SQLDataTableModel.m_printer_valueList != null) && (SQLDataTableModel.m_printer_valueList.Count > 0) && (SQLDataTableModel.m_printer_templateDataTable.Rows.Count > 0))//判斷列印範本資料表有資料
            {
                //m_printer_templateDataTable --印表範本資料表全部資料
                //m_printer_valueList --存放所有UI印表機設定值

                //---
                //從DB載入對應範本到engine中
                String Strtemplate_type = "INVOICE";//預設值確保DB查不到

                String expression01 = $"template_type = '{Strtemplate_type}'";
                DataRow[] foundRows01 = SQLDataTableModel.m_printer_templateDataTable.Select(expression01);
                if (foundRows01.Length > 0)
                {
                    if (foundRows01[0]["include_command"].ToString() == "Y")//有引用共用函數
                    {
                        String expression02 = $"template_type = 'ESC_COMMAND'";
                        DataRow[] foundRows02 = SQLDataTableModel.m_printer_templateDataTable.Select(expression02);
                        if (foundRows02.Length > 0)
                        {
                            engine.Execute(foundRows02[0]["template_value"].ToString());//共用範本載入
                        }
                    }
                    engine.Execute(foundRows01[0]["template_value"].ToString());//BILL(收據)範本載入
                }
                else
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Invoice", StrInvoice, "printer_templateDataTable.foundRows.Count=0");
                    LogFile.Write("JintErrot ; " + StrLog);
                    return blnResult;
                }
                //---從DB載入對應範本到engine中
            }
            else
            {
                String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Invoice", StrInvoice, "printer_templateDataTable.Rows.Count=0");
                LogFile.Write("JintErrot ; " + StrLog);
                return blnResult;
            }

            try
            {
                POSOrder2InvoiceB2COrder POSOrder2InvoiceB2COrderBuf = JsonClassConvert.POSOrder2InvoiceB2COrder2Class(StrInvoice);
                if(POSOrder2InvoiceB2COrderBuf.Invalid_Flag!="Y")//購買情況
                {
                    if ((POSOrder2InvoiceB2COrderBuf.Carrier_Code_1.Length > 0) || (POSOrder2InvoiceB2COrderBuf.Donate_Code.Length > 0))
                    {//有載具資料或捐贈，不用列印發票
                        PT_CommandOutputs = null;
                        blnResult = false;
                        return blnResult;
                    }
                }
                //退貨情況不管有無客戶統編 & 有無載具 都要列印退貨明細


                String StrQRCode_Value_1 = POSOrder2InvoiceB2COrderBuf.QRCode_Value_1;
                //String StrQRCode_Buf = POSOrder2InvoiceB2COrderBuf.QRCode_Value_2.Substring(2);
                //StrQRCode_Buf = "**" + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(StrQRCode_Buf));
                //StrQRCode_Buf = (StrQRCode_Buf.Length >= 200) ? (StrQRCode_Buf.Substring(0, 200)) : (StrQRCode_Buf.PadRight((200 - StrQRCode_Buf.Length), ' '));
                String StrQRCode_Value_2 = "**".PadRight(250, ' ');//StrQRCode_Buf;//         
                String StrBarCode_Value = POSOrder2InvoiceB2COrderBuf.BarCode_Value;

                engine.SetValue("Business_Name", SqliteDataAccess.m_company[0].business_name);
                engine.SetValue("Com_EIN", SqliteDataAccess.m_company[0].EIN);//賣方統一編號
                engine.SetValue("Reprint", (PrintThread.m_blnReprintInvoice)?"Y":"N");//補印
                engine.SetValue("Sandbox", POS_INVAPI.m_InvParams.Sandbox);//測試
                engine.SetValue("input", StrInput);//設定輸入值
                engine.SetValue("Invoice", StrInvoice);
                engine.SetValue("QRCode_Value_1", StrQRCode_Value_1);
                engine.SetValue("QRCode_Value_2", StrQRCode_Value_2);//**波霸紅茶:1:50:養樂多綠(大):1:50:
                engine.SetValue("BarCode_Value", StrBarCode_Value);
                String StrFunName = "Main()";
                var StrJsonResult = engine.Execute(StrFunName).GetCompletionValue();//執行範本運算

                PT_CommandOutputs = new PT_CommandOutput();
                PT_CommandOutputs = JsonSerializer.Deserialize<PT_CommandOutput>(StrJsonResult.AsString());//取回運算結果
            }
            catch (Exception ex)
            {
                PT_CommandOutputs = null;
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_Invoice", StrInput, ex.ToString());
                    LogFile.Write("JintErrot ; " + StrLog);
                }
            }

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0))
            {
                for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                {
                    PT_CommandOutputs.value[i] = UnescapeUnicode(PT_CommandOutputs.value[i]);
                }

                blnResult = true;
            }

            return blnResult;
        }

        public static bool ESCPOS_QrCode(String StrInput, out PT_CommandOutput PT_CommandOutputs)//掃碼點單
        {
            bool blnResult = false;
            PT_CommandOutputs = null;
            var engine = new Engine();

            if ((SQLDataTableModel.m_printer_templateDataTable != null) && (SQLDataTableModel.m_printer_valueList != null) && (SQLDataTableModel.m_printer_valueList.Count > 0) && (SQLDataTableModel.m_printer_templateDataTable.Rows.Count > 0))//判斷列印範本資料表有資料
            {
                //m_printer_templateDataTable --印表範本資料表全部資料
                //m_printer_valueList --存放所有UI印表機設定值

                //---
                //從DB載入對應範本到engine中
                String Strprint_templateSID = "-1";//預設值確保DB查不到
                for (int i = 0; i < SQLDataTableModel.m_printer_valueList.Count; i++)
                {
                    if (SQLDataTableModel.m_printer_valueList[i].template_type == "QrOrder")
                    {
                        Strprint_templateSID = SQLDataTableModel.m_printer_valueList[i].template_sid;
                        break;
                    }
                }

                String expression01 = $"SID = '{Strprint_templateSID}'";
                DataRow[] foundRows01 = SQLDataTableModel.m_printer_templateDataTable.Select(expression01);
                if (foundRows01.Length > 0)
                {
                    if (foundRows01[0]["include_command"].ToString() == "Y")//有引用共用函數
                    {
                        String expression02 = $"template_type = 'ESC_COMMAND'";
                        DataRow[] foundRows02 = SQLDataTableModel.m_printer_templateDataTable.Select(expression02);
                        if (foundRows02.Length > 0)
                        {
                            engine.Execute(foundRows02[0]["template_value"].ToString());//共用範本載入
                        }
                    }
                    engine.Execute(foundRows01[0]["template_value"].ToString());//掃碼點單範本載入
                }
                else
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_QrCode", StrInput, "printer_templateDataTable.foundRows.Count=0");
                    LogFile.Write("JintErrot ; " + StrLog);
                    return blnResult;
                }
                //---從DB載入對應範本到engine中
            }
            else
            {
                String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_QrCode", StrInput, "printer_templateDataTable.Rows.Count=0");
                LogFile.Write("JintErrot ; " + StrLog);
                return blnResult;
            }

            try
            {
                engine.SetValue("input", StrInput);//設定輸入值

                String StrFunName = "Main()";
                var StrJsonResult = engine.Execute(StrFunName).GetCompletionValue();//執行範本運算

                PT_CommandOutputs = new PT_CommandOutput();
                PT_CommandOutputs = JsonSerializer.Deserialize<PT_CommandOutput>(StrJsonResult.AsString());//取回運算結果
            }
            catch (Exception ex)
            {
                PT_CommandOutputs = null;
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "ESCPOS_QrCode", StrInput, ex.ToString());
                    LogFile.Write("JintErrot ; " + StrLog);
                }
            }

            if ((PT_CommandOutputs != null) && (PT_CommandOutputs.state_code == 0))
            {
                for (int i = 0; i < PT_CommandOutputs.value.Count; i++)
                {
                    PT_CommandOutputs.value[i] = UnescapeUnicode(PT_CommandOutputs.value[i]);
                }

                blnResult = true;
            }

            return blnResult;
        }

    }//PrinterTemplate
}
