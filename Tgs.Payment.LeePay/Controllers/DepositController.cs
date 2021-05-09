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
			[FromServices] ICommandService<SaveDepositReturnCommand> saveReturnService,
			[FromBody] Dictionary<string, object> response)
		{
			var jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;

			var json = JsonSerializer.Serialize(response, jsonSerializerOptions);

			_logger.LogInformation(json);

			var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;
			dict.Remove("txnSign");

			var depositResponse = JsonSerializer.Deserialize<DepositResponseViewModel>(json)!;

			var depositForm = await getDepositFormService
				.GetAsync(new GetDepositFormQuery(depositResponse.CustomOrderNo))
				.ConfigureAwait(false);

			var checkSigningCodeCommand = new CheckSigningCodeCommand(
				dict,
				depositResponse.TranscationSigningCode,
				depositForm.ApplyInfo!.Settings.SystemId,
				depositForm.ApplyInfo!.Settings.SystemKey);

			await checkSigningCodeService
				.ExecuteAsync(checkSigningCodeCommand)
				.ConfigureAwait(false);

			if (await checkSigningCodeCommand.CommandResultSource.Task.ConfigureAwait(false))
			{
				var returnData = new LeePayDepositReturn(
					depositResponse.CustomOrderNo,
					depositResponse.NotifyCustomMessage,
					depositResponse.OrderNo,
					depositResponse.Uid,
					depositResponse.Status,
					depositResponse.OrderAmount,
					depositResponse.FeeRate,
					depositResponse.Fee,
					depositResponse.ToAddress,
					depositResponse.TransactionHash,
					depositResponse.CreatedAt,
					depositResponse.Channel,
					depositResponse.ActualAmount);
				var saveReturnCommand = new SaveDepositReturnCommand(depositForm.Id, returnData);

				await saveReturnService.ExecuteAsync(saveReturnCommand).ConfigureAwait(false);

				return new DepositConfirmViewModel
				{
					Code = "00",
					Message = "Success"
				};
			}
			else
				throw new Exception();
		}
	}
}
