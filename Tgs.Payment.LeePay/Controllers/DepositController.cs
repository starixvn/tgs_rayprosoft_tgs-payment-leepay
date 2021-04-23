using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tgs.Payment.LeePay.ViewModels;

namespace Tgs.Payment.LeePay.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DepositController : ControllerBase
	{
		private readonly ILogger<DepositController> _logger;

		public DepositController(ILogger<DepositController> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpPost("callback")]
		public DepositConfirmResponseViewModel DepositCallback(
			[FromServices] IOptions<JsonOptions> jsonOptions,
			[FromBody] DepositReponseViewModel response)
		{
			_logger.LogInformation(JsonSerializer.Serialize(response, jsonOptions.Value.JsonSerializerOptions));

			return new DepositConfirmResponseViewModel
			{
				Code = "00",
				Message = "Success"
			};
		}
	}
}
