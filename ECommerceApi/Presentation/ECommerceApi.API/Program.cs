using System.Text;
using ECommerceApi.Application;
using ECommerceApi.Application.Validators;
using ECommerceApi.Infrastructure;
using ECommerceApi.Infrastructure.Filters;
using ECommerceApi.Infrastructure.Services.Storage.Azure;
using ECommerceApi.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();

// builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => 
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()
));

builder.Services.AddControllers(options => 
        options.Filters.Add<ValidationFilter>())
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
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]!)),
        LifetimeValidator = (before, expires, token, parameters) => expires != null && expires > DateTime.UtcNow, 
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
