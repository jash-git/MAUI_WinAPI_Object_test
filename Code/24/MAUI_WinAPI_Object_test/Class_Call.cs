using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_WinAPI_Object_test
{
    public class Class_Call
    {
        public static async Task<int> CallShowPopup()
        {
            int intResult = 110;
            await Class_Test.ShowPopup();
            return intResult;
        }
    }
}
