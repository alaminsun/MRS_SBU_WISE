using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Models
{
  public  class SCPGroupInfoModel
    {
        public int SDICG_ID { get; set; }
        public int SDIC_ID { get; set; }
      
        public string GROUP_NAME { get; set; }
       public string TERRITORY_NAME{ get; set; }
        public string TERRITORY_CODE{ get; set; }

        public string HBTERRITORY_CODE { get; set; }

        public string HBTERRITORY_NAME { get; set; }
    }
}
