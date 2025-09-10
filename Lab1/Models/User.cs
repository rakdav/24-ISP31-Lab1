using System;
using System.Collections.Generic;

namespace Lab1.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string Name { get; set; } = null!;

    public int IdCompany { get; set; }

    public virtual Company IdCompanyNavigation { get; set; } = null!;
}
