using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class SelectedGenericModel : BaseModel
    {
        public string GENERIC_CODE { get; set; }

        public string GENERIC_NAME { get; set; }

        public string SLNO { get; set; }
        public List<SelectedGenericModel> SelectedGenericList { get; set; }
    }
}