using dc_api.Models;
using dc_api.Services;


var builder = WebApplication.CreateBuilder(args);

// bind setting and Mongo service

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
// Create Singleton instance 
builder.Services.AddSingleton<ICustomerService, CustomerService>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "dc-api", Version = "v1" });
});

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
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dc-api v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("corsapp");

app.Run();
