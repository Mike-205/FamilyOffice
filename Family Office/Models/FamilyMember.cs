using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.Models
{
    public class FamilyMember
    {
        public int FamilyMemberID { get; set; }
        public string CustomMemberID { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public string ProfilePicture { get; set; }
        public string DOB { get; set; }
        public string FamilyRelation { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string JoiningFamilyDate { get; set; }
        public string CountryCode1 { get; set; }
        public string Telephone1 { get; set; }
        public string CountryCode2 { get; set; }
        public string Telephone2 { get; set; }
        public string PersonalEmail { get; set; }
        public string FamilyEmail { get; set; }
        public string HomeAddress { get; set; }
        public string Nationality { get; set; }
        public string CurrentResidence { get; set; }
        public int PassPortNo { get; set; }
        public string PassportExpiryDate { get; set; }
        public int EmiratesID { get; set; }
        public string EmiratesIDExpiryDate { get; set; }
        public int AadharNo { get; set; }
        public int PanCardNumber { get; set; }
        public string EducationStatus { get; set; }
        public string MainSkills { get; set; }
        public string SecondarySkills { get; set; }
        public string EducatedFrom { get; set; }
        public byte[] Document { get; set; }
    }
}
