using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    internal class Logger
    {
        /// <summary>
        /// Создание лога
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="log"></param>
        public static void CreateLog(string fileName, string log, TypeLog typeLog = TypeLog.Start)
        {
            File.AppendAllText(fileName, string.Format("{0} - {1}\n", typeLog, log));
        }

        public enum TypeLog
        {
            Start,
            Close,
            Error
        }
    }
}
