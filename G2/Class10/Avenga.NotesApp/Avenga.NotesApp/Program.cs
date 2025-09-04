using Avenga.NotesApp.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme, e.g" +
        "\"bearer {token} \"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

//read from appSettings.Json, find the property AppSettings from the main Object
var appSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettings);
AppSettings appSettingsObject = appSettings.Get<AppSettings>();

//Dependency Injection
DependencyInjectionHelper.InjectDbContext(builder.Services, appSettingsObject.ConnectionString);
DependencyInjectionHelper.InjectRepositories(builder.Services);
//DependencyInjectionHelper.InjectDapperRepositories(builder.Services, appSettingsObject.ConnectionString);
//DependencyInjectionHelper.InjectDapperRepositories(builder.Services, "Server=.;Database=NotesAppDatabase;Trusted_Connection=True;TrustServerCertificate=True");
//DependencyInjectionHelper.InjectAdoRepositories(builder.Services, appSettingsObject.ConnectionString);
DependencyInjectionHelper.InjectServices(builder.Services);

//Configure JWT
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    //Obligation of metadata in request is false
    x.RequireHttpsMetadata = false;
    // we expect the token into httpContect
    x.SaveToken = true;
    //how to validate the token
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        //the secret key
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettingsObject.SecretKey))
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Our very very secret secret key!"))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Sekogas prvo odi avtentikacija (jas sum stojan) => da se osigurame deka liceto e stojan
app.UseAuthentication();
//potoa odi Avtorizacija (momentot sto moze stojan da pravi, odnosno sto mu e dozvoleno)
app.UseAuthorization();
app.MapControllers();
app.Run();
