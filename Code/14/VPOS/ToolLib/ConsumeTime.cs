using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;//Stopwatch
namespace VPOS
{
    public class ConsumeTime
    {
        private static Stopwatch m_stopWatch = new Stopwatch();
        private static String m_StrTitle = "";
        private static String m_StrStartFileLine = "";
        private static String m_StrEndFileLine = "";
        public static void Start(String StrInfor)
        {
            StackFrame CallStack = new StackFrame(1, true);
            m_StrStartFileLine = String.Format("File : {0} , Line : {1}", CallStack.GetFileName(), CallStack.GetFileLineNumber());
            m_StrTitle = StrInfor;

            m_stopWatch.Start();
        }
        public static void Stop()
        {
            StackFrame CallStack = new StackFrame(1, true);

            m_stopWatch.Stop();

            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = m_stopWatch.Elapsed;
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",ts.Hours, ts.Minutes, ts.Seconds,ts.Milliseconds / 10);
            
            m_StrEndFileLine = String.Format("File : {0} , Line : {1}", CallStack.GetFileName(), CallStack.GetFileLineNumber());

            //MessageBox.Show(m_StrStartFileLine + " ~ " + m_StrEndFileLine + " consume time: " + elapsedTime, m_StrTitle);
        }

        public static DateTime GetStartTime(int processId=0)
        {
            Process processes = Process.GetProcessById(processId);
            // -----------------------------
            DateTime retVal = DateTime.Now;
            retVal = processes.StartTime;

            return retVal;
        }
        public static DateTime GetStartTime(string processName)
        {
            Process[] processes =
                Process.GetProcessesByName(processName);
            if (processes.Length == 0)
                throw new ApplicationException(string.Format(
                   "Process {0} is not running.", processName));
            // -----------------------------
            DateTime retVal = DateTime.Now;
            foreach (Process p in processes)
                if (p.StartTime < retVal)
                    retVal = p.StartTime;

            return retVal;
        }
    }
}
