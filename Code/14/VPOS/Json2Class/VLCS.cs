using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
	/*
	{
		"terminal_sid": "VT-POS-2020-00002",
		"api_token": "a03d80f0-b96f-11ec-9f59-092a8ad7c902",
		"machine_code": "B24AA89B50FD12C4E17A77DCC1B49837D7D4228C29EB4ADCBD47F5903506980C",
		"check_license_time": 1654752801
	}
	*/

	public class VLCS
	{
		public string terminal_sid { get; set; }
		public string api_token { get; set; }
		public string machine_code { get; set; }
		public Int64 check_license_time { get; set; }
	}
}
