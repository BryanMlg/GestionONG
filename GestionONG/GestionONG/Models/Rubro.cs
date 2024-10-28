using System;
using System.Collections.Generic;

namespace GestionONG.Models;

public partial class Rubro
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<DetalleOrdenCompra> DetalleOrdenCompras { get; set; } = new List<DetalleOrdenCompra>();

    public virtual ICollection<Donacion> Donacions { get; set; } = new List<Donacion>();

    public virtual ICollection<ProyectoRubro> ProyectoRubros { get; set; } = new List<ProyectoRubro>();
}
