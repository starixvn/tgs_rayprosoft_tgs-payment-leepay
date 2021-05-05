using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tgs.Payment.LeePay.Services;
using Tgs.Payment.LeePay.ViewModels;
using TGS.ServiceUtility;

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
		public async Task<DepositConfirmViewModel> DepositCallbackAsync(
			[FromServices] IOptions<JsonOptions> jsonOptions,
			[FromServices] IGetService<GetDepositFormQuery, DepositFormInfo> getDepositFormService,
			[FromServices] ICommandService<CheckSigningCodeCommand> checkSigningCodeService,
			[FromBody] DepositResponseViewModel response)
		{
			var jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;
			_logger.LogInformation(JsonSerializer.Serialize(response, jsonSerializerOptions));

			var depositForm = await getDepositFormService
				.GetAsync(new GetDepositFormQuery(response.CustomOrderNo))
				.ConfigureAwait(false);

			var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(
				JsonSerializer.Serialize(response, jsonSerializerOptions))!;
			dict.Remove("txnSign");

			var checkSigningCodeCommand = new CheckSigningCodeCommand(
				dict,
				response.TranscationSigningCode,
				depositForm.ApplyInfo!.Settings.SystemId,
				depositForm.ApplyInfo!.Settings.SystemKey);
			await checkSigningCodeService
				.ExecuteAsync(checkSigningCodeCommand)
				.ConfigureAwait(false);

			if (await checkSigningCodeCommand.CommandResultSource.Task.ConfigureAwait(false))
				return new DepositConfirmViewModel
				{
					Code = "00",
					Message = "Success"
				};
			else
				throw new Exception();
		}
	}
}
