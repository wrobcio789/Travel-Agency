using Pg.Rsww.RedTeam.OrderService.Application;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpLogging;
using Pg.Rsww.RedTeam.OrderService.Api.Middleware;
using Pg.Rsww.RedTeam.OrderService.Api.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddApplication(builder.Configuration)
	.AddHttpLogging(options =>
		options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
		                        HttpLoggingFields.RequestBody |
		                        HttpLoggingFields.ResponsePropertiesAndHeaders |
		                        HttpLoggingFields.ResponseBody
	)
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
app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(x => { x.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Order Service API"); });
}
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();