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

    public virtual DbSet<CatererLogin> CatererLogins { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

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
        modelBuilder.Entity<CatererLogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CatererL__3214EC074CDD9BA9");

            entity.ToTable("CatererLogin");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.RegDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Restaurant).HasMaxLength(100);
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Login__3214EC0738A733C0");

            entity.ToTable("Login");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.UserAddress).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserPassword).HasMaxLength(255);
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MenuItemNo).HasName("PK__tmp_ms_x__8943AFF061284C6D");

            entity.ToTable("Menu");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.ItemName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Menus)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Menu__CategoryId__5BE2A6F2");
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
            entity.HasKey(e => e.IngredientNo).HasName("PK__tmp_ms_x__BEAED985D3C80BE9");

            entity.ToTable("RawMaterial");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__tmp_ms_x__4BE666B4D312012F");

            entity.ToTable("Supplier");

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Pincode)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Caterer).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.CatererId)
                .HasConstraintName("FK_Supplier_CatererLogin");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.WorkerId).HasName("PK__tmp_ms_x__077C88269536219D");

            entity.ToTable("Worker");

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
                .HasConstraintName("FK__Worker__CatererI__59063A47");

            entity.HasOne(d => d.WorkerType).WithMany(p => p.Workers)
                .HasForeignKey(d => d.WorkerTypeId)
                .HasConstraintName("FK__Worker__WorkerTy__5812160E");
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
                .HasConstraintName("FK__WorkerSal__Worke__571DF1D5");
        });

        modelBuilder.Entity<WorkerType>(entity =>
        {
            entity.HasKey(e => e.WorkerTypeId).HasName("PK__tmp_ms_x__691BC549959BB90D");

            entity.ToTable("WorkerType");

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
