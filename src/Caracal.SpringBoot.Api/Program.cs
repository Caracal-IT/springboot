using Caracal.SpringBoot.Application;
using Caracal.SpringBoot.Application.Repositories;
using Caracal.SpringBoot.Data;
using Caracal.SpringBoot.Data.Postgres;
using Caracal.Web.Core.DI;
using Elastic.Apm.NetCoreAll;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args)
                            .WithSerilog();

builder.Services
       .AddSpringBoot()
       .AddSpringBootData();

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("StringBoot")));
builder.Services.AddTransient<IDataContext, MockDataContext>();

var app = builder.Build();

app.UseAllElasticApm(app.Configuration);
app.UseSpringBoot();

app.Run();

class MockDataContext: IDataContext {
  private readonly DataContext _dataContext;
  public MockDataContext(DataContext dataContext) {
    _dataContext = dataContext;
  }


  public List<string> GetData() {
    return _dataContext.Withdrawals.Select(w => w.Account).ToList();
  }
}