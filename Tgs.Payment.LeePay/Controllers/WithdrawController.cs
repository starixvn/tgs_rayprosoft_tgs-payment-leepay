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
	public class WithdrawController : ControllerBase
	{
		private readonly ILogger<WithdrawController> _logger;

		public WithdrawController(ILogger<WithdrawController> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpPost("callback")]
		public async Task<WithdrawConfirmViewModel> WithdrawCallbackAsync(
			[FromServices] IOptions<JsonOptions> jsonOptions,
			[FromServices] IGetService<GetDepositFormQuery, DepositFormInfo> getDepositFormService,
			[FromServices] ICommandService<CheckSigningCodeCommand> checkSigningCodeService,
			[FromBody] Dictionary<string, object> response)
		{
			var jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;
			_logger.LogInformation(JsonSerializer.Serialize(response, jsonSerializerOptions));

			//var depositForm = await getDepositFormService
			//	.GetAsync(new GetDepositFormQuery(response.CustomOrderNo))
			//	.ConfigureAwait(false);

			var json = JsonSerializer.Serialize(response, jsonSerializerOptions);

			var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;
			dict.Remove("txnSign");

			var withdrawResponse = JsonSerializer.Deserialize<WithdrawResponseViewModel>(json)!;

			//var checkSigningCodeCommand = new CheckSigningCodeCommand(
			//	dict,
			//	response.TranscationSigningCode,
			//	depositForm.ApplyInfo!.Settings.SystemId,
			//	depositForm.ApplyInfo!.Settings.SystemKey);

			var checkSigningCodeCommand = new CheckSigningCodeCommand(
				dict,
				withdrawResponse.TranscationSigningCode,
				"WG",
				"7ujm5tgB");
			await checkSigningCodeService
				.ExecuteAsync(checkSigningCodeCommand)
				.ConfigureAwait(false);

			if (await checkSigningCodeCommand.CommandResultSource.Task.ConfigureAwait(false))
				return new WithdrawConfirmViewModel
				{
					Code = "00",
					Message = "Success"
				};
			else
				throw new Exception();
		}
	}
}
