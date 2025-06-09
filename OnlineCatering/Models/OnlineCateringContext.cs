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

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingMenuItem> BookingMenuItems { get; set; }

    public virtual DbSet<CatererLogin> CatererLogins { get; set; }

    public virtual DbSet<FavouriteCaterer> FavouriteCaterers { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuCategory> MenuCategories { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<RawMaterial> RawMaterials { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierOrder> SupplierOrders { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    public virtual DbSet<WorkerSalary> WorkerSalaries { get; set; }

    public virtual DbSet<WorkerType> WorkerTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=DESKTOP-UD7E3S3\\SQLEXPRESS;initial catalog=OnlineCatering;user id=sa;password=hamza123456; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__73951AEDF86F8D0D");

            entity.ToTable("Booking");

            entity.Property(e => e.BillAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BookingStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Venue).HasMaxLength(255);

            entity.HasOne(d => d.Caterer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CatererId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOOKINGFROMCUSTOMERTOCATERER");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOOKINGFROMCUSTOMER");
        });

        modelBuilder.Entity<BookingMenuItem>(entity =>
        {
            entity.HasKey(e => e.BookingItemId).HasName("PK__BookingM__0CBA44C045791F4F");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingMenuItems)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookingMe__Booki__32AB8735");

            entity.HasOne(d => d.MenuItemNoNavigation).WithMany(p => p.BookingMenuItems)
                .HasForeignKey(d => d.MenuItemNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookingMe__MenuI__339FAB6E");
        });

        modelBuilder.Entity<CatererLogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CatererL__3214EC07BA06DF11");

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

        modelBuilder.Entity<FavouriteCaterer>(entity =>
        {
            entity.HasKey(e => e.FavouriteId).HasName("PK__Favourit__5944B59A0282C753");

            entity.ToTable("FavouriteCaterer");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Caterer).WithMany(p => p.FavouriteCaterers)
                .HasForeignKey(d => d.CatererId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FAVLIST_CATERERID");

            entity.HasOne(d => d.Customer).WithMany(p => p.FavouriteCaterers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FAVLIST_CUSTOMERID");
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

            entity.HasOne(d => d.CatererLogin).WithMany(p => p.Menus)
                .HasForeignKey(d => d.CatererLoginId)
                .HasConstraintName("FK_Menu_CatererLogin");
        });

        modelBuilder.Entity<MenuCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__MenuCate__19093A0B8448FF72");

            entity.ToTable("MenuCategory");

            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.CatererLogin).WithMany(p => p.MenuCategories)
                .HasForeignKey(d => d.CatererLoginId)
                .HasConstraintName("FK_MenuCategory_CatererLogin");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Message__3214EC0740E339FA");

            entity.ToTable("Message");

            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.FromCaterer).WithMany(p => p.MessageFromCaterers)
                .HasForeignKey(d => d.FromCatererId)
                .HasConstraintName("FK_Message_FromCaterer");

            entity.HasOne(d => d.FromCustomer).WithMany(p => p.MessageFromCustomers)
                .HasForeignKey(d => d.FromCustomerId)
                .HasConstraintName("FK_Message_FromCustomer");

            entity.HasOne(d => d.ToCaterer).WithMany(p => p.MessageToCaterers)
                .HasForeignKey(d => d.ToCatererId)
                .HasConstraintName("FK_Message_ToCaterer");

            entity.HasOne(d => d.ToCustomer).WithMany(p => p.MessageToCustomers)
                .HasForeignKey(d => d.ToCustomerId)
                .HasConstraintName("FK_Message_ToCustomer");
        });

        modelBuilder.Entity<RawMaterial>(entity =>
        {
            entity.HasKey(e => e.IngredientNo).HasName("PK__tmp_ms_x__BEAED9851D730242");

            entity.ToTable("RawMaterial");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Caterer).WithMany(p => p.RawMaterials)
                .HasForeignKey(d => d.CatererId)
                .HasConstraintName("FK_RawMaterial_ToTable");
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
        });

        modelBuilder.Entity<SupplierOrder>(entity =>
        {
            entity.HasKey(e => e.SuppOrderNo).HasName("PK__tmp_ms_x__41B58383064F53D5");

            entity.ToTable("SupplierOrder");

            entity.Property(e => e.EstimatedAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.InvoicePicture).HasMaxLength(255);

            entity.HasOne(d => d.Caterer).WithMany(p => p.SupplierOrders)
                .HasForeignKey(d => d.CatererId)
                .HasConstraintName("FK__SupplierO__Cater__1DB06A4F");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierOrders)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK__SupplierO__Suppl__1EA48E88");
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
