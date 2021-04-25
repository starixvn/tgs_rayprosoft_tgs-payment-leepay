using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
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
		public async Task<DepositConfirmResponseViewModel> DepositCallbackAsync(
			[FromServices] IOptions<JsonOptions> jsonOptions,
			[FromServices] ITgsRepository tgsRepository,
			[FromServices] LeePaySerializer serializer,
			[FromServices] LeePayHasher hasher,
			[FromBody] DepositReponseViewModel response)
		{
			var jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;
			_logger.LogInformation(JsonSerializer.Serialize(response, jsonSerializerOptions));

			var depositFormId = new Guid(Convert.FromBase64String(response.CustomOrderNo));

			var depositForm = await tgsRepository
				.GetDepositFormByIdAsync(depositFormId)
				.ConfigureAwait(false);

			var applyInfo = serializer.Deserialize<LeePayApplyInfo>(depositForm.ApplyInfo!);

			var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(
				JsonSerializer.Serialize(response, jsonSerializerOptions))!;

			dict.Remove("txnSign");

			var signCode = hasher.ComputeHash(applyInfo.Settings.SystemId, applyInfo.Settings.SystemKey, dict);

			if (signCode == response.TranscationSigningCode)
				return new DepositConfirmResponseViewModel
				{
					Code = "00",
					Message = "Success"
				};
			else
				throw new Exception();
		}
	}
}
