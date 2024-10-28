using System;
using System.Collections.Generic;

namespace GestionONG.Models;

public partial class Proyecto
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public int? IdMunicipio { get; set; }

    public int? IdDepartamento { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public virtual ICollection<Donacion> Donacions { get; set; } = new List<Donacion>();

    public virtual Departamento? IdDepartamentoNavigation { get; set; }

    public virtual Municipio? IdMunicipioNavigation { get; set; }

    public virtual ICollection<OrdenCompra> OrdenCompras { get; set; } = new List<OrdenCompra>();

    public virtual ICollection<ProyectoRubro> ProyectoRubros { get; set; } = new List<ProyectoRubro>();
}
