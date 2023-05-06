using System;
using System.Collections.Generic;

namespace JingleJangle.Models;

public partial class PersonType
{
    public int PersonTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
