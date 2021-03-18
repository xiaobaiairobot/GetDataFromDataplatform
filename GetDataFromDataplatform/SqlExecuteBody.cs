using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataFromDataplatform
{
    public class SqlExecuteBody
    {
        public string sql { get; set; }
        public long timeout { get; set; }
        public string engine { get; set; }
        public bool isIncludeHeaders { get; set; }
    }
}
