using System;
using System.Collections.Generic;
using GlamourHub.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GlamourHub.Models
{
    public partial class GlamourHubContext : DbContext
    {
        public GlamourHubContext()
        {
        }

        public GlamourHubContext(DbContextOptions<GlamourHubContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Cart> Cart { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Order> Order { get; set; } = null!;
        public virtual DbSet<order_items> order_items { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        public List<OrderDetail> GetOrderDetails()
        {
            // Use FromSqlRaw to execute the stored procedure and map the result to the OrderDetail model
            return this.Set<OrderDetail>().FromSqlRaw("EXEC sp_get_order_details").ToList();
        }

        public IEnumerable<string> GetProductNames()
        {
            return Products.Select(p => p.Name).ToList();
        }

        public IEnumerable<string> GetBrandNames()
        {
            return Brands.Select(b => b.Name).ToList();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-JOIT8OMU\\SQLEXPRESS;Database=GlamourHub;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Register the OrderDetail model class
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                // Specify the mapping if necessary
                entity.ToView("sp_get_order_details");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("addresses");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                // Shipping address properties
                entity.Property(e => e.FirstName).HasColumnName("shipping_first_name");
                entity.Property(e => e.LastName).HasColumnName("shipping_last_name");
                entity.Property(e => e.Street).HasColumnName("shipping_street");
                entity.Property(e => e.City).HasColumnName("shipping_city");
                entity.Property(e => e.State).HasColumnName("shipping_state");
                entity.Property(e => e.PostalCode).HasColumnName("shipping_postal_code");
                entity.Property(e => e.Country).HasColumnName("shipping_country");
                entity.Property(e => e.Phone).HasColumnName("shipping_phone");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__addresses__user___4222D4EF");
            });


            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brands");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("cart");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__cart__product_id__36B12243");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__cart__user_id__35BCFE0A");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("OrderDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("UserId");

                // Define the relationship between Order and OrderItem
                entity.HasMany(o => o.order_items)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade); // If an Order is deleted, its OrderItems will also be deleted

                // Optionally, you can add an index to the UserId column for better query performance
                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<order_items>(entity =>
            {
                entity.ToTable("order_items");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Price).HasColumnName("price");

                // Define the relationship between OrderItem and Order
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.order_items)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade); // If an OrderItem is deleted, the associated Order will not be affected

                // Define the relationship between OrderItem and Product
                entity.HasOne(oi => oi.Product)
                      .WithMany()
                      .HasForeignKey(oi => oi.ProductId);
            });
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TaxAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.DeliveryAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.GrandTotal)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<order_items>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>(entity =>
                {
                    entity.ToTable("payments");

                    entity.Property(e => e.Id).HasColumnName("id");

                    entity.Property(e => e.CardNumber)
                            .HasMaxLength(16)
                            .IsUnicode(false)
                            .HasColumnName("card_number");

                    entity.Property(e => e.CreatedAt)
                            .HasColumnType("datetime")
                            .HasColumnName("created_at")
                            .HasDefaultValueSql("(getdate())");

                    entity.Property(e => e.Cvv)
                            .HasMaxLength(4)
                            .IsUnicode(false)
                            .HasColumnName("cvv");

                    entity.Property(e => e.ExpirationDate)
                            .HasMaxLength(5)
                            .IsUnicode(false)
                            .HasColumnName("expiration_date");

                    entity.Property(e => e.UserId).HasColumnName("UserId");

                    entity.HasOne(d => d.User)
                            .WithMany(p => p.Payments)
                            .HasForeignKey(d => d.UserId)
                            .HasConstraintName("FK__payments__user_i__45F365D3");
                });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CreatedAt)
                                .HasColumnType("datetime")
                                .HasColumnName("created_at")
                                .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                                .IsUnicode(false)
                                .HasColumnName("description");

                entity.Property(e => e.Name)
                                .HasMaxLength(100)
                                .IsUnicode(false)
                                .HasColumnName("name");

                entity.Property(e => e.Price)
                                .HasColumnType("decimal(10, 2)")
                                .HasColumnName("price");

                entity.HasOne(d => d.Brand)
                                .WithMany(p => p.Products)
                                .HasForeignKey(d => d.BrandId)
                                .HasConstraintName("FK__products__brand___31EC6D26");

                entity.HasOne(d => d.Category)
                                .WithMany(p => p.Products)
                                .HasForeignKey(d => d.CategoryId)
                                .HasConstraintName("FK__products__catego__30F848ED");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("reviews");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Comment)
                    .IsUnicode(false)
                    .HasColumnName("comment");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.UserId).HasColumnName("UserId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__reviews__product__4AB81AF0");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__reviews__user_id__49C3F6B7");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
