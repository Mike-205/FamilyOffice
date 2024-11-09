using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.Models
{
    public class Document
    {
        public int DocumentID { get; set; }
        public string DocumentType { get; set; }
        public byte[] DocumentData { get; set; } 
    }
}
