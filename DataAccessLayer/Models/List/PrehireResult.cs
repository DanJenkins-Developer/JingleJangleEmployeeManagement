using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.List
{
    public class PrehireResult
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime OfferExtendedDate { get; set; }
        public DateTime OfferAcceptanceDate { get; set; }
    }
}
