using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab1.Models;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdUser { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public int IdCompany { get; set; }
    public Company? Company { get; set; }
    
}
