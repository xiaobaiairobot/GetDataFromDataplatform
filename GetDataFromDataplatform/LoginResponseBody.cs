using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataFromDataplatform
{
    public class LoginResponseBody
    {
        public int code { get; set; }
        public string error { get; set; }
        public string token { get; set; }
    }
}
