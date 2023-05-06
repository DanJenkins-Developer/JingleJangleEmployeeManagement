using System;
using System.Collections.Generic;
using JingleJangle.Models;
using Microsoft.EntityFrameworkCore;

namespace JingleJangle.Data;

public partial class JingleJangleEmployees2023Context : DbContext
{
    public JingleJangleEmployees2023Context()
    {
    }

    public JingleJangleEmployees2023Context(DbContextOptions<JingleJangleEmployees2023Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonType> PersonTypes { get; set; }

    public virtual DbSet<Prehire> Prehires { get; set; }

    public virtual DbSet<Retiree> Retirees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=GRATEFULDEAD;Initial Catalog=jingleJangleEmployees2023;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("EmployeeID");
            entity.Property(e => e.JobTitle).HasMaxLength(50);
            entity.Property(e => e.MonthlySalary).HasColumnType("decimal(15, 2)");

            entity.HasOne(d => d.EmployeeNavigation).WithOne(p => p.Employee)
                .HasForeignKey<Employee>(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Person");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK_Person.Person");

            entity.ToTable("Person");

            entity.Property(e => e.EmployeeId)
                //.ValueGeneratedNever()
                .ValueGeneratedOnAdd()
                .HasColumnName("EmployeeID");
            entity.Property(e => e.EmploymentDate).HasColumnType("date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.PersonTypeId).HasColumnName("PersonTypeID");

            entity.HasOne(d => d.PersonType).WithMany(p => p.People)
                .HasForeignKey(d => d.PersonTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Person_PersonType");
        });

        modelBuilder.Entity<PersonType>(entity =>
        {
            entity.ToTable("PersonType");

            entity.Property(e => e.PersonTypeId)
                .ValueGeneratedNever()
                .HasColumnName("PersonTypeID");
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Prehire>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK_Person.Prehire");

            entity.ToTable("Prehire");

            entity.HasIndex(e => e.EmployeeId, "IX_Employee.Prehire");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("EmployeeID");
            entity.Property(e => e.OfferAcceptanceDate).HasColumnType("date");
            entity.Property(e => e.OfferExtendedDate).HasColumnType("date");

            entity.HasOne(d => d.Employee).WithOne(p => p.Prehire)
                .HasForeignKey<Prehire>(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prehire_Person");
        });

        modelBuilder.Entity<Retiree>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK_Person.Retiree");

            entity.ToTable("Retiree");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("EmployeeID");
            entity.Property(e => e.RetirementDate).HasColumnType("date");
            entity.Property(e => e.RetirementProgram).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithOne(p => p.Retiree)
                .HasForeignKey<Retiree>(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Retiree_Person");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
