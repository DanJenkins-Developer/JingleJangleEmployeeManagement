using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.List
{
    public class EmployeeResult
    {
        public int EmployeeId { get; set; }
        public string JobTitle { get; set; }
        public decimal MonthlySalary { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime EmploymentDate { get; set; }
    }
}
