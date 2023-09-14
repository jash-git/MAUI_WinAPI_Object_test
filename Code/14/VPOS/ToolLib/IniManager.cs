using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    //http://elmer-storage.blogspot.com/2018/06/c-ini.html
    /*
     * 要寫入的訊息
            IniManager iniManager = new IniManager("D:/test.ini");
 
            iniManager.WriteIniFile("Section_A", "Key_A", "1");
            iniManager.WriteIniFile("Section_B", "Key_B_1", "2");
            iniManager.WriteIniFile("Section_B", "Key_B_2", "3"); 
    */
    /*
     * 該Section下要讀取哪個Key            
            IniManager iniManager = new IniManager("D:/test.ini");
 
            iniManager.ReadIniFile("Section_A", "Key_A", "default");
            iniManager.ReadIniFile("Section_A", "Key_B", "default");
     */
    public class IniManager
    {
        private string filePath;
        private StringBuilder lpReturnedString;
        private int bufferSize;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string lpString, string lpFileName);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        public IniManager(string iniPath)
        {
            filePath = iniPath;
            bufferSize = 512;
            lpReturnedString = new StringBuilder(bufferSize);
        }

        // read ini date depend on section and key
        public string ReadIniFile(string section, string key, string defaultValue)
        {
            lpReturnedString.Clear();
            GetPrivateProfileString(section, key, defaultValue, lpReturnedString, bufferSize, filePath);
            return lpReturnedString.ToString();
        }

        // write ini data depend on section and key
        public void WriteIniFile(string section, string key, Object value)
        {
            WritePrivateProfileString(section, key, value.ToString(), filePath);
        }
    }
}
