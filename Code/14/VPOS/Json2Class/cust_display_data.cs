using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPOS
{
    public class cust_display_content
    {
        //public int m_display_data_sid;
        public int m_item_no;
        public String m_content;
        public cust_display_content()
        {
            //m_display_data_sid = 0;
            m_item_no = 0;
            m_content = "";
        }
    }

    public class cust_display_data
    {
        public int m_SID;
        public String m_data_name;
        public String m_data_kind;
        public String m_source_type;
        public String m_stretch_size;
        public String m_play_type;
        public int m_play_speed_sec;
        public String m_del_flag;
        public String m_created_time;
        public String m_updated_time;
        public List<cust_display_content> m_display_contents;

        public cust_display_data()
        {
            m_SID = 0;// " int NOT NULL,
            m_data_name = "";// varchar(50),
            m_data_kind = "";//" char(2),
            m_source_type = "W";// V: Video I : Image T:文字資料 W:WebURL[UI沒有URL選項]
            m_stretch_size = "N";// N / Y 是否自動延展 Y:是
            m_play_type = "A";//A: Auto S : 依據設定變動的秒數
            m_play_speed_sec = 5;//sec
            m_del_flag = "N";// N / Y
            m_created_time = "";// timestamp,
            m_updated_time = "";// timestamp,
            m_display_contents = new List<cust_display_content> ();
        }
    }
}
