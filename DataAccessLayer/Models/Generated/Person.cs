using System;
using System.Collections.Generic;

namespace JingleJangle.Models;

public partial class Person
{
    public int EmployeeId { get; set; }

    public int PersonTypeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime EmploymentDate { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual PersonType PersonType { get; set; } = null!;

    public virtual Prehire? Prehire { get; set; }

    public virtual Retiree? Retiree { get; set; }
}
