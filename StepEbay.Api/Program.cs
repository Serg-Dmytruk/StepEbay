using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StepEbay.Data;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using StepEbay.Main.Api.Common.Services;

var builder = WebApplication.CreateBuilder(args);

//DB
string applicationDbContext = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(applicationDbContext));
//Services
builder.Services.AddService();
builder.Services.AddDbService();
//
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StepEbay.Main.Api", Version = "v1" });

    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
