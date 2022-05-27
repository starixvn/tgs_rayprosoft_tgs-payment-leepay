using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tgs.Payment.Clients;
using Tgs.Payment.LeePay.Services;
using Tgs.Payment.LeePay.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.Payment.LeePay.Controllers;

[Route("[controller]")]
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
		[FromServices] IGetService<GetWithdrawFormQuery, WithdrawFormInfo> getWithdrawFormService,
		[FromServices] ICommandService<CheckSigningCodeCommand> checkSigningCodeService,
		[FromServices] ICommandService<SaveWithdrawReturnCommand> saveReturnService,
		[FromServices] IPaymentClient paymentClient,
		[FromBody] Dictionary<string, object> response)
	{
		var jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;

		var json = JsonSerializer.Serialize(response, jsonSerializerOptions);

		_logger.LogInformation(json);

		var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;
		_ = dict.Remove("txnSign");

		var withdrawResponse = JsonSerializer.Deserialize<WithdrawResponseViewModel>(json)!;

		var withdrawForm = await getWithdrawFormService
			.GetAsync(new GetWithdrawFormQuery(withdrawResponse.CustomOrderNo))
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

			var saveWithdrawCommand = new SaveWithdrawReturnCommand(
					withdrawForm.Id,
					returnData,
					withdrawResponse.OrderAmount);

			await saveReturnService
				.ExecuteAsync(saveWithdrawCommand)
				.ConfigureAwait(false);

			if (withdrawResponse.Status == "Success" && withdrawForm.Status == PaymentFlowState.Approved)
			{
				await paymentClient
					.PaidWithdrawAsync(
						withdrawForm.Id,
						withdrawForm.BrandId,
						HttpContext.RequestAborted)
					.ConfigureAwait(false);
			}

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
