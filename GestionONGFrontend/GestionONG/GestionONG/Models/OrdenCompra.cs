using System;
using System.Collections.Generic;

namespace GestionONG.Models;

public partial class OrdenCompra
{
    public int Id { get; set; }

    public int IdProyecto { get; set; }

    public string Proveedor { get; set; } = null!;

    public DateOnly FechaOrden { get; set; }

    public decimal MontoTotal { get; set; }

    public virtual ICollection<DetalleOrdenCompra> DetalleOrdenCompras { get; set; } = new List<DetalleOrdenCompra>();

    public virtual Proyecto IdProyectoNavigation { get; set; } = null!;
}
