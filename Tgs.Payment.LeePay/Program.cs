using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tgs.Payment.LeePay;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.AddJsonFile("/config/appsettings.json", true);

builder.Logging
	.AddTgsLog();

builder.Services
	.AddLeePay((options, sp) =>
	{
		var config = sp.GetRequiredService<IConfiguration>();

		config.GetSection("LeePay").Bind(options);
	})
	.AddSingleton<LeePaySerializer>();

builder.Services
	.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
		options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
		options.JsonSerializerOptions.Converters.Add(new LeePayDateTimeOffsetConverter());
	});

builder.Services.AddTransient(sp =>
{
	var options = sp.GetRequiredService<IOptions<JsonOptions>>().Value.JsonSerializerOptions;

	return options;
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapControllers();
app.MapHealthChecks("/healthz");

app.Run();
