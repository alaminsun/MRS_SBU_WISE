using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class ValidationMsg
    {
        public List<string> LstErrors { get; set; }
        public string Msg { get; set; }
        public string Tag { get; set; }
        public string ReturnCode { get; set; }
        public long ReturnId { get; set; }
        public Enums.MessageType Type { get; set; }
    }
}