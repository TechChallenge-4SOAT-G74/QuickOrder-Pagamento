using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using QuickOrderPagamento.Adapters.Driving.Api.Configuration;
using QuickOrderPagamento.Core.Domain.Entities;
using QuickOrderPagamento.Infra.MQ;

// Initializes a builder object 
var builder = WebApplication.CreateBuilder(args); // part of ASP.NET core

// Add services to the container
builder.Services.Configure<DatabaseMongoDBSettings>(
    builder.Configuration.GetSection("DatabaseMongoDBSettings")
);

builder.Services.Configure<MercadoPagoSettings>(
    builder.Configuration.GetSection("MercadoPagoSettings")
);

builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection("RabbitMqSettings")
);

// MongoDB
builder.Services.AddSingleton<IMongoDatabase>(options =>
{
    var settings = builder.Configuration.GetSection("DatabaseMongoDBSettings").Get<DatabaseMongoDBSettings>();
    var client = new MongoClient(settings.ConnectionString);
    return client.GetDatabase(settings.DatabaseName);
});

// Adds dependency injection configuration
builder.Services.AddDependencyInjectionConfiguration();

// Allows any origin, method, and header, but restricts origins to http://localhost:8090
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .WithOrigins("http://localhost:8090");
                      });
});

// Controllers configuration
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


// ----------------------- Swagger/OpenAPI -----------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuickOrderPagamento.Api", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

// ----------------------------------------------------------------

app.UseReDoc(c =>
{
    c.DocumentTitle = "QuickOrder API Documentation";
    c.SpecUrl = "/swagger/v1/swagger.json";
});

//app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
