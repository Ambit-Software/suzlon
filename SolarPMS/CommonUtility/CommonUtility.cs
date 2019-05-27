using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public class CommonUtility
    {
        public static void TraceToLog(string StrPriValue)
        {
            FileStream ObjPriFSErrorLog = null;
            StreamWriter ObjPriSWErrorLog = null;
            try
            {
                String StrPriPath = ConfigurationManager.AppSettings["TraceLogFilePath"];
                String StrPriErrorLog = StrPriPath + "\\LOG";
                String StrPriErrorLogFile = "";

                if (!Directory.Exists(StrPriErrorLog))
                    Directory.CreateDirectory(StrPriErrorLog);

                StrPriErrorLogFile = StrPriErrorLog + "\\eTouches_API" + DateTime.Now.ToString("ddMMyyyy").Trim() + ".log";
                if (File.Exists(StrPriErrorLogFile))
                {
                    ObjPriFSErrorLog = new FileStream(StrPriErrorLogFile, FileMode.Append, FileAccess.Write);
                }
                else
                {
                    ObjPriFSErrorLog = new FileStream(StrPriErrorLogFile, FileMode.Create);
                }
                ObjPriSWErrorLog = new StreamWriter(ObjPriFSErrorLog);
                ObjPriSWErrorLog.WriteLine("Date Time : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss").Trim() + "\n");
                ObjPriSWErrorLog.WriteLine(StrPriValue + "\n");
                ObjPriSWErrorLog.WriteLine("-----------------------------------------------------------------------" + "\n");
                ObjPriSWErrorLog.Flush();
                if (ObjPriFSErrorLog != null)
                {
                    ObjPriFSErrorLog.Close();
                    ObjPriFSErrorLog = null;
                }
                if (ObjPriSWErrorLog != null)
                {
                    ObjPriSWErrorLog = null;
                }
            }
            catch (Exception ex)
            {
                ex = null;
            }
            finally
            {
                if (ObjPriFSErrorLog != null)
                {
                    ObjPriFSErrorLog.Close();
                    ObjPriFSErrorLog = null;
                }
                if (ObjPriSWErrorLog != null)
                {
                    ObjPriSWErrorLog = null;
                }
            }
        }
    }
}
