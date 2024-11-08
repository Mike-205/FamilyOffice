using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; } = "admin"; // Default value
        public string Password { get; set; } = "12345"; // Default value
        public string? Profile { get; set; }
        public DateTime? DOB { get; set; }
        public string? FamRelation { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
