using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.Models
{
    public class PropertyInvestment
    {
        public int PropertyInvestmentID { get; set; }
        public string PropertyType { get; set; }
        public string Purpose { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Coordinates { get; set; }
        public int TotalArea { get; set; }
        public double TotalPurchasePrice { get; set; }
        public double PricePerUnit { get; set; }
        public double AnnualMaintenanceCost { get; set; }
        public double BrokerCost { get; set; }
        public string Ownership { get; set; }
        public byte[] Document { get; set; }
    }
}
