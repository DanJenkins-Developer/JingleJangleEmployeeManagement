using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.List
{
    public class RetireeResult
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RetirementProgram { get; set; }
        public DateTime RetirementDate { get; set; }
    }
}
