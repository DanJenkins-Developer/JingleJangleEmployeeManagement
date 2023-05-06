using System;
using System.Collections.Generic;

namespace JingleJangle.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string JobTitle { get; set; } = null!;

    public decimal MonthlySalary { get; set; }

    public virtual Person EmployeeNavigation { get; set; } = null!;
}
