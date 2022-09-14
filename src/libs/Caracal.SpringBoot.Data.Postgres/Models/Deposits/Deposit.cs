namespace Caracal.SpringBoot.Data.Postgres.Models.Deposits; 

public class Deposit {
  public int Id { get; set; }
  public string Account { get; set; } = "";
  public decimal Amount { get; set; }
  public DateTime DepositedDate { get; set; }
  public int Status { get; set; }
}