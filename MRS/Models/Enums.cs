using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class Enums
    {
        public enum MessageType
        {
            None = 0,
            Error = 1,
            Success = 2,
            Update = 3,
            Delete = 4,
        }
    }
}