using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using MyWebAPI.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:MyWebAPI"]);
    opts.EnableSensitiveDataLogging();
});

builder.Services.AddControllers().AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

builder.Services.Configure<MvcNewtonsoftJsonOptions>(opts =>
{
    opts.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.Configure<MvcOptions>(opts =>
{
    //opts.RespectBrowserAcceptHeader = true;
    opts.ReturnHttpNotAcceptable = true;
});

/*builder.Services.Configure<JsonOptions>(opts =>
{
    opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});*/

builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("menu", new OpenApiInfo { Title = "MyWebAPI", Version = "v1"});
});

#endregion

var app = builder.Build();

#region App

app.UseSwagger();
app.UseSwaggerUI(opts =>
{
    opts.SwaggerEndpoint("/swagger/menu/swagger.json", "MyWebAPI");
});

app.MapGet("/", () => "Hello World!");

app.MapControllers();

SeedData.EnsurePopulated(app);

#endregion

app.Run();
