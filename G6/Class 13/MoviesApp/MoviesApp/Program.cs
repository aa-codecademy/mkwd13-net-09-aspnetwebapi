using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoviesApp.Helpers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
	c =>
	{
		c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		{
			Description = "JWT Authorization header using Bearer scheme.",
			Name = "Authorization",
			In = ParameterLocation.Header,
			Type = SecuritySchemeType.ApiKey,
			Scheme = "Bearer"
		});

		c.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					},
					Scheme = "oauth2",
					Name = "Bearer",
					In = ParameterLocation.Header
				},
				new List<string>()
			}
		});
	});

DependencyInjectionHelper.InjectDbContext(builder.Services);
DependencyInjectionHelper.InjectRepositories(builder.Services);
DependencyInjectionHelper.InjectServices(builder.Services);

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
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Our secret secret secret secret secret secret secret secret key")),
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero
	};
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication(); //always use authentication before authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
