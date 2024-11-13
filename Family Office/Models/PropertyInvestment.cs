
namespace Family_Office.Models
{
    public class PropertyInvestment
    {
        public PropertyInvestment()
        {
            // Initialize PurchaseDate to current date by default
            PurchaseDate = DateTime.Today;
        }

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
        public string UnitOfMeasurement { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
