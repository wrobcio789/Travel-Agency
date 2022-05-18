using Pg.Rsww.RedTeam.OrderService.Application;
using System.Text.Json.Serialization;
using Pg.Rsww.RedTeam.OrderService.Api.Middleware;
using Pg.Rsww.RedTeam.OrderService.Api.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddApplication(builder.Configuration)
	.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
	.AddSingleton<JwtHelper>()
	.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services
	.AddControllers()
	.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services
	.AddEndpointsApiExplorer()
	.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();