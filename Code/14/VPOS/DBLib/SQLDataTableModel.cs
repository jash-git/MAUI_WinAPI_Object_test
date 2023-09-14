using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;//SQLiteConnection
using Dapper;//DynamicParameters
using System.Data;//IDbConnection
using System.Configuration;//ConfigurationManager
using System.Text.RegularExpressions;
using System.Diagnostics.Metrics;
using System.Web;
using System.Drawing;


namespace VPOS
{
    public class SQLDataTableModel
    {
        private static bool m_blnlogfile = true;

        public static string UUID2SID(int len=32)
        {
            String StrResult = "";

            Guid guid = Guid.NewGuid();
            byte[] guidBytes = guid.ToByteArray();
            byte[] result = new byte[64];
            Buffer.BlockCopy(guidBytes, 0, result, 0, 16);
            Buffer.BlockCopy(guidBytes, 0, result, 16, 16);
            Buffer.BlockCopy(guidBytes, 0, result, 32, 16);
            Buffer.BlockCopy(guidBytes, 0, result, 48, 16);

            StrResult = BitConverter.ToString(result).Replace("-", "");
            if(len<=128)
            {
                StrResult = StrResult.Substring(0,len);
            }

            return StrResult;
        }
        public static string ConnectionStringLoad(string id = "Default")
        {
            //return Cryption.AesDecrypt(ConfigurationManager.ConnectionStrings[id].ConnectionString, "0123456789987654"); //ConfigurationManager.ConnectionStrings[id].ConnectionString;
            String StrResult="";
            switch(id)
            {
                case "Default":
                    StrResult = Cryption.AesDecrypt("PnJr/hxqamOJC/sUCd4T0dk7zjNWeNME0IanujV7jOybNgYLGna0zIs1f21ITwz3SMfYOt6ttz+MrBBMV/hC0zkfpyVOuqtryGj8H4Z/jKI5JJaKyDN3HS1sRQ0xzk8cjHyolFTENltSoYLRGxrdJNogqCz33hNl6RmsbzdzOclF/GLh3nyHY5eD3xjJINNBFdTqZxtcPJWWVwx8/hJD1q5+IXw/hNYW1zi59t42Fu4=", "0123456789987654");
                    break;
                case "Synchronize":
                    StrResult = Cryption.AesDecrypt("vv7U+j7ISnOI2aLG/KwSrOEcK7bdwzXdk9fGE3URlmwF3bA23QVu9umwPaO7ZxMKdsLwwcUMdQqQQPxIENP2pW9lz1g0lRFnQ+Z17l4bl3BLA7ygZFRoXi56Q2v2Cad0VkBGhjWewFAj+SREepKj7vB3ygnkaQFJGVJHcSS1gF86rCGvBNQ9jCoFEqR325NXA/GpKL7WKj26eq33rVwFGJZ58l+lTJo/N0507Xx1GfwATOAN1t98T8C9t+qjn/yc", "0123456789987654");
                    break;
                case "Takeaways":
                    StrResult = Cryption.AesDecrypt("vnul9w5EOmC8lC0tqJKIO5GhRgtq7O4r444s4xn9+Qeqf0NguXUdv0mSXMFZLqXllS6KIo4u/9utY8vx7rFEIj797eEmXi1hhoEqeDxzpBBpQgOFNpJFu2S0I99QXniaEUdy2vdasnr/51sFHwxKGHxrTnRHkYh1cXTPeI+jYb9VYgf32HXVyQTb4uQoel78LBTFDcl/fEZ+H12HDlyynh7n3yZCmQVD58zjkxmq/8hUOvPH0VlLrlFZnWMgv5gD", "0123456789987654");
                    break;
            }
            return StrResult;
        }

        //---
        //C# 通用操作語法 ~ for System.Data.SQLite.Core & Finisar.SQLite 元件 
        //https://einboch.pixnet.net/blog/post/248187728
        public static SQLiteConnection OpenConn(string Database,bool blnAppSet=true)//資料庫連線程式
        {
            string cnstr = (blnAppSet)? ConnectionStringLoad(Database) : String.Format("Data Source={0};Version=3;", Database);
            SQLiteConnection icn = new SQLiteConnection();
            icn.ConnectionString = cnstr;
            if (icn.State == ConnectionState.Open) { icn.Close(); icn = null; /*GC.Collect();*/ }
            icn.Open();
            return icn;
        }

        //public static SQLiteConnection m_mainICN = null;
        public static SQLiteConnection OpenConn()//資料庫連線程式
        {
            string cnstr = ConnectionStringLoad("Default");
            SQLiteConnection icn = new SQLiteConnection();
            icn.ConnectionString = cnstr;
            if (icn.State == ConnectionState.Open) { icn.Close(); icn = null; /*GC.Collect();*/ }
            icn.Open();
            return icn;
            //if(m_mainICN==null)
            //{
            //    string cnstr = ConnectionStringLoad("Default");
            //    m_mainICN = new SQLiteConnection();
            //    m_mainICN.ConnectionString = cnstr;
            //    m_mainICN.Open();
            //}
            //return m_mainICN;
        }
        public static void CreateSQLiteDatabase(string Database)//建立資料庫程式
        {
            string cnstr = string.Format("Data Source=" + Database + ";Version=3;New=True;Compress=True;");
            SQLiteConnection icn = new SQLiteConnection();
            icn.ConnectionString = cnstr;
            icn.Open();
            { icn.Close(); icn = null; /*GC.Collect();*/ }
        }

