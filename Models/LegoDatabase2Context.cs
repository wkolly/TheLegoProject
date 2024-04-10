using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TheLegoProject.Models;

public partial class LegoDatabase2Context : DbContext
{
    public LegoDatabase2Context()
    {
    }

    public LegoDatabase2Context(DbContextOptions<LegoDatabase2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<CustRecommendation> CustRecommendations { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<LineItem> LineItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Recommendation> Recommendations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=LegoDatabase2.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustRecommendation>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.ToTable("Cust_Recommendation");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("customer_ID");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.Rec1).HasColumnName("Rec_1");
            entity.Property(e => e.Rec2).HasColumnName("Rec_2");
            entity.Property(e => e.Rec3).HasColumnName("Rec_3");
            entity.Property(e => e.Rec4).HasColumnName("Rec_4");
            entity.Property(e => e.Rec5).HasColumnName("Rec_5");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("customer_ID");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.CountryOfResidence).HasColumnName("country_of_residence");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.LastName).HasColumnName("last_name");
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.HasKey(e => new { e.TransactionId, e.ProductId });

            entity.ToTable("LineItem");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_ID");
            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Rating).HasColumnName("rating");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.TransactionId);

            entity.ToTable("Order");

            entity.Property(e => e.TransactionId)
                .ValueGeneratedNever()
                .HasColumnName("transaction_ID");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Bank).HasColumnName("bank");
            entity.Property(e => e.CountryOfTransaction).HasColumnName("country_of_transaction");
            entity.Property(e => e.CustomerId).HasColumnName("customer_ID");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
            entity.Property(e => e.EntryMode).HasColumnName("entry_mode");
            entity.Property(e => e.Fraud).HasColumnName("fraud");
            entity.Property(e => e.ShippingAddress).HasColumnName("shipping_address");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.TypeOfCard).HasColumnName("type_of_card");
            entity.Property(e => e.TypeOfTransaction).HasColumnName("type_of_transaction");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("product_ID");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImgLink).HasColumnName("img_link");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.NumParts).HasColumnName("num_parts");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.PrimaryColor).HasColumnName("primary_color");
            entity.Property(e => e.SecondaryColor).HasColumnName("secondary_color");
            entity.Property(e => e.Subcategory).HasColumnName("subcategory");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            entity.ToTable("Recommendation");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("product_ID");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.PopScore).HasColumnName("pop_score");
            entity.Property(e => e.Rec1).HasColumnName("rec_1");
            entity.Property(e => e.Rec2).HasColumnName("rec_2");
            entity.Property(e => e.Rec3).HasColumnName("rec_3");
            entity.Property(e => e.Rec4).HasColumnName("rec_4");
            entity.Property(e => e.Rec5).HasColumnName("rec_5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
