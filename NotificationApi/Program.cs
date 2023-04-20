using Customer.Data;
using Customer.Manager;
using Customer.Model;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotificationApi.Extentions;
using NotificationApi.Filters;
using NotificationApi.Hub;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment environment = builder.Environment;

// Add services to the container.
builder.Services.AddDataInfrastructure(builder.Configuration);
builder.Services.AddModelLayer(builder.Configuration);
builder.Services.AddBusinessLayer(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddMvc(options =>
//{
//    options.Filters.Add(typeof(ValidateModelStateAttribute));
//})
//.AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<CreateOrUpdateCustomerDtoValidator>());

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();
app.UseCors(builder2 => builder2
                               .WithOrigins(builder.Configuration.GetSection("WhiteListOrigins").GetChildren().Select(x => x.Value).ToArray())
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials()
                       );
app.UseAuthorization();
app.MapHub<NotificationHub>("/notification");
app.UseMiddleware<LoggingMiddleware>();
app.MapControllers();

app.Run();
