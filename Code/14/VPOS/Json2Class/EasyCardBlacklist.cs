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
	    "data": {
		    "sandbox": {
			    "SID": "7d650890-832a-11ed-9210-b98d828106cf",
			    "data_module": "EASY_CARD",
			    "data_type": "BLACK_LIST_SANDBOX",
			    "data_time": 1681781405,
			    "file_name": "BLC03342A_190703.BIG",
			    "version": null,
			    "file_info": {
				    "location": "https://storage.googleapis.com/vteam-pub-storage/vteam-cloud-test/easy_card/BLACK_LIST_SANDBOX/BLC03342A_190703.BIG",
				    "download_id": "7d650890-832a-11ed-9210-b98d828106cf"
			    }
		    },
		    "production": {
			    "SID": "8f03e060-dd88-11ed-80a7-ef0762a062b2",
			    "data_module": "EASY_CARD",
			    "data_type": "BLACK_LIST",
			    "data_time": 1681781410,
			    "file_name": "BLC07114A_230417.BIG",
			    "version": null,
			    "file_info": {
				    "location": "https://storage.googleapis.com/vteam-pub-storage/vteam-cloud-test/easy_card/BLACK_LIST/BLC07114A_230417.BIG",
				    "download_id": "8f03e060-dd88-11ed-80a7-ef0762a062b2"
			    }
		    }
	    }
    }
    */
    public class ECBData
    {
        public ECBSandbox sandbox { get; set; }
        public ECBProduction production { get; set; }
    }

    public class ECBFileInfo
    {
        public string location { get; set; }
        public string download_id { get; set; }
    }

    public class ECBProduction
    {
        public string SID { get; set; }
        public string data_module { get; set; }
        public string data_type { get; set; }
        public int data_time { get; set; }
        public string file_name { get; set; }
        public object version { get; set; }
        public ECBFileInfo file_info { get; set; }
    }

    public class EasyCardBlacklist
    {
        public string status { get; set; }
        public string message { get; set; }
        public ECBData data { get; set; }
    }

    public class ECBSandbox
    {
        public string SID { get; set; }
        public string data_module { get; set; }
        public string data_type { get; set; }
        public int data_time { get; set; }
        public string file_name { get; set; }
        public object version { get; set; }
        public ECBFileInfo file_info { get; set; }
    }
}
