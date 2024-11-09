using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.Models
{
    public class BankAccount
    {
        public int AccountID { get; set; }
        public string BankName { get; set; }
        public string Country { get; set; }
        public string AccountHolder { get; set; }
        public string SwiftCode { get; set; }
        public string AccountType { get; set; }
        public string BranchLocation { get; set; }
        public int AccountNumber { get; set; }
        public string Nominee { get; set; }
        public double OpeningBalance { get; set; }
    }
}
