using APIAlamoCN.Exceptions;
using APIAlamoCN.Repositories;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;
using APIAlamoCN.Models.ApiKey;
//using APIAlamoCN.Services.ApiKey;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

//Repositorys
builder.Services.AddScoped<PedidosRepository>();
builder.Services.AddScoped<CuentaCorrienteRepository>();
builder.Services.AddScoped<ComprobanteRepository>();
builder.Services.AddScoped<ClientesRepository>();
builder.Services.AddScoped<FCRepository>();
builder.Services.AddScoped<CORepository>();
builder.Services.AddScoped<LoginRepository>();
var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn
            {ColumnName = "ControllerName", DataType = SqlDbType.NVarChar, NonClusteredIndex = true},
        new SqlColumn
            {ColumnName = "JsonBody", DataType = SqlDbType.NVarChar, NonClusteredIndex = true}
    }
};

builder.Services.AddSingleton<Serilog.ILogger>(options =>
{
    var connstring = builder.Configuration["Serilog:ConnectionString"];
    var tableName = builder.Configuration["Serilog:TableName"];

    return new LoggerConfiguration()
                .WriteTo
                .MSSqlServer(
                    connectionString: connstring,
                    columnOptions: columnOptions,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = tableName, AutoCreateSqlTable = true },
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .CreateLogger();

});


//Filtro de Excepcion
builder.Services.AddMvc(Options =>
{
    Options.Filters.Add(typeof(ExceptionFilter));
})
               .AddJsonOptions(options =>
               {
                   options.JsonSerializerOptions.PropertyNamingPolicy = null;
                   options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
               });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var key = builder.Configuration["key"];
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
