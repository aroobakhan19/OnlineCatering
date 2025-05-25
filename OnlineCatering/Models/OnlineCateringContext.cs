using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnlineCatering.Models;

public partial class OnlineCateringContext : DbContext
{
    public OnlineCateringContext()
    {
    }

    public OnlineCateringContext(DbContextOptions<OnlineCateringContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Caterer> Caterers { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuCategory> MenuCategories { get; set; }

    public virtual DbSet<RawMaterial> RawMaterials { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    public virtual DbSet<WorkerSalary> WorkerSalaries { get; set; }

    public virtual DbSet<WorkerType> WorkerTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=DESKTOP-UD7E3S3\\SQLEXPRESS;initial catalog=OnlineCatering;user id=sa;password=hamza123456; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Caterer>(entity =>
        {
            entity.HasKey(e => e.CatererId).HasName("PK__Caterer__41AC128076326CC3");

            entity.ToTable("Caterer");

            entity.Property(e => e.CatererId).ValueGeneratedNever();
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PinCode)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MenuItemNo).HasName("PK__tmp_ms_x__8943AFF08CB38F19");

            entity.ToTable("Menu");

            entity.Property(e => e.MenuItemNo).ValueGeneratedNever();
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.ItemName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Menus)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Menu__CategoryId__4AB81AF0");
        });

        modelBuilder.Entity<MenuCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__MenuCate__19093A0B8448FF72");

            entity.ToTable("MenuCategory");

            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RawMaterial>(entity =>
        {
            entity.HasKey(e => e.IngredientNo).HasName("PK__RawMater__BEAED985F0674354");

            entity.ToTable("RawMaterial");

            entity.Property(e => e.IngredientNo).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B4A8F77E1E");

            entity.ToTable("Supplier");

            entity.Property(e => e.SupplierId).ValueGeneratedNever();
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Pincode)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.WorkerId).HasName("PK__Worker__077C8826C3E7F892");

            entity.ToTable("Worker");

            entity.Property(e => e.WorkerId).ValueGeneratedNever();
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Caterer).WithMany(p => p.Workers)
                .HasForeignKey(d => d.CatererId)
                .HasConstraintName("FK__Worker__CatererI__3C69FB99");

            entity.HasOne(d => d.WorkerType).WithMany(p => p.Workers)
                .HasForeignKey(d => d.WorkerTypeId)
                .HasConstraintName("FK__Worker__WorkerTy__3B75D760");
        });

        modelBuilder.Entity<WorkerSalary>(entity =>
        {
            entity.HasKey(e => e.SalaryId).HasName("PK__WorkerSa__4BE20457A3DC1563");

            entity.ToTable("WorkerSalary");

            entity.Property(e => e.SalaryId).ValueGeneratedNever();
            entity.Property(e => e.PayMonth)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PayYear)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TotalPay).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Worker).WithMany(p => p.WorkerSalaries)
                .HasForeignKey(d => d.WorkerId)
                .HasConstraintName("FK__WorkerSal__Worke__3F466844");
        });

        modelBuilder.Entity<WorkerType>(entity =>
        {
            entity.HasKey(e => e.WorkerTypeId).HasName("PK__WorkerTy__691BC5492880FCD7");

            entity.ToTable("WorkerType");

            entity.Property(e => e.WorkerTypeId).ValueGeneratedNever();
            entity.Property(e => e.PayPerDay).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.WorkerType1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("WorkerType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
