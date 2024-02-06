using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CSI.IBTA.DataLayer.Models;

public partial class CsiTrainingHealthcare2024Context : DbContext
{
    public CsiTrainingHealthcare2024Context()
    {
    }

    public CsiTrainingHealthcare2024Context(DbContextOptions<CsiTrainingHealthcare2024Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Test> Tests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ACER-LAPTOP\\MSSQLSERVER02;Database=csi-training-healthcare-2024;Trusted_Connection=True;Encrypt=False;");

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
