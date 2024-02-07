using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project_Net.core.config;
using Weight_Watchers.data;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


//������ ����,
//�������� �BL 
var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;


builder.Services.AddControllers();
/*
builder.Services.AddSingleton(setting);
*/
//.����� ������ ������� �� ����� ������ ��� �MAPPER 
builder.Services.ConfigureServices();
// Add services to the container.

builder.Host.UseSerilog((context, configuration) =>
{

    ///����� �� ������ �������d 
    configuration.ReadFrom.Configuration(context.Configuration);

});

builder.Services.AddDbContext<Weight_Watchers_Context>(option =>
{
    ///������ ����� DB ������ 
    option.UseSqlServer(configuration.GetConnectionString("Weight_WatchersConnectionString"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();
//���� ����� ��� �������� 
app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.MapControllers();

app.UseMiddleware(typeof(MiddleWare));

app.Run();
