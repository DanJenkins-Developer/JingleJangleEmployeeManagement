using JingleJangle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Update
{
    internal class PersonInfo
    {
        public int EmployeeId { get; set; }
        // Other properties

        public int? PersonTypeId { get; set; }
        public PersonType PersonType { get; set; }

        // Navigation properties
        public Employee Employee { get; set; }
        public Prehire Prehire { get; set; }
        public Retiree Retiree { get; set; }
    }
}
