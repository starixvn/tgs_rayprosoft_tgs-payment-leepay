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
			[FromServices] ICommandService<SaveWithdrawReturnCommand> saveReturnService,
			[FromBody] Dictionary<string, object> response)
		{
			var jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;

			var json = JsonSerializer.Serialize(response, jsonSerializerOptions);

			_logger.LogInformation(json);

			var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;
			dict.Remove("txnSign");

			var withdrawResponse = JsonSerializer.Deserialize<WithdrawResponseViewModel>(json)!;

			var withdrawForm = await getDepositFormService
				.GetAsync(new GetDepositFormQuery(withdrawResponse.CustomOrderNo))
				.ConfigureAwait(false);

			var checkSigningCodeCommand = new CheckSigningCodeCommand(
				dict,
				withdrawResponse.TranscationSigningCode,
				withdrawForm.ApplyInfo!.Settings.SystemId,
				withdrawForm.ApplyInfo!.Settings.SystemKey);

			await checkSigningCodeService
				.ExecuteAsync(checkSigningCodeCommand)
				.ConfigureAwait(false);

			if (await checkSigningCodeCommand.CommandResultSource.Task.ConfigureAwait(false))
			{
				var returnData = new LeePayWithdrawReturn(
					withdrawResponse.Channel,
					withdrawResponse.CreatedAt,
					withdrawResponse.CustomOrderNo,
					withdrawResponse.Fee,
					withdrawResponse.FeeRate,
					withdrawResponse.NotifyCustomMessage,
					withdrawResponse.OrderAmount,
					withdrawResponse.OrderNo,
					withdrawResponse.Status,
					withdrawResponse.ToAddress,
					withdrawResponse.TransactionHash);
				var saveWithdrawCommand = new SaveWithdrawReturnCommand(withdrawForm.Id, returnData);

				await saveReturnService.ExecuteAsync(saveWithdrawCommand).ConfigureAwait(false);

				return new WithdrawConfirmViewModel
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
