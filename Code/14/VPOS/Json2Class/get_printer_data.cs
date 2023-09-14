using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    /*
	{
		"status": "ACCEPTED",
		"message": "",
		"data": [
			{
				"company_sid": 7,
				"printer_sid": 15,
				"printer_code": "BILL",
				"printer_name": "收據",
				"output_type": "B",
				"template_type": "BILL",
				"template_sid": "aa98e920-d775-11ed-a494-4d38ed24f824",
				"stop_flag": "N",
				"stop_time": null,
				"stop_unix_time": 0,
				"del_flag": "N",
				"del_time": null,
				"del_unix_time": 0,
				"created_unix_time": 1633503609,
				"updated_unix_time": 1681719275
			},
			{
				"company_sid": 7,
				"printer_sid": 16,
				"printer_code": "INVOICE",
				"printer_name": "發票",
				"output_type": "I",
				"template_type": "INVOICE",
				"template_sid": "2",
				"stop_flag": "N",
				"stop_time": null,
				"stop_unix_time": 0,
				"del_flag": "N",
				"del_time": null,
				"del_unix_time": 0,
				"created_unix_time": 1633503626,
				"updated_unix_time": 1679500803
			},
			{
				"company_sid": 7,
				"printer_sid": 17,
				"printer_code": "FORMULA",
				"printer_name": "智能食譜",
				"output_type": "S",
				"template_type": "SMART",
				"template_sid": "6",
				"stop_flag": "N",
				"stop_time": null,
				"stop_unix_time": 0,
				"del_flag": "N",
				"del_time": null,
				"del_unix_time": 0,
				"created_unix_time": 1633503650,
				"updated_unix_time": 1679500803
			},
			{
				"company_sid": 7,
				"printer_sid": 18,
				"printer_code": "REPORT",
				"printer_name": "報表",
				"output_type": "R",
				"template_type": "REPORT",
				"template_sid": "b1370a80-d777-11ed-a689-676e86eabdf8",
				"stop_flag": "N",
				"stop_time": null,
				"stop_unix_time": 0,
				"del_flag": "N",
				"del_time": null,
				"del_unix_time": 0,
				"created_unix_time": 1633503663,
				"updated_unix_time": 1681719501
			},
			{
				"company_sid": 7,
				"printer_sid": 36,
				"printer_code": "WORK",
				"printer_name": "工作票",
				"output_type": "W",
				"template_type": "WORK_TICKET",
				"template_sid": "457a01c0-d777-11ed-b86d-7bffe497587e",
				"stop_flag": "N",
				"stop_time": null,
				"stop_unix_time": 0,
				"del_flag": "N",
				"del_time": null,
				"del_unix_time": 0,
				"created_unix_time": 1679500803,
				"updated_unix_time": 1681719524
			},
			{
				"company_sid": 7,
				"printer_sid": 39,
				"printer_code": "標籤貼紙",
				"printer_name": "標籤貼紙",
				"output_type": "L",
				"template_type": "LABEL",
				"template_sid": "04476c40-d777-11ed-b0ca-1361ed3b7e26",
				"stop_flag": "N",
				"stop_time": null,
				"stop_unix_time": 0,
				"del_flag": "N",
				"del_time": null,
				"del_unix_time": 0,
				"created_unix_time": 1681716780,
				"updated_unix_time": 1681719540
			}
		]
	}
    */
    public class GPDDatum2
    {
        public int company_sid { get; set; }
        public int printer_sid { get; set; }
        public string printer_code { get; set; }
        public string printer_name { get; set; }
        public string output_type { get; set; }
        public string template_type { get; set; }
        public string template_sid { get; set; }
        public string stop_flag { get; set; }
        public int stop_time { get; set; }
        public int stop_unix_time { get; set; }
        public string del_flag { get; set; }
        public int del_time { get; set; }
        public int del_unix_time { get; set; }
        public int created_unix_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_printer_data
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GPDDatum2> data { get; set; }
    }
}
