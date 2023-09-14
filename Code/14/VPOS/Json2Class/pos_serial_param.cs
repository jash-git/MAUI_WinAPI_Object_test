using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
	/*
	{
		"terminal_server_flag": "N",
		"terminal_server_port": 8080,
		"order_no_from":"L",
		"serial_server_name":"127.0.0.1",
		"serial_server_port":8080
	} 
	*/
	public class pos_serial_param
	{
		public string terminal_server_flag { get; set; }
		public int terminal_server_port { get; set; }
		public string order_no_from { get; set; }
		public string serial_server_name { get; set; }
		public int serial_server_port { get; set; }

		public pos_serial_param()
		{
			terminal_server_flag = "N";//Y
			terminal_server_port = 8080;
			order_no_from = "L";//S
			serial_server_name = "127.0.0.1";
			serial_server_port = 8080;
		}
	}
}
