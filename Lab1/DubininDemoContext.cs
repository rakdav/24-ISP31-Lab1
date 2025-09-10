using System;
using System.Collections.Generic;
using Lab1.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab1;

public partial class DubininDemoContext : DbContext
{
    public DubininDemoContext()
    {
        Database.EnsureCreated();
    }

    public DubininDemoContext(DbContextOptions<DubininDemoContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=TeacherPC;Initial Catalog=DubininDemo;User ID=user1;Password=user1;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.IdCompany).HasName("PK__Company__3AF752DF03715989");

            entity.ToTable("Company");

            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Users__B7C926386BFA2E9D");

            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdCompany)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Company_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
