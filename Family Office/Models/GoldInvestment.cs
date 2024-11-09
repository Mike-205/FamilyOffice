using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.Models
{
    public class GoldInvestment
    {
        public int GoldInvestmentID { get; set; }
        public string GoldType { get; set; }
        public double TotalGrams { get; set; }
        public int Carat { get; set; }
        public double TotalPerGram { get; set; }
        public string StorageLocation { get; set; }
        public string Country { get; set; }
        public double AnnualStorageCost { get; set; }
        public double AnnualMaintenanceCost { get; set; }
        public byte[] Document { get; set; }
        public string PurchaseDate { get; set; }
        public string InCareOf { get; set; }
    }
}
