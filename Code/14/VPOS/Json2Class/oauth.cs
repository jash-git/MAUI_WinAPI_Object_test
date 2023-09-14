using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{ 
	/*
	{
		"client_id": "b4f3edb0-b96f-11ec-8689-97b3100ea649",
		"client_secret": "4d23bb799b566da98e19f0de72108e2b2c784e6535bd62813c358563d3f357ff"
	}
	*/
	public class oauthInput
	{
		public string client_id { get; set; }
		public string client_secret { get; set; }
	}

	/*
	{
		"status": "OK",
		"message": "",
		"data": "",
		"access_token": "eyJpdiI6InVPcnQ0RU1uNFVrUlI1Y2oxOGM2elE9PSIsInZhbHVlIjoiYU5OZU83aHNsUWJPdUFWR0RNejJvd3lzL21GaFU3STFObUNOMm1UbEhDdlErbENsd05VQmN0QThpNzI2c09VRkRNR3U2eUJVcy9taml4UzJvVWZLb09LTjI4ZVBZZEM3TTlVdzhsQTNqam5JRFQ2WnJqSG8vUXdKOVZ1dTNVdE1iTmlJZnkzUHBjaUFZZ2VrNjVkS3FPcjFKSExKUnFPMEFmT2ZhUjBkUzRWcm0vTHZFQ2xlQlBseGJOL3Q4UkZPc1pMWnZVUWMzV0RyMDYxdmRRMXNpYjVldm10TjNPQWcvakx1a3hCUGZ6TkQ0WFJVbUpDdCt4VDVVaDFUZndiWFRFd2EwSWhrbitwa0cyUnZIVTN3RVMwaUx2dmRXaGJIUjBHRXRnL2k4M01CN2dvVVhOd0dPUTY2a094ZzhsMHBVaTR6SUN4VlpLNHJIU1ZOK01aMkszTjlyTXZuRXlLUGloeUFvQ1FTcmszeUladFYxbmdhU290RzVBV0N4UTBnRFJGc0c0VnNFTHk2amNtdk1NK0tFTlZiRTdscytHTXd5RXBZRHZvSDh0SnBaTjQxNjZBVmdsbkpJUDdTWUU0eVFtMFZQT1EyRHVzay85aWZRKzNOU2VmbndudldsT1RQNUJMK0tJNFU1cjVBUkF3d1NYTGZXaHJsWWcrOHRyNS9aTTN6UkVDTzE0V0VlRU9HZDlXNFpDdXFpcmRQNEloSDhyeS85cEZUYlVwNnF5b3hkc2YxVXcyMGFMTkdTUEdod3dQREVTd3M2L1RaKzZNTFA1STkzdlozN1hrWFFqUEw1S3NlcGxCWFZnZy95dm1ZcVcrY1ZNdWdZeGZnVFpUNyt4OTlEUitKaHErcXErenVsWU16YW1yN20zNXFMK25mdjBmb29WTDFRMjFMZ3N3OEVtVXJ0blE0dk5sZ3grbjl4cGxvdzhaRzJBVDQ5RURBOFZ0SjU2ei81VTN2V3JBNXg5dU5IanVEZEJJMFlBdlMxZkVOUmk0ZXFzRHY4RlErdVB6THpKWGF0UVdkUDZDZVNpODlDR29tUkJXWi9DQXFWOU1LRjdWUlViVmhjNm9VNS9CRUk1a1pBRkxUUEJBWkFJNUNSTVorSndrNmVPcUVFa0crZ3dUKzVFaHBXMXpJYlR1c2ZUaUtPaFFHN0pwcVQrNVNsbTFsalY5Qks0MGJVWUdMZTFFMWliTk9IZFltdjJSRVhRPT0iLCJtYWMiOiIyNzE2NTVmZmM5MjU0ZmI5ZTQ5NWMzOTBmYmI4ZTQ2MGExZDhiOWQ0ZTkxNTljY2VhNjJjOGQ0YTVhYzE4ZGYwIn0=",
		"expires_time": "2022-05-26 14:15:59",
		"expires_unixtime": 1653545759,
		"terminal_sid": "VT-POS-2020-00002"
	}
	*/
	public class oauthResult
	{
		public string status { get; set; }
		public string message { get; set; }
		public string data { get; set; }
		public string access_token { get; set; }
		public string expires_time { get; set; }
		public int expires_unixtime { get; set; }
		public string terminal_sid { get; set; }
	}
}
