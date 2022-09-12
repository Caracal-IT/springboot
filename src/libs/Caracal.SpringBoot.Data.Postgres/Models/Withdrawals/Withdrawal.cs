namespace Caracal.SpringBoot.Data.Postgres.Models.Withdrawals; 

public sealed class Withdrawal {
  public int Id { get; set; }
  public string Account { get; set; } = "";
  public decimal Amount { get; set; }
  public DateTime RequestedDate { get; set; }
  public int Status { get; set; }
}