        public static void CreateSQLiteTable(string Database, string CreateTableString)//建立資料表程式
        {
            LogFile.SQLRecord(CreateTableString);
            SQLiteConnection icn = OpenConn(Database,false);
            SQLiteCommand cmd = new SQLiteCommand(CreateTableString, icn);
            SQLiteTransaction mySqlTransaction = icn.BeginTransaction();
            try
            {
                cmd.Transaction = mySqlTransaction;
                cmd.ExecuteNonQuery();
                mySqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                mySqlTransaction.Rollback();
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "CreateSQLiteTable", CreateTableString, ex.ToString());
                    LogFile.Write("SQLError ; " + StrLog);
                }
            }
            if (icn.State == ConnectionState.Open) { icn.Close(); icn = null; /*GC.Collect();*/ }
        }

        public static void SQLiteInsertUpdateDelete(string Database, string SqlSelectString)//新增資料程式
        {
            SyncThread.m_blnWait = true;
            LogFile.SQLRecord(SqlSelectString);
            SQLiteConnection icn = OpenConn(Database);
            SQLiteCommand cmd = new SQLiteCommand(SqlSelectString, icn);
            SQLiteTransaction mySqlTransaction = icn.BeginTransaction();
            try
            {
                cmd.Transaction = mySqlTransaction;
                cmd.ExecuteNonQuery();
                mySqlTransaction.Commit();
                if (m_blnlogfile)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "sync_SQLiteInsertUpdateDelete", SqlSelectString, "success");
                    LogFile.Write("SQLNormal ; " + StrLog);
                }
            }
            catch (Exception ex)
            {
                mySqlTransaction.Rollback();
                //throw (ex);
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "sync_SQLiteInsertUpdateDelete", SqlSelectString, ex.ToString());
                    LogFile.Write("SQLError ; " + StrLog);
                }
            }
            if (icn.State == ConnectionState.Open) { icn.Close(); icn = null; /*GC.Collect();*/ }

            SyncThread.m_blnWait = false;
        }

        public static void SQLiteInsertUpdateDelete(string SqlSelectString)//新增資料程式
        {
            LogFile.SQLRecord(SqlSelectString);
            // The critical section.
            SQLiteConnection icn = OpenConn();//OpenConn(Database);
            SQLiteCommand cmd = new SQLiteCommand(SqlSelectString, icn);
            SQLiteTransaction mySqlTransaction = icn.BeginTransaction();
            try
            {
                cmd.Transaction = mySqlTransaction;
                cmd.ExecuteNonQuery();
                mySqlTransaction.Commit();
                if (m_blnlogfile)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "vpos_SQLiteInsertUpdateDelete", SqlSelectString, "success");
                    LogFile.Write("SQLNormal ; " + StrLog);
                }
            }
            catch (Exception ex)
            {
                mySqlTransaction.Rollback();
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "vpos_SQLiteInsertUpdateDelete", SqlSelectString, ex.ToString());
                    LogFile.Write("SQLError ; " + StrLog);
                }
            }
            if (icn.State == ConnectionState.Open) { icn.Close(); icn = null; /*GC.Collect();*/ }

        }

        public static void SQLiteInsertUpdateDelete(string commandText, DataTable dtData)//新增資料程式
        {
            LogFile.SQLRecord(commandText);
            // The critical section.
            String StrCommandText = "";
            SQLiteConnection icn = OpenConn();//OpenConn(Database);
            SQLiteTransaction mySqlTransaction = icn.BeginTransaction();
            try
            {
                foreach (DataRow row in dtData.Rows)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(commandText, icn))
                    {
                        for(int i = 0; i < row.Table.Columns.Count; i++) 
                        {
                            cmd.Parameters.Add(new SQLiteParameter($"@{row.Table.Columns[i].ColumnName}", row[i]));
                        }
                        cmd.Transaction = mySqlTransaction;
                        StrCommandText = cmd.CommandText;
                        cmd.ExecuteNonQuery();
                    }
                }

                mySqlTransaction.Commit();
                if (m_blnlogfile)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "vpos_SQLiteInsertUpdateDelete", StrCommandText, "success");
                    LogFile.Write("SQLNormal ; " + StrLog);
                }
            }
            catch (Exception ex)
            {
                mySqlTransaction.Rollback();
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "vpos_SQLiteInsertUpdateDelete", StrCommandText, ex.ToString());
                    LogFile.Write("SQLError ; " + StrLog);
                }
            }
            if (icn.State == ConnectionState.Open) { icn.Close(); icn = null; /*GC.Collect();*/ }

        }

        public static DataTable GetDataTable(string Database, string SQLiteString)//讀取資料程式
        {
            SyncThread.m_blnWait = true;
            LogFile.SQLRecord(SQLiteString);
            DataTable myDataTable = new DataTable(Database);
            try
            {

                SQLiteConnection icn = OpenConn(Database);
                SQLiteDataAdapter da = new SQLiteDataAdapter(SQLiteString, icn);
                DataSet ds = new DataSet();
                ds.Clear();
                da.Fill(ds);
                myDataTable = ds.Tables[0];
                da.Dispose();
                ds.Dispose();
                da = null;
                ds = null;
                if (icn.State == ConnectionState.Open) { icn.Close(); icn = null; /*GC.Collect();*/ }

                if (m_blnlogfile)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "sync_GetDataTable", SQLiteString, "success");
                    LogFile.Write("SQLNormal ; " + StrLog);
                }
            }
            catch (Exception ex)
            {
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "sync_GetDataTable", SQLiteString, ex.ToString());
                    LogFile.Write("SQLError ; " + StrLog);
                }
            }

            SyncThread.m_blnWait = false;

            return myDataTable;
        }

        public static DataTable GetDataTable(string SQLiteString)//讀取資料程式
        {
            DataTable myDataTable = new DataTable();
            LogFile.SQLRecord(SQLiteString);
            try
            {

                SQLiteConnection icn = OpenConn();//OpenConn(Database);
                SQLiteDataAdapter da = new SQLiteDataAdapter(SQLiteString, icn);
                DataSet ds = new DataSet();
                ds.Clear();
                da.Fill(ds);
                myDataTable = ds.Tables[0];
                da.Dispose();
                ds.Dispose();
                da = null;
                ds = null;
                if (icn.State == ConnectionState.Open) { icn.Close(); icn = null; /*GC.Collect();*/ }

                if (m_blnlogfile)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "vpos_GetDataTable", SQLiteString, "success");
                    LogFile.Write("SQLNormal ; " + StrLog);
                }
            }
            catch (Exception ex)
            {
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "vpos_GetDataTable", SQLiteString, ex.ToString());
                    LogFile.Write("SQLError ; " + StrLog);
                }
            }

            return myDataTable;
        }

        public static void DBBackupAndClean()//做一次實體DB檔案備份，並清空所有資料表資料
        {
            String DBName = "";
            String StrSourceFilePath = "";
            String StrDestFilePath = "";

            try
            {
                //---
                //inv_sync
                DBName = "inv_sync";
                StrSourceFilePath = FileLib.path + DBName + ".db";
                StrDestFilePath = FileLib.path + DBName + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".db";
                if (File.Exists(StrSourceFilePath))
                {
                    File.Copy(StrSourceFilePath, StrDestFilePath, true);
                    File.Delete(StrSourceFilePath);
                }
                //---inv_sync
            }
            catch (Exception ex) 
            {
                LogFile.Write("DBBackupAndClean Error:"+ex.ToString()); 
            }

            try
            {
                //---
                //takeaways
                DBName = "takeaways";
                StrSourceFilePath = FileLib.path + DBName + ".db";
                StrDestFilePath = FileLib.path + DBName + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".db";
                if (File.Exists(StrSourceFilePath))
                {
                    File.Copy(StrSourceFilePath, StrDestFilePath, true);
                    File.Delete(StrSourceFilePath);
                }
                //---takeaways
            }
            catch (Exception ex)
            {
                LogFile.Write("DBBackupAndClean Error:" + ex.ToString());
            }

            try
            {
                //---
                //vtcloud_sync
                DBName = "vtcloud_sync";
                StrSourceFilePath = FileLib.path + DBName + ".db";
                StrDestFilePath = FileLib.path + DBName + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".db";
                if (File.Exists(StrSourceFilePath))
                {
                    File.Copy(StrSourceFilePath, StrDestFilePath, true);
                    File.Delete(StrSourceFilePath);

                    DBName = LogFile.m_StrSysPath + "vtcloud_sync.db";
                    if (!File.Exists(DBName))
                    {
                        SQLDataTableModel.CreateSQLiteDatabase(DBName);
                        string CreateTableString = SQLDataTableModel.VPOSInitialTableSyntax("upload_data");
                        SQLDataTableModel.CreateSQLiteTable(DBName, CreateTableString);//建立資料表程式
                        LogFile.Write("SystemNormal ; vtcloud_sync.db Init");
                    }
                }
                //---vtcloud_sync
            }
            catch (Exception ex)
            {
                LogFile.Write("DBBackupAndClean Error:" + ex.ToString());
            }

            try
            {
                //---
                //vpos
                DBName = "vpos";
                StrSourceFilePath = FileLib.path + DBName + ".db";
                StrDestFilePath = FileLib.path + DBName + DateTime.Now.ToString("_yyyyMMddHHmmss") + ".db";
                if (File.Exists(StrSourceFilePath))
                {
                    File.Copy(StrSourceFilePath, StrDestFilePath, true);
                    File.Delete(StrSourceFilePath);

                    String Strvpos_def = LogFile.m_StrSysPath + "vpos_def.db";
                    String Strvpos = LogFile.m_StrSysPath + "vpos.db";
                    if (!((File.Exists(Strvpos_def)) && (!File.Exists(Strvpos))))
                    {
                        String SQL = "SELECT name FROM sqlite_schema WHERE type IN ('table','view') AND name NOT LIKE 'sqlite_%' ORDER BY 1;";//https://database.guide/2-ways-to-list-tables-in-sqlite-database/
                        DataTable DataTableBuf = GetDataTable(SQL);
                        if ((DataTableBuf != null) && (DataTableBuf.Rows.Count > 0))
                        {
                            for (int i = 0; i < DataTableBuf.Rows.Count; i++)
                            {
                                SQL = String.Format("DELETE FROM {0}", DataTableBuf.Rows[i][0].ToString());
                                SQLiteInsertUpdateDelete(SQL);
                            }
                        }
                    }
                    else
                    {
                        File.Copy(Strvpos_def, Strvpos, true);
                        LogFile.Write("SystemNormal ; vpos.db Init");
                    }
                }
                //---vpos
            }
            catch (Exception ex)
            {
                LogFile.Write("DBBackupAndClean Error:" + ex.ToString());
            }
        }

        public static void DBColumnsPadding(String StrTable, String StrColumn,String StrType,String StrPreset="null", bool blnAllClear=true, string Database = "")//資料庫(DB) 新增(補)欄位
        {
            String SQL = "";
            bool blnCheck = false;

            SQL = String.Format("SELECT * FROM {1} LIMIT 0,1", StrColumn, StrTable);
            DataTable chaekDataTable = (Database.Length==0)?GetDataTable(SQL): GetDataTable(Database, SQL);          
            if(chaekDataTable != null)
            {
                foreach (DataColumn column in chaekDataTable.Columns)
                {
                    if(column.ColumnName == StrColumn)
                    {
                        blnCheck = true;
                        break;
                    }
                }
                chaekDataTable.Dispose();
            }
            chaekDataTable = null;

            if (!blnCheck)
            {
                //---
                //新增對應欄位
                //ALTER TABLE MY_TABLE ADD COLUMN MISSING_COLUMN int null
                SQL = String.Format("ALTER TABLE {1} ADD COLUMN {0} {2}  DEFAULT {3}", StrColumn, StrTable, StrType,StrPreset);
                if(Database.Length == 0)
                {
                    SQLiteInsertUpdateDelete(SQL);
                }
                else
                {
                    SQLiteInsertUpdateDelete(Database,SQL);
                }
                //---新增對應欄位

                if(blnAllClear)
                {
                    //---
                    //新增欄位後，清空該資料表內容，藉此強迫觸發資料全部更新機制
                    SQL = String.Format("DELETE FROM {0}", StrTable);
                    if (Database.Length == 0)
                    {
                        SQLiteInsertUpdateDelete(SQL);
                    }
                    else
                    {
                        SQLiteInsertUpdateDelete(Database, SQL);
                    }
                    //---新增欄位後，清空該資料表內容，藉此強迫觸發資料全部更新機制
                }

            }
        }

        public static void DBColumnsModifying(String StrTable, String StrColumn, String StrType,string Database = "")//修改欄位
        {
            String SQL = "";
            bool blnCheck = false;
            SQL = String.Format("PRAGMA table_info({0})", StrTable, StrColumn, StrType);
            DataTable chaekDataTable = (Database.Length == 0) ? GetDataTable(SQL) : GetDataTable(Database, SQL);

            if (chaekDataTable != null)
            {
                bool blnDifference = false;
                //判斷欄位是否已是正確型態
                for (int i=0;i< chaekDataTable.Rows.Count;i++)
                {
                    if (chaekDataTable.Rows[i]["name"].ToString() == StrColumn)
                    {
                        if(chaekDataTable.Rows[i]["type"].ToString() != StrType)
                        {
                            blnDifference = true;
                        }
                        else
                        {
                            blnDifference = false;
                        }
                        break;
                    }
                }
                if(blnDifference) 
                {
                    //舊表更名
                    SQL = String.Format("ALTER TABLE {0} RENAME TO temp_{0}",StrTable, StrColumn, StrType);
                    if (Database.Length == 0)
                    {
                        SQLiteInsertUpdateDelete(SQL);
                    }
                    else
                    {
                        SQLiteInsertUpdateDelete(Database, SQL);
                    }

                    //建立新表
                    SQL = VPOSInitialTableSyntax(StrTable);//取出 存在程式碼最新的SQLit最新最正確的建表SQL
                    if (Database.Length == 0)
                    {
                        SQLiteInsertUpdateDelete(SQL);
                    }
                    else
                    {
                        SQLiteInsertUpdateDelete(Database, SQL);
                    }

                    //複製資料
                    SQL = String.Format("INSERT INTO {0} SELECT * FROM temp_{0}",StrTable, StrColumn, StrType);
                    if (Database.Length == 0)
                    {
                        SQLiteInsertUpdateDelete(SQL);
                    }
                    else
                    {
                        SQLiteInsertUpdateDelete(Database, SQL);
                    }

                    //刪除舊表
                    SQL = String.Format("DROP TABLE temp_{0}", StrTable, StrColumn, StrType);
                    if (Database.Length == 0)
                    {
                        SQLiteInsertUpdateDelete(SQL);
                    }
                    else
                    {
                        SQLiteInsertUpdateDelete(Database, SQL);
                    }

                    //補上原本對應 SQL INDEX
                    SQL = VPOSInitialIndexSyntax(StrTable);
                    if(SQL.Length > 0) 
                    {
                        if (Database.Length == 0)
                        {
                            SQLiteInsertUpdateDelete(SQL);
                        }
                        else
                        {
                            SQLiteInsertUpdateDelete(Database, SQL);
                        }
                    }
                }
            }
        }

        //---C# SQLite通用操作語法 ~ for System.Data.SQLite.Core & Finisar.SQLite 元件 

        ///應用
        ///產生SQLite的資料庫文件，副檔名為.db
        ///CreateSQLiteDatabase("data.db");
        ///建立資料表test
        ///string createtablestring = "create table test (speed double, dist double);";
        ///CreateSQLiteTable("data.db", createtablestring);
        ///插入資料到test表中
        ///string insertstring = "insert into test (speed,dist) values ('10','100');insert into test (speed,dist) values ('20','200');";
        ///SQLiteInsertUpdateDelete("data.db", insertstring);
        ///讀取資料
        ///DataTable dt = GetDataTable("data.db", "select * from test");
        ///dataGridView1.DataSource = dt;  

        //---
        //DB2DataTable Function
        public static bool user_dataLoad()
        {
            bool blnResult=false;
            m_intLoginSID = -1;
            m_user_dataDataTable = GetDataTable("SELECT * FROM user_data");
            if( ((m_user_dataDataTable != null) && (m_user_dataDataTable.Rows.Count > 0)) )
            {
                blnResult = true;
            }
            return blnResult;
        }

        public static void productLoad()
        {
            //m_productDataTable = GetDataTable("select a.sort as product_sort,a.SID as product_sid,a.product_code,a.barcode,a.product_type,a.product_name,a.product_shortname,a.price_mode,a.product_price,a.unit_sid,a.tax_sid,b.category_sid,c.tax_rate,c.tax_type,IFNULL(e.spec_sid,0) as spec_sid,d.spec_name,d.init_product_sid from product_data a JOIN product_category_relation b ON b.product_sid=a.SID LEFT JOIN tax_data c ON c.SID=a.tax_sid LEFT JOIN product_spec_data d ON d.init_product_sid=a.SID AND d.del_flag='N' LEFT JOIN product_spec_relation e ON e.product_sid=a.SID where a.del_flag='N' and a.stop_flag='N' order by a.sort,a.SID ;");
            m_productDataTable = GetDataTable("select a.display_flag as product_display,a.sort as product_sort,a.SID as product_sid,a.product_code,a.barcode,a.product_type,a.product_name,a.product_shortname,a.price_mode,a.product_price,a.unit_sid,a.tax_sid,b.category_sid,c.tax_rate,c.tax_type,IFNULL(e.spec_sid,0) as spec_sid,d.spec_name,d.init_product_sid,e.alias_name from product_data a JOIN product_category_relation b ON b.product_sid=a.SID LEFT JOIN tax_data c ON c.SID=a.tax_sid LEFT JOIN product_spec_data d ON d.init_product_sid=a.SID AND d.del_flag='N' LEFT JOIN product_spec_relation e ON e.product_sid=a.SID where a.del_flag='N' and a.stop_flag='N' order by a.sort,a.SID ;");
        }

        public static void condimentLoad()
        {
            m_condimentDataTable = GetDataTable("Select a.*,b.group_name,b.required_flag,b.single_flag,b.newline_flag,b.count_flag,b.min_count,b.max_count,c.product_sid,a.SID as condiment_sid From condiment_data a,condiment_group b,product_condiment_relation c where b.SID=a.group_sid AND a.del_flag='N' AND a.stop_flag='N' AND a.SID=c.condiment_sid Order by b.sort,a.group_sid,a.sort ;");
        }

        public static void discount_product_dataLoad()
        {
            m_discount_product_dataDataTable = GetDataTable("SELECT a.SID AS discount_sid,a.discount_code AS discount_code,a.filter_type AS filter_type,a.round_calc_type AS round_calc_type,b.product_sid FROM discount_param AS a JOIN discount_product_relation AS b ON (b.discount_param_sid=a.SID) ORDER BY a.SID");
        }

        public static void promotion_dataLoad()
        {
            DataTable promotion_DataTable = new DataTable();
            DataTable promotion_productDataTable = new DataTable();
            DataTable promotion_order_typeDataTable = new DataTable();
            String SQL = "";

            //---
            //預防觸發 DataTable的 String '' was not recognized as a valid DateTime 錯誤 
            SQL = $"UPDATE promotion_data SET promotion_start_time = '1970-01-01 00:00:00.000' WHERE promotion_start_time=''";
            SQLiteInsertUpdateDelete(SQL);
            SQL = $"UPDATE promotion_data SET promotion_end_time = '1970-01-01 00:00:00.000' WHERE promotion_end_time=''";
            SQLiteInsertUpdateDelete(SQL);
            //---預防觸發 DataTable的 String '' was not recognized as a valid DateTime 錯誤 

            SQL = @"SELECT a.SID,a.promotion_name,a.coexist_flag,a.promotion_type,a.promotion_data,a.promotion_start_time,a.promotion_end_time FROM promotion_data AS a WHERE a.stop_flag='N' AND a.del_flag='N' ORDER BY a.promotion_sort";
            promotion_DataTable = GetDataTable(SQL);
            for(int i = 0; i < promotion_DataTable.Rows.Count; i++)
            {
                promotion_join_data promotion_join_dataBuf = new promotion_join_data();
                promotion_join_dataBuf.m_intSID = Convert.ToInt32(promotion_DataTable.Rows[i][0].ToString());
                promotion_join_dataBuf.m_StrName = promotion_DataTable.Rows[i][1].ToString();
                promotion_join_dataBuf.m_Strcoexist_flag = promotion_DataTable.Rows[i][2].ToString();
                promotion_join_dataBuf.m_Strtype = promotion_DataTable.Rows[i][3].ToString();
                promotion_join_dataBuf.m_promotion_data_rule = JsonClassConvert.promotion_data_rule2Class(promotion_DataTable.Rows[i][4].ToString());
                promotion_join_dataBuf.m_DTstart_time = Convert.ToDateTime(promotion_DataTable.Rows[i][5].ToString());
                promotion_join_dataBuf.m_DTend_time = Convert.ToDateTime(promotion_DataTable.Rows[i][6].ToString());
                m_promotion_join_dataList.Add(promotion_join_dataBuf);
            }

            SQL = @"SELECT a.SID,b.product_sid FROM promotion_data AS a JOIN product_promotion_relation AS b ON (a.SID=b.promotion_sid) WHERE a.stop_flag='N' AND a.del_flag='N' ORDER BY a.promotion_sort,b.product_sid";
            promotion_productDataTable = GetDataTable(SQL);
            for(int i = 0; i < m_promotion_join_dataList.Count; i++)
            {
                for (int j = 0; j < promotion_productDataTable.Rows.Count;j++)
                {
                    if (m_promotion_join_dataList[i].m_intSID == Convert.ToInt32(promotion_productDataTable.Rows[j][0].ToString()))
                    {
                        m_promotion_join_dataList[i].m_ListProductID.Add(Convert.ToInt32(promotion_productDataTable.Rows[j][1].ToString()));
                    }
                }
            }

            SQL = @"SELECT a.SID,b.order_type_sid FROM promotion_data AS a JOIN promotion_order_type_relation AS b ON (a.SID=b.promotion_sid) WHERE a.stop_flag='N' AND a.del_flag='N' ORDER BY a.promotion_sort";           
            promotion_order_typeDataTable = GetDataTable(SQL);
            for (int i = 0; i < m_promotion_join_dataList.Count; i++)
            {
                for (int j = 0; j < promotion_order_typeDataTable.Rows.Count; j++)
                {
                    if (m_promotion_join_dataList[i].m_intSID == Convert.ToInt32(promotion_order_typeDataTable.Rows[j][0].ToString()))
                    {
                        m_promotion_join_dataList[i].m_ListOrderTypeID.Add(Convert.ToInt32(promotion_order_typeDataTable.Rows[j][1].ToString()));
                    }
                }
            }

        }

        public static void printer_valueLoad()//m_printer_valueList 清空+從DB載入最新資料 (變數資料初始化)
        {
            m_printer_valueList.Clear();

            DataTable printer_valueTable = new DataTable();
            //20230215舊版 String SQL = @"SELECT a.SID,a.printer_code,a.printer_name,a.output_type,IFNULL(b.param_value,"""") FROM printer_data AS a LEFT JOIN printer_config AS b ON a.SID=b.printer_sid ORDER BY a.SID";
            String SQL = @"SELECT a.SID,a.printer_code,a.printer_name,a.output_type,IFNULL(a.template_type,""""),IFNULL(a.template_sid,""""),IFNULL(b.param_value,"""") FROM printer_data AS a LEFT JOIN printer_config AS b ON a.SID=b.printer_sid ORDER BY a.SID";
            printer_valueTable = GetDataTable(SQL);

            if((printer_valueTable != null) && (printer_valueTable.Rows.Count > 0))
            {
                for(int i = 0; i < printer_valueTable.Rows.Count; i++)
                {
                    printer_value printer_valuebuf = new printer_value();

                    printer_valuebuf.SID = Convert.ToInt32(printer_valueTable.Rows[i][0].ToString());
                    printer_valuebuf.printer_code = printer_valueTable.Rows[i][1].ToString();
                    printer_valuebuf.printer_name = printer_valueTable.Rows[i][2].ToString();
                    printer_valuebuf.output_type = printer_valueTable.Rows[i][3].ToString();
                    printer_valuebuf.template_type = printer_valueTable.Rows[i][4].ToString();//20230215 新增的欄位
                    printer_valuebuf.template_sid = printer_valueTable.Rows[i][5].ToString();//20230215 新增的欄位
                    printer_valuebuf.printer_config_value = (printer_valueTable.Rows[i][6].ToString().Length > 0) ? (JsonClassConvert.printer_config2Class(printer_valueTable.Rows[i][6].ToString())) : (new printer_config());
                    if((printer_valuebuf.template_sid!=null) && (printer_valuebuf.template_sid.Length>0) && (printer_valuebuf.printer_config_value.print_template.Length == 0))
                    {
                        SQL = $"SELECT SID,template_name FROM printer_template WHERE SID='{printer_valuebuf.template_sid}' LIMIT 0,1";
                        DataTable printer_template = new DataTable();
                        printer_template = GetDataTable(SQL);
                        printer_valuebuf.printer_config_value.print_template = $"{printer_template.Rows[0][1].ToString()};{printer_template.Rows[0][0].ToString()}";
                    }
                    m_printer_valueList.Add(printer_valuebuf);
                }
            }
        }

        public static bool printer_templateLoad()
        {
            bool blnResult = false;
            m_printer_templateDataTable = GetDataTable("SELECT * FROM printer_template ORDER BY SID");
            if (((m_printer_templateDataTable != null) && (m_printer_templateDataTable.Rows.Count > 0)))
            {
                blnResult = true;
            }
            return blnResult;
        }

        public static void printer_groupLoad()//印表群組資料 ~ 用來過濾 產品是否列印
        {
            /*
            SELECT a.SID AS SID,a.printer_group_name AS printer_group_name,a.printer_sid AS printer_sid,b.order_type_sid AS order_type_sid,c.product_sid AS product_sid,d.product_code AS product_code
            FROM printer_group_data AS a 
            JOIN printer_group_order_type_relation AS b ON a.SID=b.printer_group_sid 
            JOIN printer_group_relation AS c ON a.SID=c.printer_group_sid 
            JOIN product_data AS d ON d.SID=c.product_sid
            WHERE a.stop_flag='N' AND a.del_flag='N'       
            */
            m_printer_groupDataTable3 = GetDataTable("SELECT a.SID AS SID,a.printer_group_name AS printer_group_name,a.printer_sid AS printer_sid,b.order_type_sid AS order_type_sid,c.product_sid AS product_sid,d.product_code AS product_code FROM printer_group_data AS a JOIN printer_group_order_type_relation AS b ON a.SID=b.printer_group_sid JOIN printer_group_relation AS c ON a.SID=c.printer_group_sid JOIN product_data AS d ON d.SID=c.product_sid WHERE a.stop_flag='N' AND a.del_flag='N'");

            /*
            SELECT a.SID AS SID,a.printer_group_name AS printer_group_name,a.printer_sid AS printer_sid,b.order_type_sid AS order_type_sid
            FROM printer_group_data AS a 
            JOIN printer_group_order_type_relation AS b ON a.SID=b.printer_group_sid 
            WHERE a.stop_flag='N' AND a.del_flag='N'
             */
            m_printer_groupDataTable2 = GetDataTable("SELECT a.SID AS SID,a.printer_group_name AS printer_group_name,a.printer_sid AS printer_sid,b.order_type_sid AS order_type_sid FROM printer_group_data AS a JOIN printer_group_order_type_relation AS b ON a.SID=b.printer_group_sid WHERE a.stop_flag='N' AND a.del_flag='N'");

        }

        public static bool formula_dataLoad()
        {
            bool blnResult = false;

            DataTable basic_paramsDataTable = new DataTable();
            String SQL = "SELECT param_value FROM basic_params WHERE param_key='FORMULA_PARAM' LIMIT 0,1";
            basic_paramsDataTable = SQLDataTableModel.GetDataTable(SQL);
            if ((basic_paramsDataTable != null) && (basic_paramsDataTable.Rows.Count > 0))
            {
                String StrJSON = basic_paramsDataTable.Rows[0][0].ToString();
                
                //---
                //JSON正規化
                if(StrJSON.Length>2)
                {
                    //找出所有product_code的值 並把對應json元件名稱換成空字串 方便後續轉成陣列元素
                    MatchCollection matches01 = Regex.Matches(StrJSON, @"""product_code""?:?""(([\s\S])*?)"",");
                    foreach (Match match in matches01)
                    {
                        Console.WriteLine(match.Groups[1].Value);
                        //strinput = Regex.Replace(strinput, $"\"{ match.Groups[1].Value}\"?:", "\"formula_obj\":");//並把 對應json元件名稱換成固定的formula_obj名詞
                        StrJSON = Regex.Replace(StrJSON, $"\"{match.Groups[1].Value}\"?:", "");
                    }

                    //找出所有condiment_code的值 並把對應json元件名稱換成空字串 方便後續轉成陣列元素
                    MatchCollection matches02 = Regex.Matches(StrJSON, @"""condiment_code""?:?""(([\s\S])*?)"",");
                    foreach (Match match in matches02)
                    {
                        Console.WriteLine(match.Groups[1].Value);
                        //strinput = Regex.Replace(strinput, $"\"{match.Groups[1].Value}\"?:", "\"condiment_obj\":");//並把 對應json元件名稱換成固定的condiment_obj名詞
                        StrJSON = Regex.Replace(StrJSON, $"\"{match.Groups[1].Value}\"?:", "");
                    }

                    //將兩段JSON元件轉陣列元件
                    StrJSON = StrJSON.Replace(@"""formula_data"":{", @"""formula_data"":[");
                    StrJSON = StrJSON.Replace(@"},""condiment_bom"":{", @"],""condiment_bom"":[");
                    StrJSON = StrJSON.Replace(@"},""material_change_data""", @"],""material_change_data""");
                    StrJSON = StrJSON.Substring(0, StrJSON.Length - 2) + "]}";
                }
                //---JSON正規化

                m_formula_data = JsonClassConvert.Formula_Data2Class(StrJSON);
                if((m_formula_data!=null) && (m_formula_data.formula_data!=null) && (m_formula_data.condiment_bom!=null))
                {
                    blnResult = true;
                }            
            }
            return blnResult;
        }

        public static void payment_module_paramsLoad()
        {
            String SQL = "";
            SQL = "SELECT * FROM payment_module";
            m_payment_module = GetDataTable(SQL);
            SQL = "SELECT * FROM payment_module_params WHERE del_flag='N' AND stop_flag='N'";
            m_payment_module_params = GetDataTable(SQL);
            
        }

        public static void takeaways_paramsLoad()//外送平台參數串接資料
        {
            String SQL = "SELECT TP1.SID,TP1.platform_name,TP2.active_state,TP2.params FROM takeaways_platform AS TP1 INNER JOIN takeaways_params AS TP2 ON TP2.active_state='Y'WHERE TP1.SID=TP2.platform_sid";
            m_takeaways_params = GetDataTable(SQL);
            m_blnFOODPANDAOpen = false;
            m_blnUBER_EATSOpen = false;
            m_blnVTSTOREOpen = false;
            if( (m_takeaways_params!=null) && (m_takeaways_params.Rows.Count>0))
            {
                for(int i=0;i< m_takeaways_params.Rows.Count;i++)
                {
                    if ((m_takeaways_params.Rows[i]["SID"].ToString()== "FOODPANDA") && (m_takeaways_params.Rows[i]["active_state"].ToString()=="Y"))
                    {
                        m_blnFOODPANDAOpen = true;
                        continue;
                    }

                    if ((m_takeaways_params.Rows[i]["SID"].ToString() == "UBER_EATS") && (m_takeaways_params.Rows[i]["active_state"].ToString() == "Y"))
                    {
                        m_blnUBER_EATSOpen = true;
                        continue;
                    }

                    if ((m_takeaways_params.Rows[i]["SID"].ToString() == "VTSTORE") && (m_takeaways_params.Rows[i]["active_state"].ToString() == "Y"))
                    {
                        m_blnVTSTOREOpen = true;
                        continue;
                    }
                }
            }

            //---
            //初始化外賣接單資料庫
            if (m_blnFOODPANDAOpen || m_blnUBER_EATSOpen || m_blnVTSTOREOpen)
            {
                String StrFileName = LogFile.m_StrSysPath + "takeaways.db";
                if (!File.Exists(StrFileName))
                {
                    SQLDataTableModel.CreateSQLiteDatabase(StrFileName);
                    string CreateTableString = SQLDataTableModel.VPOSInitialTableSyntax("takeaways_order_data");
                    SQLDataTableModel.CreateSQLiteTable(StrFileName, CreateTableString);//建立資料表程式
                    LogFile.Write("SystemNormal ; takeaways.db Init");

                    SQLDataTableModel.DBColumnsPadding("takeaways_order_data", "pre_order", "CHAR(1)", "N", false, "Takeaways");//20230321
                }
            }
            //---初始化外賣接單資料庫
        }

        public static String NormalizeProductName(String StrItemCode, String StrItemName,bool blncheck)//外部接單產品名稱正規化 (外送接單，依據商品及配料是否有設定轉換成POS名稱，決定是否進行轉換)
        {
            String StrResult = StrItemName;

            if(!blncheck) 
            {
                return StrResult;
            }

            try
            {
                DataRow[] foundRows = SQLDataTableModel.m_productDataTable.Select($"product_code='{StrItemCode}'");
                if ((foundRows != null) && (foundRows.Length > 0))
                {
                    StrResult = foundRows[0]["product_name"].ToString();
                }
            }
            catch (Exception ex)
            {
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "NormalizeProductName()", StrItemCode, ex.ToString());
                    LogFile.Write("SQLError ; " + StrLog);
                }
            }
            return StrResult;
        }

        public static String NormalizeCondimentName(String StrItemCode, String StrItemName, bool blncheck)//外部接單配料名稱正規化 (外送接單，依據商品及配料是否有設定轉換成POS名稱，決定是否進行轉換)
        {
            String StrResult = StrItemName;

            if (!blncheck)
            {
                return StrResult;
            }
            
            try
            {
                DataRow[] foundRows = SQLDataTableModel.m_condimentDataTable.Select($"condiment_code='{StrItemCode}'");
                if ((foundRows != null) && (foundRows.Length > 0))
                {
                    StrResult = foundRows[0]["condiment_name"].ToString();
                }
            }
            catch (Exception ex)
            {
                if (true)
                {
                    String StrLog = String.Format("{0}: {1};{2}", "NormalizeCondimentName()", StrItemCode, ex.ToString());
                    LogFile.Write("SQLError ; " + StrLog);
                }
            }
            return StrResult;
        }

        public static void cust_display_paramLoad()
        {
            m_cust_display_param = GetDataTable("SELECT * FROM cust_display_param LIMIT 0,1");
        }
        //---DB2DataTable Function

        //---
        //DB2DataTable Var
        public static int m_intLoginSID = -1;
        public static DataTable m_productDataTable = new DataTable();

        public static DataTable m_user_dataDataTable = new DataTable();
        public static List<product_spec_Var> m_product_spec_Var=new List<product_spec_Var>();

        public static DataTable m_condimentDataTable = new DataTable();
        public static List<condiment_Var> m_condiment_Var = new List<condiment_Var>();

        public static DataTable m_product_price_type_relationDataTable = new DataTable();

        public static DataTable m_discount_product_dataDataTable = new DataTable();

        public static List<promotion_join_data> m_promotion_join_dataList = new List<promotion_join_data>();

        public static List<printer_value> m_printer_valueList=new List<printer_value>();

        public static DataTable m_printer_templateDataTable = new DataTable();

        public static DataTable m_printer_groupDataTable3 = new DataTable();//貼紙用
        public static DataTable m_printer_groupDataTable2 = new DataTable();//號碼單用

        public static Formula_Data m_formula_data = new Formula_Data();

        public static DataTable m_payment_module = new DataTable();
        public static DataTable m_payment_module_params = new DataTable();

        public static DataTable m_takeaways_params = new DataTable();//外送平台串接參數資料
        public static bool m_blnFOODPANDAOpen = false;//判斷對應外送平台是否有啟用變數
        public static bool m_blnUBER_EATSOpen = false;
        public static bool m_blnVTSTOREOpen = false;
        public static bool m_blnVTEAMQrorderOpen = false;

        public static DataTable m_cust_display_param = new DataTable();//客顯參數
        //---DB2DataTable Var

        //---
        public static String VPOSInitialIndexSyntax(String StrTableName)//回饋對應資料表的預設語法
        {
            String StrResult = "";
            switch (StrTableName)
            {
                case "condiment_data":
                    StrResult = @"CREATE INDEX ""idx_condiment_data_01"" ON ""condiment_data"" (
                    ""company_sid""	ASC
                    );";
                    break;
                case "func_main":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_func_main_01"" ON ""func_main"" (
                    ""func_type""	ASC
                    );";
                    break;
                case "order_invoice_data":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_order_inv_data_01"" ON ""order_invoice_data"" (
                    ""order_no""	ASC
                    );
                    CREATE INDEX IF NOT EXISTS ""idx_order_inv_data_02"" ON ""order_invoice_data"" (
                    ""inv_period""	ASC,
                    ""inv_no""	ASC
                    );";
                    break;
                case "serial_code_data":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_serial_code_data_01"" ON ""serial_code_data"" (
	                    ""serial_type""	ASC,
	                    ""serial_owner""	ASC
                    );";
                    break;
                case "product_spec_relation":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_product_spec_relation_01"" ON ""product_spec_relation"" (
                    ""product_sid""
                    );";
                    break;
                case "store_table_data":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_store_table_data_01"" ON ""store_table_data"" (
                    ""table_code""
                    );";
                    break;
                case "product_category":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_product_category_01"" ON ""product_category"" (
                    ""company_sid""	ASC
                    );";
                    break;
                case "product_set_relation":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_product_set_relation_01"" ON ""product_set_relation"" (
                    ""set_sid""
                    );";
                    break;
                case "set_attribute_data":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_set_attribute_data_01"" ON ""set_attribute_data"" (
	                ""set_sid""
                    );";
                    break;
                case "user_data":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_terminal_user_data_01"" ON ""user_data"" (
                    ""user_account""	ASC
                    );
                    CREATE INDEX IF NOT EXISTS ""idx_terminal_user_data_02"" ON ""user_data"" (
                    ""company_sid""	ASC
                    );";
                    break;
                case "payment_module_params":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_payment_module_params_01"" ON ""payment_module_params"" (
                    ""payment_module_code""
                    );";
                    break;
                case "expense_data":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_expense_data_01"" ON ""expense_data"" (
	                ""action_time""
                    );";
                    break;
                case "order_payment_data":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_order_payment_data_01"" ON ""order_payment_data"" (
                    ""order_no"",
                    ""payment_code""
                    );";
                    break;
                case "order_coupon_data":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_order_coupon_data_01"" ON ""order_coupon_data"" (
                    ""order_no""	ASC,
                    ""coupon_code""	ASC
                    );
                    CREATE INDEX IF NOT EXISTS ""idx_order_coupon_data_02"" ON ""order_coupon_data"" (
                    ""order_no""	ASC,
                    ""external_id""	ASC
                    );";
                    break;
                case "product_data":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_product_data_01"" ON ""product_data"" (
                    ""company_sid""	ASC
                    );
                    CREATE INDEX IF NOT EXISTS ""idx_product_data_02"" ON ""product_data"" (
                    ""product_code""	ASC
                    );";
                    break;
                case "order_calc_formula":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""order_calc_formula_idx_01"" ON ""order_calc_formula"" (
                    ""order_no""
                    );";
                    break;
                case "order_content_data":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_order_content_01"" ON ""order_content_data"" (
                    ""order_no""	ASC
                    );";
                    break;
                case "daily_report":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_daily_report_01"" ON ""daily_report"" (
                    ""report_no""	ASC
                    );
                    CREATE INDEX IF NOT EXISTS ""idx_daily_report_02"" ON ""daily_report"" (
                    ""order_start_time""	ASC,
                    ""order_end_time""	ASC
                    );";
                    break;
                case "order_data":
                    StrResult = @"CREATE INDEX IF NOT EXISTS ""idx_order_data_01"" ON ""order_data"" (
                    ""order_time""	ASC
                    );
                    CREATE INDEX IF NOT EXISTS ""idx_order_data_02"" ON ""order_data"" (
                    ""terminal_sid""	ASC
                    );
                    CREATE INDEX IF NOT EXISTS ""idx_order_data_05"" ON ""order_data"" (
                    ""takeaways_order_sid""	ASC
                    );";
                    break;
                case "":
                    StrResult = @"";
                    break;
            }
            return StrResult;
        }
        public static String VPOSInitialTableSyntax(String StrTableName)//回饋對應資料表的預設語法
        {
            String StrResult = "";
            switch(StrTableName)
            {
                case "takeaways_order_data":
                    StrResult = @"CREATE TABLE ""takeaways_order_data"" (
	                    ""SID""	char(64) NOT NULL,
	                    ""platform_sid""	varchar(30) NOT NULL,
	                    ""external_order_id""	varchar(100) NOT NULL,
	                    ""order_no""	varchar(100) NOT NULL,
	                    ""order_time""	int NOT NULL,
	                    ""order_type""	varchar(30) NOT NULL,
	                    ""order_state""	varchar(30) NOT NULL,
	                    ""call_num""	varchar(30),
	                    ""item_count""	int DEFAULT 0,
	                    ""subtotal""	decimal(20, 5) DEFAULT 0,
	                    ""promotion_fee""	decimal(20, 5) DEFAULT 0,
	                    ""discount_fee""	decimal(20, 5) DEFAULT 0,
	                    ""delivery_fee""	decimal(20, 5) DEFAULT 0,
	                    ""service_fee""	decimal(20, 5) DEFAULT 0,
	                    ""amount""	decimal(20, 5) DEFAULT 0,
	                    ""payment_type""	varchar(30),
	                    ""cust_name""	varchar(50),
	                    ""cust_phone""	varchar(50),
	                    ""cust_tax_number""	varchar(10),
	                    ""cust_reserve_time""	int DEFAULT 0,
	                    ""delivery_city_name""	varchar(50),
	                    ""delivery_district_name""	varchar(50),
	                    ""delivery_address""	varchar(200),
	                    ""remarks""	text,
	                    ""estimated_read_time""	int DEFAULT 0,
	                    ""original_data""	text NOT NULL,
	                    ""sync_state""	char(1) NOT NULL DEFAULT 'N',
	                    ""sync_msg""	text,
	                    ""pos_order_no""	varchar(30),
	                    ""created_time""	timestamp,
	                    ""updated_time""	timestamp,
	                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "condiment_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""condiment_data"" (
                      ""SID"" int NOT NULL,
                      ""company_sid"" int NOT NULL,
                      ""condiment_code"" varchar(64),
                      ""condiment_name"" varchar(50) NOT NULL,
                      ""condiment_price"" decimal(10,2) NOT NULL,
                      ""unit_sid"" int,
                      ""group_sid"" int NOT NULL,
                      ""sort"" int NOT NULL DEFAULT 0,
                      ""stop_flag"" char(1) NOT NULL DEFAULT 'N',
                      ""stop_time"" timestamp NOT NULL DEFAULT '0000-00-00',
                      ""del_flag"" char(1) NOT NULL DEFAULT 'N',
                      ""del_time"" timestamp NOT NULL DEFAULT '0000-00-00',
                      ""created_time"" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                      ""updated_time"" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                      PRIMARY KEY (""SID"", ""company_sid"")
                    )";
                    break;
                case "printer_template":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""printer_template"" (
	                ""SID""	varchar(64) NOT NULL,
	                ""template_name""	varchar(50) NOT NULL,
	                ""template_value""	text,
	                ""template_type""	varchar(20) NOT NULL,
	                ""include_command""	char(1) DEFAULT 'N',
	                ""created_time""	timestamp,
	                ""updated_time""	timestamp,
	                PRIMARY KEY(""SID"")
                    )";
                    break;
                case "upload_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""upload_data"" (
	                ""SID""	char(12) NOT NULL,
	                ""upload_type""	varchar(50) NOT NULL,
	                ""data_no""	varchar(50) NOT NULL,
	                ""upload_state""	char(1) NOT NULL DEFAULT 'N',
	                ""upload_time""	datetime DEFAULT NULL,
	                ""upload_msg""	text,
	                ""try_count""	int DEFAULT 0,
	                ""created_time""	timestamp,
	                ""updated_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
	                PRIMARY KEY(""SID"")
                    )";
                    break;
                case "city_code_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""city_code_data"" (
	                ""code""	varchar(20) NOT NULL,
	                ""code_type""	char(1) NOT NULL,
	                ""parent_code""	varchar(20),
	                ""show_name""	varchar(30) NOT NULL,
	                ""sort""	int NOT NULL DEFAULT 0,
	                ""stop_flag""	char(1) NOT NULL DEFAULT 'N',
	                ""created_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
	                ""updated_time""	timestamp,
	                PRIMARY KEY(""code"")
                    )";
                    break;
                case "class_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""class_data"" (
	                ""SID""	int NOT NULL,
	                ""class_name""	varchar(10) NOT NULL,
	                ""time_start""	char(5),
	                ""time_end""	char(5),
	                ""sort""	int(2) NOT NULL DEFAULT 0,
	                ""del_flag""	char(1) NOT NULL DEFAULT 'N',
	                ""del_time""	timestamp NOT NULL DEFAULT '0000-00-00',
	                ""created_time""	timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	                ""updated_time""	timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	                PRIMARY KEY(""SID"")
                    )";
                    break;
                case "company_payment_params":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""company_payment_params"" (
                    ""payment_sid""	int NOT NULL,
                    ""active_state""	char(1) NOT NULL,
                    ""env_type""	char(1) NOT NULL,
                    ""client_id""	varchar(100),
                    ""client_secret""	varchar(100),
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""payment_sid"")
                    )";
                    break;
                case "condiment_group":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""condiment_group"" (
                    ""SID""	int NOT NULL,
                    ""company_sid""	int NOT NULL,
                    ""group_name""	varchar(20) NOT NULL,
                    ""required_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""single_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""newline_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""count_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""min_count""	int DEFAULT 0,
                    ""max_count""	int DEFAULT 0,
                    ""sort""	int(3) NOT NULL DEFAULT 0,
                    ""del_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""del_time""	datetime DEFAULT NULL,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""SID"",""company_sid"")
                    )";
                    break;
                case "cust_display_content":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""cust_display_content"" (
                    ""display_data_sid""	int NOT NULL,
                    ""item_no""	int NOT NULL,
                    ""content""	text,
                    PRIMARY KEY(""display_data_sid"",""item_no"")
                    )";
                    break;
                case "cust_display_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""cust_display_data"" (
                    ""SID""	int NOT NULL,
                    ""data_name""	varchar(50),
                    ""data_kind""	char(2),
                    ""source_type""	char(1),
                    ""stretch_size""	char(1) DEFAULT 'N',
                    ""play_type""	char(1),
                    ""play_speed_sec""	int,
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "cust_display_param":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""cust_display_param"" (
                    ""display_show""	char(1) DEFAULT 'N',
                    ""layout_id""	int DEFAULT 0,
                    ""display_size_str""	varchar(15),
                    ""form_width""	int DEFAULT 0,
                    ""form_height""	int DEFAULT 0,
                    ""cf_banner_height""	int,
                    ""cf_show_data_sid""	int,
                    ""mp_play_data_sid""	int,
                    ""ol_label_data_sid""	int,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp
                    )";
                    break;
                case "cust_screen_param":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""cust_screen_param"" (
                    ""terminal_sid""	varcahr(30) NOT NULL,
                    ""layout_id""	int DEFAULT 0,
                    ""show_flag""	char(1) DEFAULT 'N',
                    ""screen_width""	int DEFAULT 0,
                    ""screen_height""	int DEFAULT 0,
                    ""created_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    ""updated_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    PRIMARY KEY(""terminal_sid"")
                    )";
                    break;
                case "func_main":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""func_main"" (
                    ""SID""	varchar(50) NOT NULL,
                    ""func_type""	char(1) NOT NULL,
                    ""parent_func_sid""	varchar(50),
                    ""func_name""	varchar(20) NOT NULL,
                    ""content""	varchar(500),
                    ""sort""	smallint DEFAULT 0,
                    ""stop_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""stop_time""	datetime DEFAULT NULL,
                    ""del_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""del_time""	datetime DEFAULT NULL,
                    ""created_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    ""updated_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "invoice_platform":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""invoice_platform"" (
	                ""SID""	int NOT NULL,
	                ""platform_name""	varchar(50),
	                ""inv_url_1""	varchar(1024),
	                ""inv_url_2""	varchar(1024),
	                ""inv_test_url_1""	varchar(1024),
	                ""inv_test_url_2""	varchar(1024),
	                ""created_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
	                ""updated_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
	                PRIMARY KEY(""SID"")
                    )";
                    break;
                case "order_invoice_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""order_invoice_data"" (
                    ""order_no""	varchar(20) NOT NULL,
                    ""inv_period""	char(6) NOT NULL,
                    ""inv_no""	char(10) NOT NULL,
                    ""cust_ein""	varchar(8),
                    ""donate_flag""	char(1) DEFAULT 'N',
                    ""donate_code""	varchar(20),
                    ""carrier_type""	varchar(20),
                    ""carrier_code_1""	varchar(64),
                    ""carrier_code_2""	varchar(64),
                    ""batch_num""	int NOT NULL,
                    ""random_code""	char(4) NOT NULL,
                    ""qrcode_aes_key""	varchar(32),
                    PRIMARY KEY(""order_no"",""inv_period"",""inv_no"")
                    )";
                    break;
                case "param_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""param_data"" (
	                ""terminal_sid""	varcahr(30) NOT NULL,
	                ""terminal_server_flag""	char(1) DEFAULT 'N',
	                ""terminal_server_port""	int DEFAULT 0,
	                ""order_no_from""	char(1),
	                ""serial_server_name""	varchar(40),
	                ""serial_server_port""	int DEFAULT 0,
	                ""print_server_name""	varchar(40),
	                ""print_server_port""	int DEFAULT 0,
	                ""created_time""	datetime DEFAULT CURRENT_TIMESTAMP,
	                ""updated_time""	datetime DEFAULT CURRENT_TIMESTAMP,
	                PRIMARY KEY(""terminal_sid"")
                    )";
                    break;
                case "price_type":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""price_type"" (
                    ""price_type_sid""	int NOT NULL,
                    ""company_sid""	int NOT NULL,
                    ""price_type_name""	char(10) NOT NULL,
                    ""del_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""del_time""	datetime DEFAULT NULL,
                    ""stop_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""stop_time""	datetime DEFAULT NULL,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""company_sid"",""price_type_sid"")
                    )";
                    break;
                case "product_category_relation":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""product_category_relation"" (
                    ""category_sid""	int NOT NULL,
                    ""product_sid""	int NOT NULL,
                    PRIMARY KEY(""category_sid"",""product_sid"")
                    )";
                    break;
                case "product_condiment_relation":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""product_condiment_relation"" (
                    ""product_sid""	int NOT NULL,
                    ""condiment_sid""	int NOT NULL,
                    PRIMARY KEY(""product_sid"",""condiment_sid"")
                    )";
                    break;
                case "product_price_type_relation":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""product_price_type_relation"" (
                    ""product_sid""	int NOT NULL,
                    ""price_type_sid""	int NOT NULL,
                    ""price""	decimal(10, 2) NOT NULL,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""product_sid"",""price_type_sid"")
                    )";
                    break;
                case "product_promotion_relation":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""product_promotion_relation"" (
                    ""product_sid""	int NOT NULL,
                    ""promotion_sid""	int NOT NULL,
                    PRIMARY KEY(""product_sid"",""promotion_sid"")
                    )";
                    break;
                case "product_unit":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""product_unit"" (
                    ""SID""	int NOT NULL,
                    ""company_sid""	int NOT NULL,
                    ""unit_name""	varchar(10) NOT NULL,
                    ""sort""	int(3) NOT NULL DEFAULT 0,
                    ""del_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""del_time""	timestamp NOT NULL DEFAULT '0000-00-00',
                    ""created_time""	timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    ""updated_time""	timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    PRIMARY KEY(""SID"",""company_sid"")
                    )";
                    break;
                case "promotion_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""promotion_data"" (
                    ""SID""	int NOT NULL,
                    ""company_sid""	int NOT NULL,
                    ""promotion_name""	varchar(30) NOT NULL,
                    ""promotion_start_time""	datetime,
                    ""promotion_end_time""	datetime,
                    ""promotion_sort""	int NOT NULL DEFAULT 0,
                    ""coexist_flag""	char(1) NOT NULL,
                    ""promotion_type""	char(1) NOT NULL,
                    ""promotion_data""	text NOT NULL,
                    ""stop_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""stop_time""	datetime DEFAULT NULL,
                    ""del_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""del_time""	datetime DEFAULT NULL,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""SID"",""company_sid"")
                    )";
                    break;
                case "role_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""role_data"" (
                    ""SID""	int NOT NULL,
                    ""role_name""	varchar(20) NOT NULL,
                    ""del_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""del_time""	datetime DEFAULT NULL,
                    ""created_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    ""updated_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "role_func":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""role_func"" (
                    ""role_sid""	int NOT NULL,
                    ""func_sid""	varchar(50) NOT NULL,
                    PRIMARY KEY(""role_sid"",""func_sid"")
                    )";
                    break;
                case "serial_code_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""serial_code_data"" (
                    ""serial_type""	varchar(30) NOT NULL,
                    ""serial_name""	varchar(30),
                    ""code_first_char""	varchar(20),
                    ""code_split_char""	chae(1),
                    ""code_num_len""	int NOT NULL DEFAULT 5,
                    ""code_str""	varchar(20),
                    ""code_num""	int DEFAULT 0,
                    ""serial_owner""	varchar(32),
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""serial_type"")
                    )";
                    break;
                case "tax_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""tax_data"" (
                    ""SID""	int NOT NULL,
                    ""company_sid""	int NOT NULL,
                    ""tax_name""	varchar(20) NOT NULL,
                    ""tax_rate""	decimal(5, 2),
                    ""tax_type""	char(1),
                    ""del_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""del_time""	timestamp NOT NULL DEFAULT '0000-00-00',
                    ""created_time""	timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    ""updated_time""	timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    PRIMARY KEY(""SID"",""company_sid"")
                    )";
                    break;
                case "device_config":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""device_config"" (
                    ""device_sid""	varchar(10) NOT NULL,
                    ""device_type""	int,
                    ""page_size""	int,
                    ""device_model""	varchar(30),
                    ""conn_io_type""	char(1),
                    ""tcp_ip""	char(15),
                    ""tcp_port""	int,
                    ""port_name""	varchar(10),
                    ""baud_rate""	int,
                    ""flow_control""	int,
                    ""onoff_flag""	char(1) DEFAULT 'N',
                    ""auto_print_flag""	char(1) DEFAULT 'N',
                    ""cash_box_flag""	char(1) DEFAULT N,
                    ""formula_print_flag""	char(1) DEFAULT N,
                    ""print_bill_flag""	char(1) DEFAULT 'N',
                    ""print_logo_flag""	char(1) DEFAULT 'N',
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""device_sid"")
                    )";
                    break;
                case "formula_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""formula_data"" (
	                ""SID""	int NOT NULL,
	                ""formula_params""	ntext,
	                ""created_time""	timestamp,
	                ""updated_time""	timestamp,
	                PRIMARY KEY(""SID"")
                    )";
                    break;
                case "product_spec_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""product_spec_data"" (
	                ""SID""	int NOT NULL,
	                ""spec_name""	varchar(50) NOT NULL,
	                ""init_product_sid""	int,
	                ""del_flag""	char(1) NOT NULL DEFAULT 'N',
	                ""del_time""	datetime,
	                ""created_time""	timestamp NOT NULL,
	                ""updated_time""	timestamp NOT NULL,
	                PRIMARY KEY(""SID"")
                    )";
                    break;
                case "product_spec_relation":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""product_spec_relation"" (
                    ""spec_sid""	int NOT NULL,
                    ""product_sid""	int NOT NULL,
                    ""alias_name""	varchar(50),
                    ""sort""	int,
                    PRIMARY KEY(""spec_sid"",""product_sid"")
                    )";
                    break;
                case "basic_params":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""basic_params"" (
                    ""param_key""	varchar(60) NOT NULL,
                    ""param_value""	text NOT NULL,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    ""source_type""	CHAR(10) DEFAULT null,
                    PRIMARY KEY(""param_key"")
                    )";
                    break;
                case "store_table_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""store_table_data"" (
                    ""SID""	int NOT NULL,
                    ""table_code""	varchar(20),
                    ""table_name""	varchar(30),
                    ""table_capacity""	int(2),
                    ""table_sort""	int,
                    ""stop_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""stop_time""	datetime,
                    ""del_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""del_time""	datetime,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "company_invoice_params":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""company_invoice_params"" (
                    ""company_sid""	int NOT NULL,
                    ""platform_sid""	int NOT NULL,
                    ""env_type""	char(1) NOT NULL,
                    ""active_state""	char(1) NOT NULL DEFAULT 'N',
                    ""branch_no""	varchar(30),
                    ""reg_id""	varchar(64),
                    ""qrcode_aes_key""	varchar(32),
                    ""inv_renew_count""	int DEFAULT 0,
                    ""auth_account""	varchar(50),
                    ""auth_password""	varchar(50),
                    ""created_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    ""updated_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    ""booklet""	int DEFAULT 0,
                    PRIMARY KEY(""company_sid"")
                    )";
                    break;
                case "product_category":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""product_category"" (
	                ""SID""	int NOT NULL,
	                ""company_sid""	int NOT NULL,
	                ""category_code""	varchar(50),
	                ""category_name""	varchar(20) NOT NULL,
	                ""sort""	int(3) NOT NULL DEFAULT 0,
                    ""display_flag""	CHAR(1) DEFAULT N,
	                ""stop_flag""	char(1) DEFAULT 'N',
	                ""stop_time""	datetime DEFAULT NULL,
	                ""del_flag""	char(1) NOT NULL DEFAULT 'N',
	                ""del_time""	timestamp NOT NULL DEFAULT '0000-00-00',
	                ""created_time""	timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	                ""updated_time""	timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	                PRIMARY KEY(""SID"",""company_sid"")
                    )";
                    break;
                case "payment_type":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""payment_type"" (
                    ""SID""	int NOT NULL,
                    ""payment_code""	varchar(20) NOT NULL,
                    ""payment_name""	varchar(30) NOT NULL,
                    ""created_time""	timestamp NOT NULL,
                    ""updated_time""	timestamp NOT NULL,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "printer_group_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""printer_group_data"" (
                    ""SID""	int NOT NULL,
                    ""printer_group_name""	varchar(60) NOT NULL,
                    ""printer_sid""	int NOT NULL,
                    ""order_type_sid""	int DEFAULT 0,
                    ""stop_flag""	char(1) DEFAULT 'N',
                    ""stop_time""	datetime,
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""del_time""	datetime,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "printer_group_relation":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""printer_group_relation"" (
                    ""printer_group_sid""	int NOT NULL,
                    ""product_sid""	int NOT NULL,
                    PRIMARY KEY(""printer_group_sid"",""product_sid"")
                    )";
                    break;
                case "printer_config":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""printer_config"" (
                    ""printer_sid""	int NOT NULL,
                    ""param_value""	text,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""printer_sid"")
                    )";
                    break;
                case "printer_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""printer_data"" (
                    ""SID""	int NOT NULL,
                    ""printer_code""	varchar(50) NOT NULL,
                    ""printer_name""	varchar(50) NOT NULL,
                    ""output_type""	char(1) NOT NULL,
                    ""stop_flag""	char(1) DEFAULT 'N',
                    ""stop_time""	datetime,
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""del_time""	datetime,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    ""template_type""	varchar(20) DEFAULT null,
                    ""template_sid""	varchar(64) DEFAULT null,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "product_set_relation":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""product_set_relation"" (
                    ""set_sid""	int NOT NULL,
                    ""attribute_sid""	int NOT NULL,
                    ""category_sid""	int NOT NULL,
                    ""product_sid""	int NOT NULL,
                    ""main_flag""	char(1) NOT NULL,
                    ""default_flag""	char(1) NOT NULL,
                    PRIMARY KEY(""set_sid"",""product_sid"",""category_sid"",""attribute_sid"")
                    )";
                    break;
                case "set_attribute_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""set_attribute_data"" (
                    ""SID""	int NOT NULL,
                    ""set_sid""	int NOT NULL,
                    ""attribute_name""	varchar(50) NOT NULL,
                    ""main_price_type""	char(1) NOT NULL,
                    ""main_price""	decimal(10, 2) NOT NULL DEFAULT 0,
                    ""main_max_price""	decimal(10, 2) DEFAULT 0,
                    ""sub_price_type""	char(1) NOT NULL,
                    ""sub_price""	decimal(10, 2) NOT NULL DEFAULT 0,
                    ""sub_max_price""	decimal(10, 2) DEFAULT 0,
                    ""required_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""limit_count""	int NOT NULL DEFAULT 0,
                    ""repeat_flag""	char(1) NOT NULL,
                    ""sort""	int NOT NULL DEFAULT 0,
                    ""created_time""	timestamp NOT NULL,
                    ""updated_time""	timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    PRIMARY KEY(""SID"",""set_sid"")
                    )";
                    break;
                case "takeaways_platform":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""takeaways_platform"" (
                    ""SID""	varchar(15) NOT NULL,
                    ""platform_name""	varchar(20),
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "takeaways_params":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""takeaways_params"" (
                    ""platform_sid""	varchar(15) NOT NULL,
                    ""active_state""	char(1) NOT NULL DEFAULT 'N',
                    ""params""	text,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""platform_sid"")
                    )";
                    break;
                case "discount_product_relation":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""discount_product_relation"" (
                    ""discount_param_sid""	int NOT NULL,
                    ""product_sid""	int NOT NULL,
                    PRIMARY KEY(""discount_param_sid"",""product_sid"")
                    )";
                    break;
                case "company":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""company"" (
                    ""SID""	int NOT NULL,
                    ""company_no""	varchar(25),
                    ""authorized_store_no""	varchar(20),
                    ""company_name""	varchar(50) NOT NULL,
                    ""company_shortname""	varchar(20),
                    ""EIN""	varchar(20),
                    ""business_name""	varchar(50),
                    ""company_owner""	varchar(30),
                    ""tel""	varchar(25),
                    ""fax""	varchar(25),
                    ""zip_code""	varchar(10),
                    ""country_code""	varchar(5) NOT NULL,
                    ""province_code""	varchar(10) NOT NULL,
                    ""city_code""	varchar(10) NOT NULL,
                    ""district_code""	varchar(10) NOT NULL,
                    ""address""	varchar(200) NOT NULL,
                    ""def_order_type""	int DEFAULT T,
                    ""def_tax_sid""	int,
                    ""def_unit_sid""	int,
                    ""vtstore_order_url""	varchar(200),
                    ""take_service_flag""	char(1) DEFAULT 'N',
                    ""take_service_type""	char(1),
                    ""take_service_val""	decimal(5, 2),
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""del_time""	datetime DEFAULT NULL,
                    ""created_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    ""updated_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    ""def_params""	TEXT,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "company_customized_params":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""company_customized_params"" (
                    ""customized_code""	varchar(30) NOT NULL,
                    ""customized_name""	varchar(30) NOT NULL,
                    ""active_state""	char(1) DEFAULT 'N',
                    ""params""	text,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""customized_code"")
                    )";
                    break;
                case "user_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""user_data"" (
                    ""SID""	int NOT NULL,
                    ""company_sid""	int NOT NULL,
                    ""role_sid""	int NOT NULL,
                    ""user_account""	varchar(20) NOT NULL,
                    ""user_pwd""	varchar(64) NOT NULL,
                    ""user_name""	varchar(50) NOT NULL,
                    ""employee_no""	varchar(10),
                    ""job_title""	varchar(20),
                    ""tel""	varchar(25),
                    ""cellphone""	varchar(15),
                    ""state_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""state_time""	datetime DEFAULT '0000-00-00',
                    ""del_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""del_time""	datetime DEFAULT '0000-00-00',
                    ""created_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    ""updated_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "payment_module_params":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""payment_module_params"" (
                    ""SID""	int NOT NULL,
                    ""payment_module_code""	varchar(50) NOT NULL,
                    ""params""	text,
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""del_time""	datetime,
                    ""stop_flag""	char(1) DEFAULT 'N',
                    ""stop_time""	datetime,
                    ""sort""	int DEFAULT 0,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "order_payment_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""order_payment_data"" (
                    ""order_no""	varchar(20) NOT NULL,
                    ""item_no""	int NOT NULL DEFAULT 0,
                    ""payment_sid""	int,
                    ""payment_code""	varchar(30) NOT NULL,
                    ""payment_name""	varchar(30),
                    ""payment_module_code""	varchar(30),
                    ""payment_module_params""	text,
                    ""coupon_discount""	decimal(10, 2) DEFAULT 0,
                    ""amount""	decimal(10, 2) NOT NULL,
                    ""received_fee""	decimal(10, 2) DEFAULT 0,
                    ""change_fee""	decimal(10, 2) DEFAULT 0,
                    ""payment_time""	datetime,
                    ""payment_info""	text,
                    ""refund_flag""	char(1) DEFAULT 'N',
                    ""refund_time""	datetime,
                    ""refund_info""	text,
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""del_time""	datetime,
                    ""created_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""order_no"",""item_no"")
                    )";
                    break;
                case "order_type_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""order_type_data"" (
                    ""SID""	int NOT NULL,
                    ""price_type_sid""	int NOT NULL,
                    ""type_name""	varchar(10) NOT NULL,
                    ""order_type_code""	varchar(30),
                    ""payment_def""	int DEFAULT 0,
                    ""def_payment_code""	varchar(20),
                    ""invoice_state""	int NOT NULL DEFAULT 0,
                    ""display_state""	char(1) NOT NULL DEFAULT 'Y',
                    ""sort""	int,
                    ""stop_flag""	char(1) DEFAULT 'N',
                    ""stop_time""	datetime DEFAULT NULL,
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""del_time""	datetime DEFAULT NULL,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    ""params""	TEXT,
                    PRIMARY KEY(""SID"",""price_type_sid"")
                    )";
                    break;
                case "payment_module":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""payment_module"" (
                    ""payment_module_code""	varchar(50) NOT NULL,
                    ""payment_module_name""	varchar(50),
                    ""def_params""	text,
                    ""active_state""	char(1) NOT NULL DEFAULT 'N',
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""payment_module_code"")
                    )";
                    break;
                case "member_platform_params":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""member_platform_params"" (
                    ""SID""	int NOT NULL,
                    ""platform_type""	varchar(30),
                    ""params""	text,
                    ""sort""	int,
                    ""stop_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""stop_time""	datetime,
                    ""del_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""del_time""	datetime,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "account_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""account_data"" (
                    ""SID""	int NOT NULL,
                    ""account_code""	varchar(50) NOT NULL,
                    ""account_name""	varchar(50) NOT NULL,
                    ""type""	char(1) NOT NULL,
                    ""sort""	int,
                    ""stop_flag""	char(1) DEFAULT 'N',
                    ""stop_time""	int,
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""del_time""	int,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "expense_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""expense_data"" (
                    ""expense_no""	varchar(20) NOT NULL,
                    ""action_time""	int,
                    ""action_user""	varchar(30),
                    ""account_code""	varchar(50),
                    ""account_name""	varchar(50),
                    ""account_type""	char(1),
                    ""money""	decimal(20, 5),
                    ""payment_code""	varchar(30),
                    ""payment_name""	varchar(30),
                    ""remark""	text,
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""del_time""	int,
                    ""class_close_flag""	char(1) DEFAULT 'N',
                    ""class_report_no""	varchar(20),
                    ""daily_close_flag""	char(1) DEFAULT 'N',
                    ""daily_report_no""	varchar(20),
                    ""created_time""	timestamp NOT NULL,
                    ""updated_time""	timestamp NOT NULL,
                    PRIMARY KEY(""expense_no"")
                    )";
                    break;
                case "order_coupon_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""order_coupon_data"" (
                    ""order_no""	varchar(20) NOT NULL,
                    ""item_no""	int NOT NULL,
                    ""external_id""	varchar(100),
                    ""coupon_issuer""	varchar(20),
                    ""coupon_mode""	varchar(50),
                    ""coupon_code""	varchar(100),
                    ""coupon_name""	varchar(100),
                    ""coupon_amount""	decimal(10, 2),
                    ""original""	text,
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""del_time""	int,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    ""coupon_quantity""	INT DEFAULT 0,
                    ""coupon_val""	decimal(10, 2) DEFAULT 0,
                    PRIMARY KEY(""order_no"",""item_no"")
                    )";
                    break;
                case "packaging_type":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""packaging_type"" (
	                ""SID""	int(11) NOT NULL,
	                ""name""	varchar(20) NOT NULL,
	                ""sort""	int(3) DEFAULT 0,
	                ""show_flag""	char(1),
	                ""required_flag""	char(1),
	                ""del_flag""	char(1),
	                ""del_time""	datetime DEFAULT NULL,
	                ""created_time""	timestamp,
	                ""updated_time""	timestamp,
	                PRIMARY KEY(""SID"")
                    )";
                    break;
                case "packaging_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""packaging_data"" (
	                ""SID""	int(11) NOT NULL,
	                ""packaging_type_sid""	int(11) NOT NULL,
	                ""code""	varchar(30),
	                ""name""	varchar(30),
	                ""price""	decimal(10, 2) DEFAULT 0,
	                ""memo""	varchar(50),
	                ""sort""	int(11) DEFAULT 0,
	                ""del_flag""	char(1),
	                ""del_time""	datetime DEFAULT NULL,
	                ""created_time""	timestamp,
	                ""updated_time""	timestamp,
	                PRIMARY KEY(""SID"")
                    )";
                    break;
                case "product_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""product_data"" (
	                ""SID""	int NOT NULL,
	                ""company_sid""	int NOT NULL,
	                ""product_code""	varchar(50) NOT NULL,
	                ""barcode""	varchar(100),
	                ""product_name""	varchar(50) NOT NULL,
	                ""product_shortname""	varchar(40),
	                ""product_type""	char(10),
	                ""price_mode""	char(1) DEFAULT 'F',
	                ""product_price""	decimal(10, 2),
	                ""unit_sid""	int NOT NULL,
	                ""tax_sid""	int,
	                ""sort""	int NOT NULL DEFAULT 0,
                    ""display_flag""	CHAR(1) DEFAULT N,
	                ""memo""	text,
	                ""stop_flag""	char(1) NOT NULL DEFAULT 'N',
	                ""stop_time""	datetime NOT NULL,
	                ""del_flag""	char(1) NOT NULL DEFAULT 'N',
	                ""del_time""	datetime NOT NULL,
	                ""price_update_time""	datetime,
	                ""category_update_time""	datetime DEFAULT NULL,
	                ""condiment_update_time""	datetime DEFAULT NULL,
	                ""promotion_update_time""	datetime,
	                ""spec_update_time""	datetime,
	                ""created_time""	timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	                ""updated_time""	timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
	                PRIMARY KEY(""SID"",""company_sid"")
                )";
                    break;
                case "company_payment_type":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""company_payment_type"" (
                    ""SID""	int NOT NULL,
                    ""payment_code""	varchar(20),
                    ""payment_name""	varchar(30),
                    ""payment_module_code""	varchar(50),
                    ""def_paid_flag""	char(1) DEFAULT 'N',
                    ""def_paid_amount""	decimal(10, 2) DEFAULT 0,
                    ""no_change_flag""	char(1) DEFAULT 'N',
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""del_time""	datetime,
                    ""stop_flag""	char(1),
                    ""stop_time""	datetime,
                    ""sort""	int DEFAULT 0,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "order_calc_formula":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""order_calc_formula"" (
                    ""order_no""	varchar(30) NOT NULL,
                    ""item_no""	int NOT NULL,
                    ""operator_text""	text,
                    ""print_count""	int DEFAULT 0,
                    ""print_flag""	char(1) DEFAULT 'N',
                    ""created_time""	timestamp,
                    PRIMARY KEY(""order_no"",""item_no"")
                    )";
                    break;
                case "promotion_order_type_relation":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""promotion_order_type_relation"" (
                    ""promotion_sid""	int NOT NULL,
                    ""order_type_sid""	int NOT NULL,
                    PRIMARY KEY(""promotion_sid"",""order_type_sid"")
                    )";
                    break;
                case "discount_param":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""discount_param"" (
                    ""SID""	int NOT NULL,
                    ""discount_code""	varchar(50) NOT NULL,
                    ""filter_type""	varchar(10),
                    ""round_calc_type""	char(1) DEFAULT 'S',
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""del_time""	datetime,
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "discount_hotkey":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""discount_hotkey"" (
                    ""SID""	int NOT NULL,
                    ""hotkey_name""	varchar(50) NOT NULL,
                    ""hotkey_code""	varchar(50) NOT NULL,
                    ""discount_code""	varchar(50) NOT NULL,
                    ""val_mode""	char(1) DEFAULT 'F',
                    ""val""	float,
                    ""round_calc_type""	char(1) DEFAULT 'S',
                    ""stop_flag""	char(1) DEFAULT 'N',
                    ""stop_time""	datetime,
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""del_time""	datetime,
                    ""sort""	char(10),
                    ""created_time""	timestamp,
                    ""updated_time""	timestamp,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "terminal_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""terminal_data"" (
                    ""SID""	varchar(20) NOT NULL,
                    ""company_sid""	int NOT NULL,
                    ""terminal_name""	varchar(20) NOT NULL,
                    ""pos_no""	varchar(20),
                    ""pid""	varchar(20),
                    ""rid""	varchar(50),
                    ""app_version""	varchar(20),
                    ""reg_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""reg_submit_time""	datetime DEFAULT '0000-00-00',
                    ""reg_accept_time""	datetime DEFAULT '0000-00-00',
                    ""api_token_id""	varchar(64),
                    ""client_id""	varchar(64),
                    ""client_secret""	varchar(64),
                    ""now_class_sid""	int DEFAULT 0,
                    ""petty_cash""	decimal(10, 2) DEFAULT 0,
                    ""business_day""	datetime,
                    ""business_close_time""	datetime,
                    ""invoice_flag""	char(1) DEFAULT 'N',
                    ""invoice_batch_num""	INT DEFAULT 0,
                    ""invoice_active_state""	char(1) DEFAULT 'N',
                    ""last_order_no""	varchar(20),
                    ""use_call_num""	integer DEFAULT 0,
                    ""use_call_date""	char(8),
                    ""online_time""	datetime DEFAULT '0000-00-00',
                    ""keyhook_enable""	char(1) DEFAULT 'N',
                    ""last_check_update_time""	datetime DEFAULT NULL,
                    ""last_class_report_no""	varchar(30),
                    ""last_daily_report_no""	varchar(30),
                    ""use_call_num_date""	char(8),
                    ""params""	TEXT DEFAULT null,
                    PRIMARY KEY(""SID"")
                    )";
                    break;
                case "order_content_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""order_content_data"" (
                    ""order_no""	varchar(30) NOT NULL,
                    ""item_no""	int NOT NULL,
                    ""data_type""	char(1) NOT NULL DEFAULT 'A',
                    ""item_type""	char(1) NOT NULL,
                    ""item_code""	varchar(64),
                    ""item_group_code""	varchar(64),
                    ""item_group_name""	varchar(64),
                    ""show_detail_flag""	char(1) DEFAULT 'N',
                    ""condiment_group_sid""	int DEFAULT 0,
                    ""parent_item_no""	int DEFAULT 0,
                    ""item_sid""	int NOT NULL,
                    ""item_spec_sid""	int,
                    ""item_name""	varchar(30) NOT NULL,
                    ""item_cost""	decimal(10, 2) NOT NULL DEFAULT 0,
                    ""item_count""	decimal(10, 2) NOT NULL DEFAULT 0,
                    ""condiment_price""	decimal(10, 2) DEFAULT 0,
                    ""item_subtotal""	decimal(10, 2) NOT NULL DEFAULT 0,
                    ""discount_type""	char(1) DEFAULT 'N',
                    ""discount_code""	varchar(60),
                    ""discount_name""	varchar(60),
                    ""discount_rate""	decimal(10, 2),
                    ""discount_fee""	decimal(10, 2),
                    ""discount_info""	text,
                    ""stock_remainder_quantity""	int DEFAULT 0,
                    ""stock_push_price""	decimal(10, 2) DEFAULT 0,
                    ""stock_push_quantity""	int DEFAULT 0,
                    ""stock_push_amount""	decimal(10, 2) DEFAULT 0,
                    ""stock_pull_code""	varchar(60),
                    ""stock_pull_name""	varchar(60),
                    ""stock_pull_price""	decimal(10, 2) DEFAULT 0,
                    ""stock_pull_quantity""	int DEFAULT 0,
                    ""stock_pul_amount""	decimal(10, 2) DEFAULT 0,
                    ""external_id""	varchar(100),
                    ""external_mode""	varchar(50),
                    ""external_description""	text,
                    ""tax_sid""	int,
                    ""tax_rate""	decimal(10, 2) DEFAULT 0,
                    ""tax_type""	char(1),
                    ""tax_fee""	decimal(10, 2) DEFAULT 0,
                    ""item_amount""	decimal(10, 2) DEFAULT 0,
                    ""subtotal_flag""	char(1) DEFAULT 'N',
                    ""subtotal_item_no""	int,
                    ""cust_name""	varchar(50),
                    ""other_info""	text,
                    ""print_flag""	char(1) DEFAULT 'N',
                    ""print_count""	int,
                    ""del_flag""	char(1) NOT NULL DEFAULT 'N',
                    ""del_time""	datetime DEFAULT NULL,
                    ""stock_pull_amount""	decimal(10, 2) DEFAULT 0,
                    PRIMARY KEY(""order_no"",""item_no"",""data_type"")
                    )";
                    break;
                case "daily_report":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""daily_report"" (
                    ""report_no""	varchar(20) NOT NULL,
                    ""report_type""	char(1) NOT NULL,
                    ""report_time""	datetime NOT NULL,
                    ""business_day""	datetime,
                    ""class_sid""	int DEFAULT 0,
                    ""class_name""	varchar(20),
                    ""employee_no""	varchar(20),
                    ""order_start_time""	datetime NOT NULL,
                    ""order_end_time""	datetime NOT NULL,
                    ""order_count""	int DEFAULT 0,
                    ""sale_total""	decimal(10, 2) DEFAULT 0,
                    ""sale_item_count""	int DEFAULT 0,
                    ""sale_item_avg_cost""	decimal(10, 2) DEFAULT 0,
                    ""cancel_count""	int DEFAULT 0,
                    ""cancel_total""	decimal(10, 2) DEFAULT 0,
                    ""other_cancel_count""	int DEFAULT 0,
                    ""other_cancel_total""	decimal(10, 2) DEFAULT 0,
                    ""discount_total""	decimal(10, 2) DEFAULT 0,
                    ""promotion_total""	decimal(10, 2) DEFAULT 0,
                    ""coupon_total""	decimal(10, 2) DEFAULT 0,
                    ""tax_total""	decimal(10, 2) DEFAULT 0,
                    ""service_total""	decimal(10, 2) DEFAULT 0,
                    ""trans_reversal""	decimal(10, 2) DEFAULT 0,
                    ""over_paid""	decimal(10, 2) DEFAULT 0,
                    ""sale_amount""	decimal(10, 2) DEFAULT 0,
                    ""refund_cash_total""	decimal(10, 2) DEFAULT 0,
                    ""cash_total""	decimal(10, 2) DEFAULT 0,
                    ""payment_info""	text,
                    ""payment_cash_total""	decimal(10, 2) DEFAULT 0,
                    ""expense_info""	text,
                    ""expense_cash_total""	decimal(10, 2) DEFAULT 0,
                    ""coupon_info""	text,
                    ""stock_push_amount""	decimal(10, 2) DEFAULT 0,
                    ""stock_pull_amount""	decimal(10, 2) DEFAULT 0,
                    ""inv_summery_info""	text,
                    ""upload_flag""	char(1) DEFAULT 'N',
                    ""upload_time""	datetime,
                    ""category_sale_info""	TEXT DEFAULT null,
                    ""promotions_info""	TEXT DEFAULT null,
                    ""checkout_info""	TEXT DEFAULT null,
                    PRIMARY KEY(""report_no"",""report_type"")
                    )";
                    break;
                case "order_data":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""order_data"" (
                    ""order_no""	varchar(20) NOT NULL,
                    ""order_no_from""	char(1) NOT NULL,
                    ""order_time""	datetime NOT NULL,
                    ""order_type""	int NOT NULL,
                    ""order_type_name""	varchar(50),
                    ""order_type_code""	varchar(30),
                    ""order_open_time""	datetime,
                    ""order_state""	int DEFAULT 0,
                    ""order_mode""	char(1) DEFAULT 'L',
                    ""terminal_sid""	varchar(20) NOT NULL,
                    ""pos_no""	varchar(20),
                    ""class_sid""	int NOT NULL,
                    ""class_name""	varchar(20),
                    ""user_sid""	int NOT NULL,
                    ""employee_no""	varchar(20),
                    ""table_code""	varchar(10),
                    ""table_name""	varchar(10),
                    ""meal_num""	varchar(20),
                    ""call_num""	varchar(10),
                    ""member_flag""	char(1) DEFAULT 'N',
                    ""member_platform""	varchar(30),
                    ""member_no""	varchar(60),
                    ""member_name""	varchar(30),
                    ""member_phone""	varchar(20),
                    ""member_info""	text,
                    ""takeaways_order_sid""	varchar(64),
                    ""takeaways_order_info""	text,
                    ""outside_order_no""	varchar(60),
                    ""outside_description""	text,
                    ""delivery_city_name""	varchar(20),
                    ""delivery_district_name""	varchar(20),
                    ""delivery_address""	varchar(100),
                    ""item_count""	decimal(10, 2) DEFAULT 0,
                    ""subtotal""	decimal(10, 2) DEFAULT 0,
                    ""discount_fee""	decimal(10, 2) DEFAULT 0,
                    ""promotion_fee""	decimal(10, 2),
                    ""promotion_value""	text,
                    ""coupon_discount""	decimal(10, 2),
                    ""coupon_value""	text,
                    ""stock_push_quantity""	int DEFAULT 0,
                    ""stock_push_amount""	decimal(10, 2) DEFAULT 0,
                    ""stock_pull_quantity""	int DEFAULT 0,
                    ""stock_pull_amount""	decimal(10, 2) DEFAULT 0,
                    ""service_rate""	decimal(10, 2) DEFAULT 0,
                    ""service_fee""	decimal(10, 2) DEFAULT 0,
                    ""trans_reversal""	decimal(10, 2) DEFAULT 0,
                    ""over_paid""	decimal(10, 2) DEFAULT 0,
                    ""tax_fee""	decimal(10, 2) DEFAULT 0,
                    ""amount""	decimal(10, 2) DEFAULT 0,
                    ""paid_type_sid""	int,
                    ""paid_flag""	char(1) DEFAULT 'N',
                    ""paid_time""	datetime DEFAULT NULL,
                    ""paid_info""	text,
                    ""cash_fee""	decimal(10, 2) DEFAULT 0,
                    ""change_fee""	decimal(10, 2) DEFAULT 0,
                    ""invoice_flag""	char(1) DEFAULT 'N',
                    ""invoice_info""	text,
                    ""cust_ein""	varchar(8),
                    ""cancel_flag""	char(1) DEFAULT 'N',
                    ""cancel_time""	datetime DEFAULT NULL,
                    ""cancel_class_sid""	int,
                    ""cancel_class_name""	varchar(20),
                    ""cash_refund_flag""	char DEFAULT 'N',
                    ""refund""	decimal(10, 2),
                    ""refund_type_sid""	int,
                    ""cancel_upload_flag""	char(1) DEFAULT 'N',
                    ""cancel_upload_time""	datetime,
                    ""del_flag""	char(1) DEFAULT 'N',
                    ""business_day""	datetime,
                    ""class_close_flag""	char(1) DEFAULT 'N',
                    ""class_report_no""	varchar(20),
                    ""daily_close_flag""	char(1) DEFAULT 'N',
                    ""daily_report_no""	varchar(20),
                    ""order_temp_info""	text,
                    ""upload_flag""	char(1) DEFAULT 'N',
                    ""upload_time""	datetime DEFAULT NULL,
                    ""created_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    ""updated_time""	timestamp DEFAULT CURRENT_TIMESTAMP,
                    ""remarks""	text,
                    ""guests_num""	INT DEFAULT 0,
                    PRIMARY KEY(""order_no"")
                    )";
                    break;
                case "printer_group_order_type_relation":
                    StrResult = @"CREATE TABLE IF NOT EXISTS ""printer_group_order_type_relation"" (
                    ""printer_group_sid""	int NOT NULL,
                    ""order_type_sid""	int NOT NULL,
                    PRIMARY KEY(""printer_group_sid"",""order_type_sid"")
                    )";
                    break;
                case "":
                    StrResult = @"";
                    break;

            }
            return StrResult;
        }
        //---

    }//SQLDataTableModel
}
