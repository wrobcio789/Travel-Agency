using System.Reflection;
using Pg.Rsww.RedTeam.OfferService.Application;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddApplication(builder.Configuration)
	.AddHttpLogging(options =>
		options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
		                        HttpLoggingFields.RequestBody |
		                        HttpLoggingFields.ResponsePropertiesAndHeaders |
		                        HttpLoggingFields.ResponseBody
	)
	.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services
	.AddControllers()
	.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services
	.AddEndpointsApiExplorer()
	.AddSwaggerGen(c =>
	{
		var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
		var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
		c.IncludeXmlComments(xmlPath);
	});

var app = builder.Build();
app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(x => { x.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Offer Service API"); });
}
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();