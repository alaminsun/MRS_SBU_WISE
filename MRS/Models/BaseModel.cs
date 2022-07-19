using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class BaseModel
    {
        public int SUStatus { get; set; }
        public string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
    }
}