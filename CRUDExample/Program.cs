using Entities;
using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//add services into IoC container
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddDbContext<PersonDbContext>
    (options => {
        options.UseSqlServer(builder.Configuration["ConnectionStrings:SeconConnection"]);
    });

var app = builder.Build();

if (builder.Environment.IsDevelopment()) { 
    app.UseDeveloperExceptionPage();
}

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
