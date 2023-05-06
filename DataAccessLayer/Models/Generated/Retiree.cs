using System;
using System.Collections.Generic;

namespace JingleJangle.Models;

public partial class Retiree
{
    public int EmployeeId { get; set; }

    public string RetirementProgram { get; set; } = null!;

    public DateTime RetirementDate { get; set; }

    public virtual Person Employee { get; set; } = null!;
}
