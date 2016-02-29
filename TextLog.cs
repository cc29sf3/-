using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
   
    public static class TextLog
    {
        private static object lockObject = new object();
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="msgs">日志内容</param>
        public static void Add(params object[] msgs)
        {
            string savePath = AppDomain.CurrentDomain.BaseDirectory + "\\log\\";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            string fn = savePath + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            if (msgs == null || msgs.Length == 0)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < msgs.Length; i++)
            {
                sb.AppendFormat("{{{0}}}\r\n", i);
            }
            sb.Append("\r\n");

            var time = DateTime.Now;
            string messageFormat = string.Empty;
            messageFormat += "[" + time.Hour + ":" + time.Minute + ":" + time.Second + ":" + time.Millisecond + "]";
            messageFormat += sb.ToString();
            lock (lockObject)
            {
                File.AppendAllText(fn, string.Format(messageFormat, msgs));
            }
        }
    }
}