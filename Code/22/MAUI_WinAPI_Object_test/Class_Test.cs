using CommunityToolkit.Maui.Views;
using MAUI_WinAPI_Object_test.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_WinAPI_Object_test
{
    public class Class_Test
    {
        public static MainPage MainPagePoint = null;
        public static async Task ShowPopup()
        {
            await MainPagePoint.ShowPopupAsync(new PopupPage1());//this.ShowPopup(new PopupPage1());
            await MainPagePoint.ShowPopupAsync(new GridPage());
        }

        public static async Task<int> CallShowPopup()
        {
            int intResult = 110;
            await Class_Test.ShowPopup();
            return intResult;
        }
    }
}
