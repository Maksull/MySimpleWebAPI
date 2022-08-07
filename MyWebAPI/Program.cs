using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:MyWebAPI"]);
    opts.EnableSensitiveDataLogging();
});

builder.Services.AddControllers();

#endregion

var app = builder.Build();

#region App

app.MapGet("/", () => "Hello World!");

app.MapControllers();

SeedData.EnsurePopulated(app);

#endregion

app.Run();
