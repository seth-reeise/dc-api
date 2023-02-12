using dc_api.Models;
using dc_api.Services;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new() {Title = "AdminService", Version = "v1"}); });

// bind setting and Mongo service
var connectionUri = Environment.GetEnvironmentVariable("CONNECTION_URI");
var client = new MongoClient(connectionUri);
var database = client.GetDatabase("The_divine_canine");
builder.Services.AddSingleton(database);
    
// Create Singleton instance 
builder.Services.AddSingleton<ICustomerService, CustomerService>();

// Add CORS
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdminService v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("corsapp");

app.Run();