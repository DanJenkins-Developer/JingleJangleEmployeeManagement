using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Update
{
    public class EmployeeInfo
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string TypeName { get; set; }
    }

    public class RegularEmployeeInfo : EmployeeInfo
    {
        public string JobTitle { get; set; }
        public decimal? MonthlySalary { get; set; }
    }

    public class PrehireInfo : EmployeeInfo
    {
        public DateTime? OfferAcceptanceDate { get; set; }
        public DateTime? OfferExtendedDate { get; set; }
    }

    public class RetireeInfo : EmployeeInfo
    {
        public DateTime? RetirementDate { get; set; }
        public string RetirementProgram { get; set; }
    }
}
