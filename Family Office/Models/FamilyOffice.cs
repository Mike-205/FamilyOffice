using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.Models
{
    public class FamilyOffice
    {
        public int FamilyOfficeID { get; set; }
        public string FamilyOfficeName { get; set; }
        public byte[] FamilyOfficeLogo { get; set; }
        public string EstablishmentDate { get; set; }
        public string Vision { get; set; }
        public string HeadOfFamily { get; set; }
        public string ChiefInvestOfficer { get; set; }
        public string Motto { get; set; }
        public string Purpose { get; set; }
    }
}
