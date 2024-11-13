using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.Models
{
    public class GoldInvestment
    {
        public GoldInvestment()
        {
            // Initialize PurchaseDate to current date by default
            PurchaseDate = DateTime.Today;
        }

        public int GoldInvestmentID { get; set; }

        /// <summary>
        /// Type or form of gold (e.g., bar, coin, jewelry)
        /// </summary>
        public string GoldType { get; set; }

        /// <summary>
        /// Person or entity in whose care the gold is held
        /// </summary>
        public string InCareOf { get; set; }

        /// <summary>
        /// Date when the gold was purchased
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Total weight of gold in grams
        /// </summary>
        public decimal TotalGrams { get; set; }

        /// <summary>
        /// Purity of gold measured in carats
        /// </summary>
        public int Carat { get; set; }

        /// <summary>
        /// Cost per gram in the specified currency
        /// </summary>
        public decimal TotalPerGram { get; set; }

        /// <summary>
        /// Currency used for the purchase and valuation
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Method or channel through which the gold was purchased
        /// </summary>
        public string PurchaseVia { get; set; }

        /// <summary>
        /// Physical location where the gold is stored
        /// </summary>
        public string StorageLocation { get; set; }

        /// <summary>
        /// Country where the gold is stored
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Annual cost for storing the gold
        /// </summary>
        public decimal AnnualStorageCost { get; set; }

        /// <summary>
        /// Associated documentation in binary format
        /// </summary>
        public byte[] Document { get; set; }

        /// <summary>
        /// Additional notes or remarks about the investment
        /// </summary>
        public string Notes { get; set; }

    }
}
