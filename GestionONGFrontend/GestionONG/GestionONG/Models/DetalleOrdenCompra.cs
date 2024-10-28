using System;
using System.Collections.Generic;

namespace GestionONG.Models;

public partial class DetalleOrdenCompra
{
    public int Id { get; set; }

    public int IdOrdenCompra { get; set; }

    public int IdRubro { get; set; }

    public decimal Monto { get; set; }

    public string? NombreProducto { get; set; }

    public virtual OrdenCompra IdOrdenCompraNavigation { get; set; } = null!;

    public virtual Rubro IdRubroNavigation { get; set; } = null!;
}
