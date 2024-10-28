using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GestionONG.Models;

public partial class GestionOngContext : DbContext
{
    public GestionOngContext()
    {
    }

    public GestionOngContext(DbContextOptions<GestionOngContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<DetalleOrdenCompra> DetalleOrdenCompras { get; set; }

    public virtual DbSet<Donacion> Donacions { get; set; }

    public virtual DbSet<Municipio> Municipios { get; set; }

    public virtual DbSet<OrdenCompra> OrdenCompras { get; set; }

    public virtual DbSet<Proyecto> Proyectos { get; set; }

    public virtual DbSet<ProyectoRubro> ProyectoRubros { get; set; }

    public virtual DbSet<Rubro> Rubros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departam__3213E83F7A710E5F");

            entity.ToTable("Departamento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DetalleOrdenCompra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DetalleO__3213E83F0604EC98");

            entity.ToTable("DetalleOrdenCompra");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdOrdenCompra).HasColumnName("idOrdenCompra");
            entity.Property(e => e.IdRubro).HasColumnName("idRubro");
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreProducto");

            entity.HasOne(d => d.IdOrdenCompraNavigation).WithMany(p => p.DetalleOrdenCompras)
                .HasForeignKey(d => d.IdOrdenCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleOrdenCompra_OrdenCompra");

            entity.HasOne(d => d.IdRubroNavigation).WithMany(p => p.DetalleOrdenCompras)
                .HasForeignKey(d => d.IdRubro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleOrdenCompra_Rubro");
        });

        modelBuilder.Entity<Donacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Donacion__3213E83F26E0233E");

            entity.ToTable("Donacion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProyecto).HasColumnName("idProyecto");
            entity.Property(e => e.IdRubro).HasColumnName("idRubro");
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NombreDonante)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdProyectoNavigation).WithMany(p => p.Donacions)
                .HasForeignKey(d => d.IdProyecto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donacion_Proyecto");

            entity.HasOne(d => d.IdRubroNavigation).WithMany(p => p.Donacions)
                .HasForeignKey(d => d.IdRubro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donacion_Rubro");
        });

        modelBuilder.Entity<Municipio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Municipi__3213E83FCA69D78E");

            entity.ToTable("Municipio");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Municipios)
                .HasForeignKey(d => d.IdDepartamento)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Municipio__idDep__398D8EEE");
        });

        modelBuilder.Entity<OrdenCompra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrdenCom__3213E83F1FE931C4");

            entity.ToTable("OrdenCompra");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProyecto).HasColumnName("idProyecto");
            entity.Property(e => e.MontoTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Proveedor)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdProyectoNavigation).WithMany(p => p.OrdenCompras)
                .HasForeignKey(d => d.IdProyecto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenCompra_Proyecto");
        });

        modelBuilder.Entity<Proyecto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Proyecto__3213E83FDCD3A05A");

            entity.ToTable("Proyecto");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codigo)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");
            entity.Property(e => e.IdMunicipio).HasColumnName("idMunicipio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Proyectos)
                .HasForeignKey(d => d.IdDepartamento)
                .HasConstraintName("FK__Proyecto__idDepa__52593CB8");

            entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.Proyectos)
                .HasForeignKey(d => d.IdMunicipio)
                .HasConstraintName("FK__Proyecto__idMuni__5165187F");
        });

        modelBuilder.Entity<ProyectoRubro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Proyecto__3213E83F869B2371");

            entity.ToTable("ProyectoRubro");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProyecto).HasColumnName("idProyecto");
            entity.Property(e => e.IdRubro).HasColumnName("idRubro");
            entity.Property(e => e.Presupuesto).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdProyectoNavigation).WithMany(p => p.ProyectoRubros)
                .HasForeignKey(d => d.IdProyecto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProyectoR__idPro__71D1E811");

            entity.HasOne(d => d.IdRubroNavigation).WithMany(p => p.ProyectoRubros)
                .HasForeignKey(d => d.IdRubro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProyectoR__idRub__72C60C4A");
        });

        modelBuilder.Entity<Rubro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rubro__3213E83F9A5FEB11");

            entity.ToTable("Rubro");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
