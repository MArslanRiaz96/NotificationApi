using Customer.Data;
using Customer.Data.Context;
using Customer.Manager;
using Customer.Model;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotificationApi.Extentions;
using NotificationApi.Filters;
using NotificationApi.HubService;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment environment = builder.Environment;

// Add services to the container.
builder.Services.AddDataInfrastructure(builder.Configuration);
builder.Services.AddModelLayer(builder.Configuration);
builder.Services.AddBusinessLayer(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy" ,builder =>
    {
                 builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed((host) => true)
                .WithOrigins(configuration.GetSection("WhiteListOrigins").GetChildren().Select(x => x.Value).ToArray())
               ;
    });
});
//builder.Services.AddSignalR(hubOptions => {
//    hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(25);
//   // hubOptions.MaximumReceiveMessageSize = 65_536;
//   // hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(15);
//   // hubOptions.MaximumParallelInvocationsPerClient = 2;
//    hubOptions.EnableDetailedErrors = true;
//    //hubOptions.StreamBufferCapacity = 15;

//    if (hubOptions?.SupportedProtocols is not null)
//    {
//        foreach (var protocol in hubOptions.SupportedProtocols)
//            Console.WriteLine($"SignalR supports {protocol} protocol.");
//    }
//});

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(3);
    hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(6);
});
                //.AddJsonProtocol(options =>
                //{
                //    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                //});

builder.Logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
builder.Logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
builder.Services.AddScoped<NotificationHub>();

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
app.UseRouting();

app.UseCors("CorsPolicy");
app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    dbContext.Database.SetCommandTimeout(3600);
    dbContext.Database.Migrate();
}
app.MapHub<NotificationHub>("/notification", options =>
{
    //options.Transports =
               // HttpTransportType.WebSockets;
            //    HttpTransportType.LongPolling;
   // options.CloseOnAuthenticationExpiration = true;
    //options.ApplicationMaxBufferSize = 65_536;
    //options.TransportMaxBufferSize = 65_536;
    //options.MinimumProtocolVersion = 0;
   // options.TransportSendTimeout = TimeSpan.FromMinutes(25);
    //options.WebSockets.CloseTimeout = TimeSpan.FromMinutes(25);
   // options.LongPolling.PollTimeout = TimeSpan.FromMinutes(25);
    //Console
    //.WriteLine($"Authorization data items: {options.AuthorizationData.Count}");
});
app.UseMiddleware<LoggingMiddleware>();
app.MapControllers();

app.Run();
