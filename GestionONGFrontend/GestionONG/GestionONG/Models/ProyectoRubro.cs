using System;
using System.Collections.Generic;

namespace GestionONG.Models;

public partial class ProyectoRubro
{
    public int Id { get; set; }

    public int IdProyecto { get; set; }

    public int IdRubro { get; set; }

    public decimal Presupuesto { get; set; }

    public virtual Proyecto IdProyectoNavigation { get; set; } = null!;

    public virtual Rubro IdRubroNavigation { get; set; } = null!;
}
