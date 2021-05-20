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
	[Route("[controller]")]
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

			var depositResponse = JsonSerializer.Deserialize<DepositResponseViewModel>(json, jsonSerializerOptions)!;

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

		[HttpGet("payment/{formId}")]
		public async Task SimulationPaymentAsync(
			[FromServices] IGetService<GetDepositFormQuery, DepositFormInfo> getDepositFormService,
			[FromServices] ICommandService<CheckSigningCodeCommand> checkSigningCodeService,
			[FromServices] ICommandService<SaveDepositReturnCommand> saveReturnService,
			[FromServices] LeePayHasher hasher,
			[FromServices] IOptions<JsonOptions> jsonOptions,
			Guid formId)
		{
			var customOrderNo = Convert.ToBase64String(formId.ToByteArray());

			var depositForm = await getDepositFormService
				.GetAsync(new GetDepositFormQuery(customOrderNo))
				.ConfigureAwait(false);

			var fakeViewModel = new DepositResponseViewModel
			{
				CustomOrderNo = customOrderNo,
				OrderNo = "WG-050714-cca518-A",
				Uid = "test-wallet3",
				Status = "MappingSuccess",
				OrderAmount = depositForm.Amount,
				FeeRate = 0.009000M,
				Fee = 0.891000M,
				ToAddress = "badfafdsagagdfadfgasfdasfdsa",
				TransactionHash = "3bb8e040df2fd86bc7acc94d4fa37fbf8c675562d877bb3d698eceec25cca518", //DevSkim: ignore DS173237
				CreatedAt = DateTimeOffset.UtcNow,
				Channel = "TRON",
				ActualAmount = depositForm.Amount.ToString()
			};

			var jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;

			var json = JsonSerializer.Serialize(fakeViewModel, jsonSerializerOptions);

			var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;
			dict.Remove("txnSign");

			var txnSign = hasher.ComputeHash(
				depositForm.ApplyInfo!.Settings.SystemId,
				depositForm.ApplyInfo!.Settings.SystemKey,
				dict);

			dict.Add("txnSign", txnSign);

			await DepositCallbackAsync(
				jsonOptions,
				getDepositFormService,
				checkSigningCodeService,
				saveReturnService,
				dict).ConfigureAwait(false);
		}
	}
}
