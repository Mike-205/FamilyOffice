using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.Models
{
    public class CurrencyExchangeRate
    {
        [Key]
        public int CurrencyExchangeRateID { get; set; }

        [Required]
        public string FromCurrencyName { get; set; }

        [Required]
        public string ToCurrencyName { get; set; }

        [Required]
        public decimal ExchangeRate { get; set; }

        // Navigation properties
        [ForeignKey("FromCurrencyName")]
        public virtual Currency FromCurrency { get; set; }

        [ForeignKey("ToCurrencyName")]
        public virtual Currency ToCurrency { get; set; }
    }
}
