using AutoMapper;
using ControleFerias.Data;
using ControleFerias.DTO;
using ControleFerias.Example;
using ControleFerias.Validation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);



//builder.Services.AddDbContext<ApplicationDBContext>(option =>
//    option.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerString")));

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.ExampleFilters();
    c.EnableAnnotations();
});


builder.Services.AddSwaggerExamplesFromAssemblyOf<FeriasCriarDTO>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ColaboradorAlterarDTOExample>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5174") // frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            if (contextFeature.Error is DomainExceptionValidation)
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            else
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = context.Response.StatusCode,
                Mensagem = contextFeature.Error.Message
            });
        }
    });
});

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();



app.Run();

public partial class Program { }
