﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Alquileres.Domain.Common;
using System;
using System.Collections.Generic;

namespace Alquileres.Domain.Entities;

public partial class LineasAlquiler : BaseEntity
{
    public int IdAlquiler { get; set; }

    public int IdPrecioVideojuego { get; set; }

    public string Comentarios { get; set; }

    public int Cantidad { get; set; }

    public virtual Alquiler AlquilerNavigation { get; set; }

    public virtual PrecioVideoJuego PrecioPlataformasNavigation { get; set; }

}