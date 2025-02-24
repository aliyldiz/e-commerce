using System.Security.Claims;
using System.Text;
using ECommerceApi.API.Configurations.ColumnWriters;
using ECommerceApi.API.Extensions;
using ECommerceApi.API.Filters;
using ECommerceApi.Application;
using ECommerceApi.Application.Validators;
using ECommerceApi.Infrastructure;
using ECommerceApi.Infrastructure.Filters;
using ECommerceApi.Infrastructure.Services.Storage.Azure;
using ECommerceApi.Persistence;
using ECommerceApi.SignalR;
using ECommerceApi.SignalR.Hubs;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using ILogger = Google.Apis.Logging.ILogger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor(); // client'tan gelen request neticesinde oluşturulan HttpContext nesnesine katmanlardaki class'lar üzerinden (business logic) erişebilmemizi sağlayan bir servistir.
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddSignalRServices();

// builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => 
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
));

Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "Logs", 
        needAutoCreateTable: true,
        columnOptions: new Dictionary<string, ColumnWriterBase>
        {
            {"Message", new RenderedMessageColumnWriter()},
            {"MessageTemplate", new MessageTemplateColumnWriter()},
            {"Level", new LevelColumnWriter()},
            {"Timestamp", new TimestampColumnWriter()},
            {"Exception", new ExceptionColumnWriter()},
            {"LogEvent", new LogEventSerializedColumnWriter()},
            {"UserName", new UserNameColumnWriter()}
        })
    .WriteTo.Seq(builder.Configuration["Seq:ServerUrl"])
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ValidationFilter>();
        options.Filters.Add<RolePermissionFilter>();
    })
    .ConfigureApiBehaviorOptions(options => 
        options.SuppressModelStateInvalidFilter = true);

builder.Services
    .AddValidatorsFromAssemblyContaining<CreateProductValidator>()
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Admin", option =>
{
    option.TokenValidationParameters = new()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, parameters) => expires != null ? expires > DateTime.UtcNow : false,
        
        NameClaimType = ClaimTypes.Name // JWT üzerinde Name claim'ine karşılık gelen değeri User.Identity.Name property'sinden elde edebiliriz.
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILogger<Program>>());

app.UseStaticFiles();

app.UseSerilogRequestLogging();
app.UseHttpLogging();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name", username);
    await next();
});

app.MapControllers();

app.MapHubs();

app.Run();
