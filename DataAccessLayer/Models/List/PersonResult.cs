using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.List
{
    public class PersonResult
    {
        public int EmployeeId { get; set; }

        public string TypeName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime EmploymentDate { get; set; }

    }
}
