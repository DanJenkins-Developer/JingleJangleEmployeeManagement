using System;
using System.Collections.Generic;

namespace JingleJangle.Models;

public partial class Prehire
{
    public int EmployeeId { get; set; }

    public DateTime OfferExtendedDate { get; set; }

    public DateTime OfferAcceptanceDate { get; set; }

    public virtual Person Employee { get; set; } = null!;
}
