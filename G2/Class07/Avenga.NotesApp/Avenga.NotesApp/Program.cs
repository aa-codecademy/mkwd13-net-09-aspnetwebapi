using Avenga.NotesApp.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//read from appSettings.Json, find the property AppSettings from the main Object
var appSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettings);
AppSettings appSettingsObject = appSettings.Get<AppSettings>();

//Dependency Injection
DependencyInjectionHelper.InjectDbContext(builder.Services, appSettingsObject.ConnectionString);
DependencyInjectionHelper.InjectRepositories(builder.Services);
DependencyInjectionHelper.InjectDapperRepositories(builder.Services, appSettingsObject.ConnectionString);
//DependencyInjectionHelper.InjectDapperRepositories(builder.Services, "Server=.;Database=NotesAppDatabase;Trusted_Connection=True;TrustServerCertificate=True");
DependencyInjectionHelper.InjectAdoRepositories(builder.Services, appSettingsObject.ConnectionString);
DependencyInjectionHelper.InjectServices(builder.Services);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
