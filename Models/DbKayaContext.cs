using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KullaniciTarafi.Models;

public partial class DbKayaContext : DbContext
{
    public DbKayaContext()
    {
    }

    public DbKayaContext(DbContextOptions<DbKayaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kullanicilar> Kullanicilars { get; set; }

    public virtual DbSet<Referanslar> Referanslars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\Buse;Initial Catalog=DbKaya");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kullanicilar>(entity =>
        {
            entity.ToTable("Kullanicilar");

            entity.Property(e => e.Ad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Soyad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefon).HasMaxLength(11);
        });

        modelBuilder.Entity<Referanslar>(entity =>
        {
            entity.ToTable("Referanslar");

            entity.Property(e => e.FirmaAdi).HasMaxLength(100);
            entity.Property(e => e.Hakkimizda).IsUnicode(false);
            entity.Property(e => e.Misyon).IsUnicode(false);
            entity.Property(e => e.Vizyon).IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
