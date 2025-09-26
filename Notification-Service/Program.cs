using Cargadores.Middleware;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Notification_Service.Models.Services;
using Notification_Service.Models.Settings;
using Notification_Service.Services.Commands.BusinessUser;
using Notification_Service.Services.Queries;
using Notification_Service.Settings;
using System.Text;
using static Notification_Service.Services.Commands.BusinessUser.SendNotificationToBusinessUserServiceHandler;


var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.Configure<DriverUserDatabaseSettings>(
    builder.Configuration.GetSection("DriverUserDatabase"));
builder.Services.Configure<BusinessUserDatabaseSettings>(
    builder.Configuration.GetSection("BusinessUserDatabase"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(); 
builder.Services.AddTransient<ISendNotificationToBusinessUserServiceHandler, SendNotificationToBusinessUserServiceHandler>();
builder.Services.Configure<FirebaseSettings>(builder.Configuration.GetSection("FirebaseSettings"));
builder.Services.AddHttpClient<ISendNotificationToBusinessUserServiceHandler, SendNotificationToBusinessUserServiceHandler>();
builder.Services.AddCors(options => options.AddPolicy("AllowWebApp",
                 builder => builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()));

var app = builder.Build();
//var Defaultapp = FirebaseApp.Create(new AppOptions()
//{
//    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources","Confidential", "file.json")),
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder =>
builder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);
app.UseHttpsRedirection();
app.UseMiddleware<ApiKeyAuthMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
