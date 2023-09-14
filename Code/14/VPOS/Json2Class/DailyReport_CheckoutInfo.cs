using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    //用來產生daily_report的checkout_info儲存資料
    public class DailyReport_CheckoutInfo
    {
        public object easycard { get; set; }
        public object nccc { get; set; }
    }
    /*
    //測試範例
    //https://dotnetfiddle.net/
	using System;
	using System.IO;
	using System.Text.Json;

	public class Root
	{
		public object easycard { get; set; }
		public object nccc { get; set; }
	}

	public class nccc
	{
		public int a { get; set; }
		public int b { get; set; }
	}

	public class Program
	{
		public static void Main()
		{
			nccc nccc1=new nccc();
			nccc1.a=10;
			nccc1.b=20;

			Root Root01=new Root();
			Root01.nccc=nccc1;
			Root01.easycard="{}";

			//---
			//obj -> string
			String StrResult = JsonSerializer.Serialize(Root01);
			Console.WriteLine(StrResult);
			//---
		
			//---
			//string -> obj
			Root Result = JsonSerializer.Deserialize<Root>(StrResult);
			nccc nccc2 = JsonSerializer.Deserialize<nccc>(Result.nccc.ToString());
			Console.WriteLine("nccc2.a="+nccc2.a);
			Console.WriteLine("nccc2.b="+nccc2.b);
			//---
		
		}
	}
    */
}
