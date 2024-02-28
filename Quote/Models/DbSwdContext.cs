using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Quote.Models;

public partial class DbSwdContext : DbContext
{
    public DbSwdContext()
    {
    }

    public DbSwdContext(DbContextOptions<DbSwdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartDetail> CartDetails { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Option> Options { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestDetail> RequestDetails { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            var connectionString = configuration.GetConnectionString("apicon");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasIndex(e => e.UserId, "UQ__Admins__1788CC4D35AA52DB").IsUnique();

            entity.Property(e => e.Image).HasMaxLength(50);

            entity.HasOne(d => d.User).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Admins_Users");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.HasIndex(e => e.CustomerId, "UQ__Cart__A4AE64D9BFCBAEF3").IsUnique();

            entity.Property(e => e.CartId).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithOne(p => p.Cart)
                .HasForeignKey<Cart>(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_Cart");
        });

        modelBuilder.Entity<CartDetail>(entity =>
        {
            entity.ToTable("CartDetail");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartDetails)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartDetail_Cart");

            entity.HasOne(d => d.Product).WithMany(p => p.CartDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartDetail_Product");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CateName).HasMaxLength(500);
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.ToTable("Contract");

            entity.HasIndex(e => e.RequestId, "UQ__Contract__33A8517B31C8143E").IsUnique();

            entity.Property(e => e.FinalPrice).HasColumnType("money");

            entity.HasOne(d => d.Request).WithOne(p => p.Contract)
                .HasForeignKey<Contract>(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contract_Request");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.HasIndex(e => e.UserId, "UQ__Customer__1788CC4DF314804D").IsUnique();

            entity.Property(e => e.Image).HasMaxLength(50);

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_Users");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Image");

            entity.Property(e => e.Image1)
                .HasMaxLength(50)
                .HasColumnName("Image");

            entity.HasOne(d => d.Product).WithMany(p => p.Images)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Image_Product");
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.ToTable("Option");

            entity.Property(e => e.OptionId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.OptionName).HasMaxLength(500);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Product).WithMany(p => p.Options)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Option_Product");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");

            entity.Property(e => e.DatePay).HasColumnType("date");
            entity.Property(e => e.PricePay).HasColumnType("money");

            entity.HasOne(d => d.Contract).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Contract");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductName).HasMaxLength(500);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.ToTable("Request");

            entity.Property(e => e.BankNumber).HasMaxLength(500);
            entity.Property(e => e.RequestName).HasMaxLength(500);
            entity.Property(e => e.TaxCode).HasColumnType("ntext");

            entity.HasOne(d => d.Customer).WithMany(p => p.Requests)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Request_Customer");
        });

        modelBuilder.Entity<RequestDetail>(entity =>
        {
            entity.ToTable("RequestDetail");

            entity.HasOne(d => d.Product).WithMany(p => p.RequestDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequestDetail_Product");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestDetails)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequestDetail_Request");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasIndex(e => e.UserId, "UQ__Staff__1788CC4D9568F826").IsUnique();

            entity.Property(e => e.Image).HasMaxLength(50);

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_Staff_Manager");

            entity.HasOne(d => d.User).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Staff_Users");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.ToTable("Task");

            entity.HasIndex(e => e.RequestId, "UQ__Task__33A8517B52585AA5").IsUnique();

            entity.Property(e => e.Location).HasMaxLength(500);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.TaskName).HasMaxLength(500);

            entity.HasOne(d => d.Request).WithOne(p => p.Task)
                .HasForeignKey<Task>(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_Request");

            entity.HasOne(d => d.Staff).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_Staff");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.TokeId).HasName("PK_tb_Token");

            entity.ToTable("Token");

            entity.Property(e => e.TokeId)
                .ValueGeneratedNever()
                .HasColumnName("TokeID");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.Token1)
                .HasColumnType("ntext")
                .HasColumnName("Token");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Token_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.UserId, "UQ__Users__1788CC4D62F95C3A").IsUnique();

            entity.Property(e => e.Cccd)
                .HasMaxLength(50)
                .HasColumnName("CCCD");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(250);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
