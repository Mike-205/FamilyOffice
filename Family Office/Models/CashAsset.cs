using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.Models
{
    public class CashAsset
    {
        public int CashAssetID { get; set; }
        public string Location { get; set; }
        public string InCareOf { get; set; }
        public string Currency { get; set; } 
        public double Amount { get; set; }
    }
}
