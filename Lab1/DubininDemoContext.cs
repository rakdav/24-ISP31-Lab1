using System;
using System.Collections.Generic;
using Lab1.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab1;

public partial class DubininDemoContext : DbContext
{
    public DubininDemoContext()
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public DubininDemoContext(DbContextOptions<DubininDemoContext> options)
        : base(options)
    {
       // Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Country> Country { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=TeacherPC;Initial Catalog=DubininDemo;User ID=user1;Password=user1;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
