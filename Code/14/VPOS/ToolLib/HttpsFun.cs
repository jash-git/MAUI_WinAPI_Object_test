using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VPOS
{
    public class VPOS_Env
    {
        public string EnvURL { get; set; }//測試網站URL
    }

    public class HttpsFun
    {
        private static bool m_blnlogfile = true;
        private static bool m_blnTestMode = true;
        public static List<String> m_ListDomain = new List<string>();
        public static List<String> m_ListDomainTest = new List<string>();
        public static int m_intNetworkLevel =0;

        public static void DomainVarInit(bool blnGetDBValue=false)
        {
            m_ListDomain.Clear();
            m_ListDomainTest.Clear();
            String StrEnvUrl = @"https://test.vteampos.com";//預設測試主機URL
            /*
            //---
            //讀取vpos.env設定檔
            if (File.Exists(LogFile.m_StrSysPath + "vpos.env"))
            {
                StreamReader sr = new StreamReader(LogFile.m_StrSysPath + "vpos.env");
                string StrInput = sr.ReadLine();
                sr.Close();// 關閉串流
                
                VPOS_Env VPOS_EnvBuf = JsonClassConvert.VPOS_Env2Class(StrInput);
                if((VPOS_EnvBuf != null) && (VPOS_EnvBuf.EnvURL.Length>0)) 
                {
                    StrEnvUrl = VPOS_EnvBuf.EnvURL;
                }

                m_blnTestMode = true;
            }
            else
            {
                m_blnTestMode = false;
            }
            //---讀取vpos.env設定檔
            */

            //---
            //讀取vtp.env設定檔
            if (File.Exists(LogFile.m_StrSysPath + "vtp.env"))
            {
                IniManager iniManager = new IniManager(LogFile.m_StrSysPath + "vtp.env");

                StrEnvUrl = iniManager.ReadIniFile("VTEAM_SERVER", "Cloud_URL", "default");

                m_blnTestMode = true;
            }
            else
            {
                m_blnTestMode = false;
            }
            //---讀取vtp.env設定檔

            m_ListDomain.Add("https://www.google.com.tw");
            m_ListDomain.Add("https://cloud.vteampos.com");
            //m_ListDomain.Add("https://vdes.vteam-cloud.com");

            m_ListDomainTest.Add("https://www.google.com.tw");
            m_ListDomainTest.Add(StrEnvUrl);
            //m_ListDomainTest.Add("https://test-vdes.vteam-cloud.com");

            if(blnGetDBValue)
            {
                DataTable basic_paramsDataTable = new DataTable();
                String SQL = "SELECT param_value FROM basic_params WHERE param_key='API_URL_INFO' LIMIT 0,1";
                basic_paramsDataTable = SQLDataTableModel.GetDataTable(SQL);
                if ((basic_paramsDataTable != null) && (basic_paramsDataTable.Rows.Count > 0))
                {
                    String StrJSON = basic_paramsDataTable.Rows[0][0].ToString();
                    get_basic_params_param_value get_basic_params_param_valueBuf = JsonClassConvert.get_basic_params_param_value2Class(StrJSON);
                    if(get_basic_params_param_valueBuf!=null)
                    {
                        if((get_basic_params_param_valueBuf.VDES_API_URL!=null) &&(get_basic_params_param_valueBuf.VDES_API_URL.Length>0))
                        {//setDomainMode(2) - vdes
                            m_ListDomain.Add(get_basic_params_param_valueBuf.VDES_API_URL.Replace("/api",""));
                            m_ListDomainTest.Add(get_basic_params_param_valueBuf.VDES_API_URL.Replace("/api", ""));
                        }

                        if ((get_basic_params_param_valueBuf.PAYMENT_API_URL != null) && (get_basic_params_param_valueBuf.PAYMENT_API_URL.Length > 0))
                        {//setDomainMode(3) - pay
                            m_ListDomain.Add(get_basic_params_param_valueBuf.PAYMENT_API_URL.Replace("/api", ""));
                            m_ListDomainTest.Add(get_basic_params_param_valueBuf.PAYMENT_API_URL.Replace("/api", ""));
                        }
                    }
                }
            }
            else
            {
                m_ListDomain.Add("https://vdes.vteam-cloud.com");
                m_ListDomainTest.Add("https://test-vdes.vteam-cloud.com");
            }

        }

        private static String m_StrDomain = "https://test.vteampos.com/api";//"https://test-vdes.vteam-cloud.com";
        public static String setDomainMode(int index)
        {
            //m_StrDomain = (m_blnTestMode) ? "https://test-vdes.vteam-cloud.com" : "https://vdes.vteam-cloud.com";
            //m_StrDomain = (m_blnTestMode) ? "https://test.vteampos.com/api" : "https://cloud.vteampos.com/api";
            
            if(m_ListDomainTest.Count> index)
            {
                m_StrDomain = (m_blnTestMode) ? m_ListDomainTest[index] : m_ListDomain[index];
            }
            else
            {
                m_StrDomain = "";
            }
            

            return m_StrDomain;
        }

        public static void setHeader(ref HttpWebRequest request,String StrHeaderName,String StrHeaderValue)//新增HTTP/HTTPS的Header參數
        {
            if((StrHeaderName.Length>0)&&(StrHeaderValue.Length>0))
            {
                request.Headers[StrHeaderName] = StrHeaderValue;
            }
        }

        public static bool DownloadFile(String StrUrl,String StrFilePath)
        {
            bool blnResult = false;
            String StrLog;
            WebClient wc = new WebClient();
            try
            {
                if(File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }
                wc.DownloadFile(StrUrl, StrFilePath);
                if (File.Exists(StrFilePath))
                {
                    blnResult = true;
                    StrLog = String.Format("DownloadFile ({0}): {1};{2}", StrUrl, "", StrFilePath);
                    LogFile.Write("HttpsNormal ; " + StrLog);
                }
                else
                {
                    StrLog = String.Format("DownloadFile ({0}): {1};{2}", StrUrl, "", StrFilePath);
                    LogFile.Write("HttpsError ; " + StrLog);
                    blnResult = false;
                }
            }
            catch(Exception e)
            {
                if (true)
                {
                    StrLog = String.Format("DownloadFile ({0}): {1};{2}", StrUrl, "", e.Message);
                    LogFile.Write("HttpsError ; " + StrLog);
                }
            }
            return blnResult;
        }

        public static String RESTfulAPI_get(String StrDomain,String path, String StrInput, String StrHeaderName = "", String StrHeaderValue = "")
        {
            //string url= "http://192.168.1.68:24410/syris/sydm/controller";
            String mStrHeaderName = StrHeaderName;
            String mStrHeaderValue = StrHeaderValue;
            String StrData = "";
            String url = StrDomain + path;

            if (StrInput.Length > 0)
            {
                url += "/";
                url += HttpUtility.UrlEncode(StrInput);
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                setHeader(ref request, mStrHeaderName, mStrHeaderValue);
                request.KeepAlive = false;

                //---
                //定義此req的緩存策略
                //https://msdn.microsoft.com/zh-tw/library/system.net.webrequest.cachepolicy(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-1
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                request.CachePolicy = noCachePolicy;
                //---定義此req的緩存策略

                request.Method = "GET";//request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";

                Thread.Sleep(100);
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string encoding = response.ContentEncoding;

                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //預設編碼
                }

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));

                StrData = reader.ReadToEnd();

                response.Close();

                //---
                //手動強制執行當呼叫完 CG API 的強制關閉連線補強措施
                request.Abort();
                response = null;
                request = null;
                //GC.Collect();//手動記憶體回收
                //---手動強制執行當呼叫完 CG API 的強制關閉連線補強措施

                if (m_blnlogfile)
                {
                    String StrLog = String.Format("RESTfulAPI_get ({0}): {1};{2}", url, "", StrData);
                    LogFile.Write("HttpsNormal ; " + StrLog);//LogFile.Write(DateTime.Now.ToString("yyyyMMdd") + ".log", StrLog);
                }
            }
            catch (Exception e)
            {
                StrData += e.Message;

                if (true)
                {
                    String StrLog = String.Format("RESTfulAPI_getBody ({0}): {1};{2}", url, "", StrData);
                    LogFile.Write("HttpsError ; " + StrLog);
                }
            }

            return StrData;
        }


        public static String RESTfulAPI_get(String StrDomain, String path, String StrInput, String[] StrHeaderName, String[] StrHeaderValue)
        {
            //string url= "http://192.168.1.68:24410/syris/sydm/controller";
            String StrData = "";
            String url = StrDomain + path;

            if (StrInput.Length > 0)
            {
                url += "/";
                url += HttpUtility.UrlEncode(StrInput);
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                for (int i = 0; i < StrHeaderName.Length; i++)
                {
                    setHeader(ref request, StrHeaderName[i], StrHeaderValue[i]);
                }
                
                request.KeepAlive = false;

                //---
                //定義此req的緩存策略
                //https://msdn.microsoft.com/zh-tw/library/system.net.webrequest.cachepolicy(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-1
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                request.CachePolicy = noCachePolicy;
                //---定義此req的緩存策略

                request.Method = "GET";//request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";

                Thread.Sleep(100);
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string encoding = response.ContentEncoding;

                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //預設編碼
                }

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));

                StrData = reader.ReadToEnd();

                response.Close();

                //---
                //手動強制執行當呼叫完 CG API 的強制關閉連線補強措施
                request.Abort();
                response = null;
                request = null;
                //GC.Collect();//手動記憶體回收
                //---手動強制執行當呼叫完 CG API 的強制關閉連線補強措施

                if (m_blnlogfile)
                {
                    String StrLog = String.Format("RESTfulAPI_get ({0}): {1};{2}", url, "", StrData);
                    LogFile.Write("HttpsNormal ; " + StrLog);//LogFile.Write(DateTime.Now.ToString("yyyyMMdd") + ".log", StrLog);
                }
            }
            catch (Exception e)
            {
                StrData += e.Message;

                if (true)
                {
                    String StrLog = String.Format("RESTfulAPI_getBody ({0}): {1};{2}", url, "", StrData);
                    LogFile.Write("HttpsError ; " + StrLog);
                }
            }

            return StrData;
        }

        public static String RESTfulAPI_get(String path, String StrInput, String StrHeaderName = "", String StrHeaderValue = "")
        {
            //string url= "http://192.168.1.68:24410/syris/sydm/controller";
            String mStrHeaderName = StrHeaderName;
            String mStrHeaderValue = StrHeaderValue;
            String StrData = "";
            String url = m_StrDomain + path;

            if(StrInput.Length>0)
            {
                url += "/";
                url += HttpUtility.UrlEncode(StrInput);
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                setHeader(ref request, mStrHeaderName, mStrHeaderValue);
                request.KeepAlive = false;

                //---
                //定義此req的緩存策略
                //https://msdn.microsoft.com/zh-tw/library/system.net.webrequest.cachepolicy(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-1
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                request.CachePolicy = noCachePolicy;
                //---定義此req的緩存策略

                request.Method = "GET";//request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";

                Thread.Sleep(100);
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string encoding = response.ContentEncoding;

                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //預設編碼
                }

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));

                StrData = reader.ReadToEnd();

                response.Close();

                //---
                //手動強制執行當呼叫完 CG API 的強制關閉連線補強措施
                request.Abort();
                response = null;
                request = null;
                //GC.Collect();//手動記憶體回收
                //---手動強制執行當呼叫完 CG API 的強制關閉連線補強措施

                if (m_blnlogfile)
                {
                    String StrLog = String.Format("RESTfulAPI_get ({0}): {1};{2}", url, "", StrData);
                    LogFile.Write("HttpsNormal ; " + StrLog);//LogFile.Write(DateTime.Now.ToString("yyyyMMdd") + ".log", StrLog);
                }
            }
            catch (Exception e)
            {
                StrData += e.Message;

                if (true)
                {
                    String StrLog = String.Format("RESTfulAPI_getBody ({0}): {1};{2}", url, "", StrData);
                    LogFile.Write("HttpsError ; " + StrLog);
                }
            }

            return StrData;
        }

        public static String RESTfulAPI_postBody(String path, String StrInput, String StrHeaderName="", String StrHeaderValue="")
        {
            String mStrHeaderName = StrHeaderName;
            String mStrHeaderValue = StrHeaderValue;
            String StrData = "";
            String url = m_StrDomain + path;

            HttpWebRequest request = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                setHeader(ref request, mStrHeaderName, mStrHeaderValue);
                request.KeepAlive = false;
                //---
                //定義此req的緩存策略
                //https://msdn.microsoft.com/zh-tw/library/system.net.webrequest.cachepolicy(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-1
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                request.CachePolicy = noCachePolicy;
                //---定義此req的緩存策略
                request.Method = "POST";

                //---
                //request.ContentType = "application/x-www-form-urlencoded";//一般POST
                request.ContentType = "application/json; charset=UTF-8";//POST to AJAX [is_ajax_request()]
                //request.Accept = "application/json, text/javascript";//POST to AJAX [is_ajax_request()]
                //request.UserAgent = "";//POST to AJAX [is_ajax_request()]
                //request.Headers.Add("X-Requested-With", "XMLHttpRequest");//POST to AJAX [is_ajax_request()]
                //---

                //request.ContentLength = data1.Length;
                //StreamWriter writer = new StreamWriter(request.GetRequestStream());//CS2PHPrestfulapi 傳送全部改為UTF8
                //writer.Write(StrInput);
                //writer.Flush();
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(StrInput);
                    streamWriter.Flush();
                }

                Thread.Sleep(100);
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码
                }

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));

                StrData = reader.ReadToEnd();

                response.Close();

                //---
                //手動強制執行當呼叫完 CG API 的強制關閉連線補強措施
                request.Abort();
                response = null;
                request = null;
                //GC.Collect();//手動記憶體回收
                //---手動強制執行當呼叫完 CG API 的強制關閉連線補強措施
                
                if (m_blnlogfile)
                {
                    String StrLog = String.Format("RESTfulAPI_postBody ({0}): {1};{2}", url, StrInput, StrData);
                    LogFile.Write("HttpsNormal ; " + StrLog);//LogFile.Write(DateTime.Now.ToString("yyyyMMdd") + ".log", StrLog);
                }
            }
            catch (WebException e)
            {
                //httpwebrequest getresponse 400
                //https://stackoverflow.com/questions/692342/net-httpwebrequest-getresponse-raises-exception-when-http-status-code-400-ba
                using (WebResponse response = e.Response)
                {
                    if(response != null)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        //Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            StrData = reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        StrData = "NULL";
                    }

                }

                if (true)
                {
                    String StrLog = String.Format("RESTfulAPI_postBody ({0}): {1};{2}", url, StrInput, StrData);
                    LogFile.Write("HttpsError ; " + StrLog);
                }
            }

            return StrData;
        }

        public static String RESTfulAPI_postBody(String StrDomain, String path, String StrInput, String StrHeaderName = "", String StrHeaderValue = "")
        {
            String mStrHeaderName = StrHeaderName;
            String mStrHeaderValue = StrHeaderValue;
            String StrData = "";
            String url = StrDomain + path;

            HttpWebRequest request = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                setHeader(ref request, mStrHeaderName, mStrHeaderValue);
                request.KeepAlive = false;
                //---
                //定義此req的緩存策略
                //https://msdn.microsoft.com/zh-tw/library/system.net.webrequest.cachepolicy(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-1
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                request.CachePolicy = noCachePolicy;
                //---定義此req的緩存策略
                request.Method = "POST";

                //---
                //request.ContentType = "application/x-www-form-urlencoded";//一般POST
                request.ContentType = "application/json; charset=UTF-8";//POST to AJAX [is_ajax_request()]
                //request.Accept = "application/json, text/javascript";//POST to AJAX [is_ajax_request()]
                //request.UserAgent = "";//POST to AJAX [is_ajax_request()]
                //request.Headers.Add("X-Requested-With", "XMLHttpRequest");//POST to AJAX [is_ajax_request()]
                //---

                //request.ContentLength = data1.Length;
                //StreamWriter writer = new StreamWriter(request.GetRequestStream());//CS2PHPrestfulapi 傳送全部改為UTF8
                //writer.Write(StrInput);
                //writer.Flush();
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(StrInput);
                    streamWriter.Flush();
                }

                Thread.Sleep(100);
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码
                }

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));

                StrData = reader.ReadToEnd();

                response.Close();

                //---
                //手動強制執行當呼叫完 CG API 的強制關閉連線補強措施
                request.Abort();
                response = null;
                request = null;
                //GC.Collect();//手動記憶體回收
                //---手動強制執行當呼叫完 CG API 的強制關閉連線補強措施

                if (m_blnlogfile)
                {
                    String StrLog = String.Format("RESTfulAPI_postBody ({0}): {1};{2}", url, StrInput, StrData);
                    LogFile.Write("HttpsNormal ; " + StrLog);//LogFile.Write(DateTime.Now.ToString("yyyyMMdd") + ".log", StrLog);
                }
            }
            catch (WebException e)
            {
                //httpwebrequest getresponse 400
                //https://stackoverflow.com/questions/692342/net-httpwebrequest-getresponse-raises-exception-when-http-status-code-400-ba
                using (WebResponse response = e.Response)
                {
                    if (response != null)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        //Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            StrData = reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        StrData = "NULL";
                    }

                }

                if (true)
                {
                    String StrLog = String.Format("RESTfulAPI_postBody ({0}): {1};{2}", url, StrInput, StrData);
                    LogFile.Write("HttpsError ; " + StrLog);
                }
            }

            return StrData;
        }

        public static String RESTfulAPI_postBody(String path, String StrInput, String []StrHeaderName, String []StrHeaderValue)
        {
            String StrData = "";
            String url = m_StrDomain + path;

            HttpWebRequest request = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);

                for(int i=0;i< StrHeaderName.Length;i++)
                {
                    setHeader(ref request, StrHeaderName[i], StrHeaderValue[i]);
                }
                
                request.KeepAlive = false;
                //---
                //定義此req的緩存策略
                //https://msdn.microsoft.com/zh-tw/library/system.net.webrequest.cachepolicy(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-1
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                request.CachePolicy = noCachePolicy;
                //---定義此req的緩存策略
                request.Method = "POST";

                //---
                //request.ContentType = "application/x-www-form-urlencoded";//一般POST
                request.ContentType = "application/json; charset=UTF-8";//POST to AJAX [is_ajax_request()]
                //request.Accept = "application/json, text/javascript";//POST to AJAX [is_ajax_request()]
                //request.UserAgent = "";//POST to AJAX [is_ajax_request()]
                //request.Headers.Add("X-Requested-With", "XMLHttpRequest");//POST to AJAX [is_ajax_request()]
                //---

                //request.ContentLength = data1.Length;
                //StreamWriter writer = new StreamWriter(request.GetRequestStream());//CS2PHPrestfulapi 傳送全部改為UTF8
                //writer.Write(StrInput);
                //writer.Flush();
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(StrInput);
                    streamWriter.Flush();
                }

                Thread.Sleep(100);
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码
                }

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));

                StrData = reader.ReadToEnd();

                response.Close();

                //---
                //手動強制執行當呼叫完 CG API 的強制關閉連線補強措施
                request.Abort();
                response = null;
                request = null;
                //GC.Collect();//手動記憶體回收
                //---手動強制執行當呼叫完 CG API 的強制關閉連線補強措施

                if (m_blnlogfile)
                {
                    String StrLog = String.Format("RESTfulAPI_postBody ({0}): {1};{2}", url, StrInput, StrData);
                    LogFile.Write("HttpsNormal ; " + StrLog);//LogFile.Write(DateTime.Now.ToString("yyyyMMdd") + ".log", StrLog);
                }
            }
            catch (WebException e)
            {
                //httpwebrequest getresponse 400
                //https://stackoverflow.com/questions/692342/net-httpwebrequest-getresponse-raises-exception-when-http-status-code-400-ba
                using (WebResponse response = e.Response)
                {
                    if (response != null)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        //Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            StrData = reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        StrData = "NULL";
                    }

                }

                if (true)
                {
                    String StrLog = String.Format("RESTfulAPI_postBody ({0}): {1};{2}", url, StrInput, StrData);
                    LogFile.Write("HttpsError ; " + StrLog);
                }
            }

            return StrData;
        }

        public static String RESTfulAPI_postBody(String StrDomain, String path, String StrInput, String[] StrHeaderName, String[] StrHeaderValue)
        {
            String StrData = "";
            String url = StrDomain + path;

            HttpWebRequest request = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);

                for (int i = 0; i < StrHeaderName.Length; i++)
                {
                    setHeader(ref request, StrHeaderName[i], StrHeaderValue[i]);
                }

                request.KeepAlive = false;
                //---
                //定義此req的緩存策略
                //https://msdn.microsoft.com/zh-tw/library/system.net.webrequest.cachepolicy(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-1
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                request.CachePolicy = noCachePolicy;
                //---定義此req的緩存策略
                request.Method = "POST";

                //---
                //request.ContentType = "application/x-www-form-urlencoded";//一般POST
                request.ContentType = "application/json; charset=UTF-8";//POST to AJAX [is_ajax_request()]
                //request.Accept = "application/json, text/javascript";//POST to AJAX [is_ajax_request()]
                //request.UserAgent = "";//POST to AJAX [is_ajax_request()]
                //request.Headers.Add("X-Requested-With", "XMLHttpRequest");//POST to AJAX [is_ajax_request()]
                //---

                //request.ContentLength = data1.Length;
                //StreamWriter writer = new StreamWriter(request.GetRequestStream());//CS2PHPrestfulapi 傳送全部改為UTF8
                //writer.Write(StrInput);
                //writer.Flush();
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(StrInput);
                    streamWriter.Flush();
                }

                Thread.Sleep(100);
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码
                }

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));

                StrData = reader.ReadToEnd();

                response.Close();

                //---
                //手動強制執行當呼叫完 CG API 的強制關閉連線補強措施
                request.Abort();
                response = null;
                request = null;
                //GC.Collect();//手動記憶體回收
                //---手動強制執行當呼叫完 CG API 的強制關閉連線補強措施

                if (m_blnlogfile)
                {
                    String StrLog = String.Format("RESTfulAPI_postBody ({0}): {1};{2}", url, StrInput, StrData);
                    LogFile.Write("HttpsNormal ; " + StrLog);//LogFile.Write(DateTime.Now.ToString("yyyyMMdd") + ".log", StrLog);
                }
            }
            catch (WebException e)
            {
                //httpwebrequest getresponse 400
                //https://stackoverflow.com/questions/692342/net-httpwebrequest-getresponse-raises-exception-when-http-status-code-400-ba
                using (WebResponse response = e.Response)
                {
                    if (response != null)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        //Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            StrData = reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        StrData = "NULL";
                    }

                }

                if (true)
                {
                    String StrLog = String.Format("RESTfulAPI_postBody ({0}): {1};{2}", url, StrInput, StrData);
                    LogFile.Write("HttpsError ; " + StrLog);
                }
            }

            return StrData;
        }

        public static String RESTfulAPI_postBody(String StrDomain, String path, String []StrInputName,String []StrInputValue, String[] StrHeaderName, String[] StrHeaderValue)
        {
            String StrData = "";
            String url = StrDomain + path;
            String StrInput = "";

            HttpWebRequest request = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);

                for (int i = 0; i < StrHeaderName.Length; i++)
                {
                    setHeader(ref request, StrHeaderName[i], StrHeaderValue[i]);
                }

                request.KeepAlive = false;
                //---
                //定義此req的緩存策略
                //https://msdn.microsoft.com/zh-tw/library/system.net.webrequest.cachepolicy(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-1
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                request.CachePolicy = noCachePolicy;
                //---定義此req的緩存策略
                request.Method = "POST";

                //---
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";//一般POST
                //request.ContentType = "application/json; charset=UTF-8";//POST to AJAX [is_ajax_request()]
                //request.Accept = "application/json, text/javascript";//POST to AJAX [is_ajax_request()]
                //request.UserAgent = "";//POST to AJAX [is_ajax_request()]
                //request.Headers.Add("X-Requested-With", "XMLHttpRequest");//POST to AJAX [is_ajax_request()]
                //---

                //必須透過ParseQueryString()來建立NameValueCollection物件，之後.ToString()才能轉換成queryString
                NameValueCollection postParams = System.Web.HttpUtility.ParseQueryString(string.Empty);
                for (int i = 0; i < StrInputName.Length; i++)
                {
                    if(StrInputValue[i].Length>0)
                    {
                        postParams.Add(StrInputName[i], StrInputValue[i]);
                    }             
                }
                StrInput = postParams.ToString();

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(StrInput);
                    streamWriter.Flush();
                }

                Thread.Sleep(100);
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码
                }

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));

                StrData = reader.ReadToEnd();

                response.Close();

                //---
                //手動強制執行當呼叫完 CG API 的強制關閉連線補強措施
                request.Abort();
                response = null;
                request = null;
                //GC.Collect();//手動記憶體回收
                //---手動強制執行當呼叫完 CG API 的強制關閉連線補強措施

                if (m_blnlogfile)
                {
                    String StrLog = String.Format("RESTfulAPI_postBody ({0}): {1};{2}", url, StrInput, StrData);
                    LogFile.Write("HttpsNormal ; " + StrLog);//LogFile.Write(DateTime.Now.ToString("yyyyMMdd") + ".log", StrLog);
                }
            }
            catch (WebException e)
            {
                //httpwebrequest getresponse 400
                //https://stackoverflow.com/questions/692342/net-httpwebrequest-getresponse-raises-exception-when-http-status-code-400-ba
                using (WebResponse response = e.Response)
                {
                    if (response != null)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        //Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            StrData = reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        StrData = "NULL";
                    }

                }

                if (true)
                {
                    String StrLog = String.Format("RESTfulAPI_postBody ({0}): {1};{2}", url, StrInput, StrData);
                    LogFile.Write("HttpsError ; " + StrLog);
                }
            }

            return StrData;
        }

        public static string GetPublicIPAddress(int mode=0)
        {
            //https://www.c-sharpcorner.com/blogs/how-to-get-public-ip-address-using-c-sharp1
            //https://www.codegrepper.com/code-examples/csharp/c%23+get+public+ip+address
            //https://codingvision.net/c-how-to-get-external-ip-address
            String address = "";
            switch(mode)
            {
                case 0:
                    using (WebClient client = new WebClient())
                    {
                        address = client.DownloadString("https://api.ipify.org/");
                    }
                    break;
                case 2:
                    address = new WebClient().DownloadString(@"http://icanhazip.com").Trim();
                    break;
                case 1:
                    WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                    using (WebResponse response = request.GetResponse())
                    using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                    {
                        address = stream.ReadToEnd();
                    }

                    int first = address.IndexOf("Address: ") + 9;
                    int last = address.LastIndexOf("</body>");
                    address = address.Substring(first, last - first);
                    break;
            }

            return address;
        }

        [DllImport("wininet")]
        public static extern bool InternetGetConnectedState(ref uint lpdwFlags, uint dwReserved);

        [DllImport("sensapi.dll", SetLastError = true)]
        public static extern bool IsNetworkAlive(out int connectionDescription);

        public static bool WebRequestTest(ref int level)
        {
            int flags = 0x0;
            //bool blnResult = InternetGetConnectedState(ref flags, 0);
            bool blnResult = IsNetworkAlive(out flags);

            if (blnResult)
            {
                try
                {
                    level = 1;
                    setDomainMode(0);//GOOGLE
                    System.Net.WebRequest myRequest = System.Net.WebRequest.Create(m_StrDomain);
                    System.Net.WebResponse myResponse = myRequest.GetResponse();
                    //LogFile.Write("network(GOOGLE) : Pass");
                }
                catch (System.Net.WebException)
                {
                    LogFile.Write("network(GOOGLE) : Fail");
                    blnResult =false;
                    return blnResult;
                }

                try
                {
                    level = 2;
                    setDomainMode(1);//POS
                    System.Net.WebRequest myRequest = System.Net.WebRequest.Create(m_StrDomain);
                    System.Net.WebResponse myResponse = myRequest.GetResponse();
                    //LogFile.Write("network(POS) : Pass");
                }
                catch (System.Net.WebException)
                {
                    LogFile.Write("network(POS) : Fail");
                    blnResult = false;
                    return blnResult;
                }

                blnResult = true;
                return blnResult;
            }
            else
            {
                LogFile.Write("network : Fail");
                level = 0;
                return blnResult;
            }

        }
    }//HttpsFun
}
