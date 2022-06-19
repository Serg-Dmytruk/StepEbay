using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StepEbay.Admin.Api.Common.Services;
using StepEbay.Admin.Api.Common.Services.DbSeeder;
using StepEbay.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StepEbay.Admin.Api", Version = "v1" });

    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//DB
string applicationDbContext = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(applicationDbContext));

//Services
builder.Services.AddService();
builder.Services.AddDbService();

var app = builder.Build();

if (app.Configuration.GetSection("UseSeed").Get<bool>())
{
    using (var seederService = app.Services.CreateScope())
    {
        var seeder = seederService.ServiceProvider.GetRequiredService<ISeeder>();
        await seeder.SeedApplication();
    }
}




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
