using System;
using System.Collections.Generic;

namespace GestionONG.Models;

public partial class Donacion
{
    public int Id { get; set; }

    public int IdProyecto { get; set; }

    public int IdRubro { get; set; }

    public DateOnly FechaDonacion { get; set; }

    public string NombreDonante { get; set; } = null!;

    public decimal Monto { get; set; }

    public virtual Proyecto IdProyectoNavigation { get; set; } = null!;

    public virtual Rubro IdRubroNavigation { get; set; } = null!;
}
