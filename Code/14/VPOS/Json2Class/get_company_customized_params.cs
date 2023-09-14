﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class GCCPDatum
    {
        public int company_sid { get; set; }
        public string customized_code { get; set; }
        public string customized_name { get; set; }
        public string active_state { get; set; }
        public string @params { get; set; }
        public string created_time { get; set; }
        public int created_unix_time { get; set; }
        public string updated_time { get; set; }
        public int updated_unix_time { get; set; }
    }

    public class get_company_customized_params
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<GCCPDatum> data { get; set; }
    }
}
