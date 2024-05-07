using Alquileres.Domain.Common;
using Alquileres.Domain.Entities;
using Alquileres.Domain.Enums;
using Alquileres.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Alquileres.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public AppDbContext()
    {
    }

    public virtual DbSet<AspNetLanguages> AspNetLanguages { get; set; }
    public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
    public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
    public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
    public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
    public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
    public virtual DbSet<Audits> Audits { get; set; }
    public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
    public virtual DbSet<Sequences> Sequences { get; set; }

    public virtual DbSet<Alquiler> Alquileres { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<LineasAlquiler> LineasAlquilers { get; set; }

    public virtual DbSet<LineasGenero> LineasGeneros { get; set; }

    public virtual DbSet<Plataforma> Plataformas { get; set; }

    public virtual DbSet<PrecioVideoJuego> PrecioVideoJuegos { get; set; }

    public virtual DbSet<VideoJuego> VideoJuegos { get; set; }
    public virtual DbSet<FormaPago> FormaPagos { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies(false);
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information, DbContextLoggerOptions.UtcTime);

        // Esto se utiliza en las pruebas unitarias
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-MOQ5BKM;Database=grc_dev;Trusted_Connection=True;Encrypt=False");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alquiler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_VideoAlquiler");

            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.Code)
               .IsRequired()
               .HasMaxLength(15);


            entity.HasOne(d => d.ClienteNavigation).WithMany(p => p.Alquileres)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VideoAlquiler_VideoCliente");

            entity.HasOne(d => d.FormaPagoNavigation).WithMany(p => p.Alquileres)
              .HasForeignKey(d => d.IdFormaPago)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_Alquileres_FormaPago");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_videoCliente");

            entity.Property(e => e.Apellidos)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(15);
            entity.Property(e => e.Comentario).HasMaxLength(200);
            entity.Property(e => e.Dni)
                .IsRequired()
                .HasMaxLength(9);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(15);
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_VideoGenero");

            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<LineasAlquiler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_VideoLineaVideoJuego");

            entity.ToTable("LineasAlquiler");

            entity.Property(e => e.Comentarios).HasMaxLength(256);

            entity.HasOne(d => d.AlquilerNavigation).WithMany(p => p.LineasAlquileres)
                .HasForeignKey(d => d.IdAlquiler)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LineasAlquiler_Alquileres");

            entity.HasOne(d => d.PrecioPlataformasNavigation).WithMany(p => p.LineasAlquileres)
                .HasForeignKey(d => d.IdPrecioVideojuego)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LineasAlquiler_PrecioPlataformas");

        });

        modelBuilder.Entity<LineasGenero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_VideoLineaGenero");

            entity.ToTable("LineasGenero");

            entity.HasOne(d => d.GeneroNavigation).WithMany(p => p.LineasGeneros)
                .HasForeignKey(d => d.IdGenero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VideoLineaGenero_VideoGenero");

            entity.HasOne(d => d.VideojuegoNavigation).WithMany(p => p.LineasGeneros)
                .HasForeignKey(d => d.IdVideojuego)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VideoLineaGenero_VideoJuego");
        });

        modelBuilder.Entity<Plataforma>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_VideoPlataforma");

            entity.Property(e => e.Company).HasMaxLength(50);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Version).HasMaxLength(30);
        });

        modelBuilder.Entity<PrecioVideoJuego>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_VideoPrecioPlataforma");

            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.PlataformaNavigation).WithMany(p => p.PrecioVideoJuegos)
                .HasForeignKey(d => d.IdPlataforma)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrecioPlataformas_Plataformas");

            entity.HasOne(d => d.VideoJuegoNavigation).WithMany(p => p.PrecioVideoJuegos)
                .HasForeignKey(d => d.IdVideoJuego)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrecioPlataformas_VideoJuegos");
        });

        modelBuilder.Entity<VideoJuego>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_VideoJuego");

            entity.Property(e => e.Desarrollador).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(256);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Volumen).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<FormaPago>(entity =>
        {
            entity.ToTable("FormaPago");

            entity.Property(e => e.Descripcion).HasMaxLength(256);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AuditEntityData();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AuditEntityData()
    {
        ChangeTracker.DetectChanges();

        var auditEntries = new List<AuditEntry>();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Audits || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged) continue;

            var auditEntry = new AuditEntry(entry)
            {
                TableName = entry.Entity.GetType().Name,
            };

            auditEntries.Add(auditEntry);

            foreach (var property in entry.Properties)
            {
                string propertyName = property.Metadata.Name;

                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = AuditTypeEnum.Create;
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditTypeEnum.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditType = AuditTypeEnum.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
        }

        foreach (var auditEntry in auditEntries)
        {
            Audits.Add(auditEntry.ToAudit());
        }

        var entries = ChangeTracker.Entries()
                    .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
    }
}