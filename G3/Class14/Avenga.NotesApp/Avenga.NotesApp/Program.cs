using Avenga.NotesApp.Helpers;
using Avenga.NotesApp.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog; // was added manually
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//Adding swagger into our app with configs so we can use authorization inside swagger
//e.g. bearer {token}
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

//Adding serilog to app
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day));
//logs20250910
//builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Minute));
//logs202509101817


var appSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettings);

AppSettings appSettingsObject = appSettings.Get<AppSettings>();

DependencyInjectionHelper.InjectDbContext(builder.Services, appSettingsObject.ConnectionString);
DependencyInjectionHelper.InjectRepositories(builder.Services);
//DependencyInjectionHelper.InjectAdoRepositories(builder.Services, appSettingsObject.ConnectionString);
//DependencyInjectionHelper.InjectDapperRepositories(builder.Services, appSettingsObject.ConnectionString);
DependencyInjectionHelper.InjectServices(builder.Services);

//added httpclient in the application pipeline for the fruit service
//documentation about the external API https://www.fruityvice.com/doc/index.html#api-GET-GETsByOrder
//External api location https://www.fruityvice.com/#3
builder.Services.AddHttpClient<FruitService>();

builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowFrontendLocalhostOrLiveServer",
    policy =>
    policy.WithOrigins("http://127.0.0.1:5501/")
            .AllowAnyHeader()
            .AllowAnyMethod());

    options.AddPolicy("AllowFrontendNotesApp",
        policy => 
        policy.WithOrigins("https://noteapp.com")
                .AllowAnyHeader()
                .AllowAnyMethod());

    options.AddPolicy("AllowAll",
        policy => 
        policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                );
});

//Configure JWT
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
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Our very secret secret secret secret key"))
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

app.UseCors("AllowAll");
app.UseAuthentication(); // to be able to use JWT auth
app.UseAuthorization();

app.MapControllers();

app.Run();
