using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CSI.IBTA.DataLayer.Models;

public partial class CsiHealthcare2024Context : DbContext
{
    public CsiHealthcare2024Context()
    {
    }

    public CsiHealthcare2024Context(DbContextOptions<CsiHealthcare2024Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Test> Tests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=csi-healthcare-2024;Trusted_Connection=True;Encrypt=False;",
            b => b.MigrationsAssembly("CSI.IBTA.DB.Migrations"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Test>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Test");

            entity.Property(e => e.Test1).HasColumnName("test");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
