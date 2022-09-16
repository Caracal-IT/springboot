namespace Caracal.SpringBoot.Data.Postgres.Models.Deposits; 

public sealed class Deposit {
  public int Id { get; set; }
  public string Account { get; set; } = "Unknown";
  public decimal Amount { get; set; }
  public DateTime DepositedDate { get; set; } = DateTime.UtcNow;
  public int Status { get; set; }
}