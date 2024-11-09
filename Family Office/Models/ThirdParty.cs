using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.Models
{
    public class ThirdParty
    {
        public int ThirdPartyID { get; set; }
        public string PartyType { get; set; }
        public string PartyID { get; set; }
        public string FullName { get; set; }
        public string FirstAssociationDate { get; set; }
        public byte[] Photo { get; set; }
        public string MainNumberCode { get; set; }
        public string MainMobileNumber { get; set; }
        public string AltNumberCode { get; set; }
        public string AltMobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string HomeAddress { get; set; }
        public string Notes { get; set; }
        public byte[] Document { get; set; }
        public string CurrentStatus { get; set; }
        public int AadharCardNo { get; set; }
        public int PanCardNo { get; set; }
        public string RelationToUs { get; set; }
        public double OpeningBalance { get; set; }
        public string CurrencyType { get; set; }
    }
}
