using Caracal.SpringBoot.Data.Postgres.Models.Deposits;
using Caracal.SpringBoot.Data.Postgres.Models.Withdrawals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Caracal.SpringBoot.Data.Postgres; 

public sealed class DataContext: DbContext {
  public DataContext(DbContextOptions<DataContext> options) : base(options) { }
  public DbSet<Withdrawal> Withdrawals { get; set; } = null!;
  public DbSet<Deposit> Deposits { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<Withdrawal>(w => {
      w.ToTable("withdrawal", "withdrawals");
      w.Property(e => e.Id)
        .HasColumnName("id")
        .HasIdentityOptions();
      w.Property(e => e.Account).HasColumnName("account");
      w.Property(e => e.Amount).HasColumnName("amount");
      w.Property(e => e.RequestedDate)
        .HasColumnName("requested_date")
        .HasDefaultValueSql("now()");
      
      w.Property(e => e.Status)
        .HasColumnName("status")
        .HasDefaultValue(0);
      
      w.HasKey(e => e.Id);
    });
    
    modelBuilder.Entity<Deposit>(d => {
      d.ToTable("deposit", "deposits");
      d.Property(e => e.Id)
        .HasColumnName("id")
        .HasIdentityOptions();
      d.Property(e => e.Account).HasColumnName("account");
      d.Property(e => e.Amount).HasColumnName("amount");
      d.Property(e => e.DepositedDate)
        .HasColumnName("deposited_date")
        .HasDefaultValueSql("now()");
      
      d.Property(e => e.Status)
        .HasColumnName("status")
        .HasDefaultValue(0);
      
      d.HasKey(e => e.Id);
    });
  }
}

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
  public DataContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
    optionsBuilder.UseNpgsql("Server=localhost;Port=5434;Database=springboot;User Id=postgres;Password=postgress;");

    return new DataContext(optionsBuilder.Options);
  }
}

// dotnet tool install -g dotnet-ef
// dotnet tool update -g dotnet-ef

// dotnet ef migrations add InitialCreate
// dotnet ef database update