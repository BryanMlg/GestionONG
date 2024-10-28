using System;
using System.Collections.Generic;

namespace GestionONG.Models;

public partial class Municipio
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdDepartamento { get; set; }

    public virtual Departamento? IdDepartamentoNavigation { get; set; }

    public virtual ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
}
