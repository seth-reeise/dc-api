using DC_CONTRACTFORM.Models;
using DC_CONTRACTFORM.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// bind setting and Mongo service

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
// Create Singleton instance 
builder.Services.AddSingleton<MongoDBService>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "dc_contractform", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dc_contractform v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
