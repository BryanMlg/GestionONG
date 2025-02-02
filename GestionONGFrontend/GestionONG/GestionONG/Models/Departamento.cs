﻿using System;
using System.Collections.Generic;

namespace GestionONG.Models;

public partial class Departamento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Municipio> Municipios { get; set; } = new List<Municipio>();

    public virtual ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
}
