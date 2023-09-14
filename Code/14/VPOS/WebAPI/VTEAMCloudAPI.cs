using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class VTEAMCloudAPI
    {
        private static int m_intLimitTime = 1;//分鐘
        private static oauthInput m_oauthInput = new oauthInput();
        public static void varInit(String client_id,String client_secret)
        {
            m_oauthInput.client_secret = client_secret;
            m_oauthInput.client_id = client_id; 
        }

        private static oauthResult m_oauthResult=new oauthResult();
        public static String m_Straccess_token = "";
        public static DateTime m_DTexpires_time=DateTime.Now;
        public static bool Authentication()
        {
            bool blnResult = false;

            if((m_Straccess_token.Length==0)||(ValidityCalculate()< m_intLimitTime))
            {
                String StrData = JsonClassConvert.oauthInput2String(m_oauthInput);

                String StrDomain = HttpsFun.setDomainMode(2);//vdes
                String StrResult = HttpsFun.RESTfulAPI_postBody(StrDomain,"/api/oauth", StrData,"","");
                m_oauthResult = JsonClassConvert.oauthResult2Class(StrResult);
                m_Straccess_token = "";
                m_DTexpires_time = DateTime.Now;
                if ( (m_oauthResult!=null) && (m_oauthResult.status == "OK") )
                {
                    blnResult = true;
                    m_Straccess_token = m_oauthResult.access_token;
                    m_DTexpires_time = TimeConvert.UnixTimeStampToDateTime(Convert.ToDouble(m_oauthResult.expires_unixtime));
                }
                else
                {
                    blnResult = false;
                }
            }
            else
            {
                blnResult = true;
            }

            return blnResult;
        }

        private static int ValidityCalculate()//回復還有幾分鐘失效
        {
            int intResult=0;
            TimeSpan ts1 = new TimeSpan(m_DTexpires_time.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            if (ts.TotalSeconds > 0)
            {
                intResult = (int)(ts.TotalSeconds / 60);
            }
            else
            {
                intResult = 0;
            }
            return intResult;
        }
    }//VTEAMCloudAPI
}
