using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uno.Entities
{
    public class Log
    {
        public int LogID { get; set; }
        public int EventID { get; set; }
        public string LogType { get; set; }
        public string Message { get; set; }
        public string Category { get; set; }
        public DateTime EventTime { get; set; }
    }
}
