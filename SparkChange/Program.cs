using Microsoft.EntityFrameworkCore;
using SparkChange.Converters;
using SparkChange.Domain;
using SparkChange.Resources;
using SparkChange.Resources.Validators;
using SparkChange.Utilities;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddDbContext<DatabaseContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddTransient<IProductConverter, ProductConverter>();
builder.Services.AddTransient<IBasketConverter, BasketConverter>();
builder.Services.AddTransient<IBasketValidator, BasketValidator>();
builder.Services.AddTransient<ICurrencyResource, CurrencyResource>();
builder.Services.AddTransient<IGoodsResource, GoodsResource>();
builder.Services.AddTransient<IBasketResource, BasketResource>();
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddHttpClient<IApiClient, ApiClient>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetSection("CurrencyLayerApi:BaseUrl").Get<string>());
})
.SetHandlerLifetime(TimeSpan.FromMinutes(5));

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization();

app.MapControllers();

app.Run();