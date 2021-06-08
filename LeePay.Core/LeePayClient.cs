using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Tgs.Payment.LeePay
{
	internal class LeePayClient : ILeePayClient
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions? _jsonSerializerOptions;
		private readonly LeePayHasher _hasher;
		private readonly ILogger<LeePayClient> _logger;

		public LeePayClient(
			HttpClient httpClient,
			JsonSerializerOptions? jsonSerializerOptions,
			LeePayHasher hasher,
			ILogger<LeePayClient> logger)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_jsonSerializerOptions = jsonSerializerOptions;
			_hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async ValueTask<LeePayResponse<DepositResult>> DepositAsync(string systemId, string systemKey, DepositOrder order)
		{
			order.TranscationSigningCode = ComputeHash(systemId, systemKey, order);

			var requestContent = new StringContent(SerializeObject(order));
			requestContent.Headers.Add("SYSTEM_ID", systemId);
			requestContent.Headers.Add("SYSTEM_KEY", systemKey);

			requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json);

			var response = await _httpClient
				.PostAsync("/payment/deposit-order/polling/place", requestContent)
				.ConfigureAwait(false);

			_logger.LogInformation($"Deposit StatusCode: {response.StatusCode}");

			var bodyJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			_logger.LogInformation($"Deposit Body: {bodyJson}");

			var result = JsonSerializer.Deserialize<LeePayResponse<DepositResult>>(bodyJson, _jsonSerializerOptions);

			result!.OriginalResponse = bodyJson;

			return result!;
		}

		public async ValueTask<LeePayResponse<WithdrawResult>> WidthdrawAsync(string systemId, string systemKey, WithdrawOrder order)
		{
			order.TranscationSigningCode = ComputeHash(systemId, systemKey, order);

			var requestContent = new StringContent(SerializeObject(order));
			requestContent.Headers.Add("SYSTEM_ID", systemId);
			requestContent.Headers.Add("SYSTEM_KEY", systemKey);

			requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json);

			var response = await _httpClient
				.PostAsync("/payment/mchw/v3/payfor", requestContent)
				.ConfigureAwait(false);

			_logger.LogInformation($"Withdraw StatusCode: {response.StatusCode}");

			var bodyJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			_logger.LogInformation($"Withdraw Body: {bodyJson}");

			var result = JsonSerializer.Deserialize<LeePayResponse<WithdrawResult>>(bodyJson, _jsonSerializerOptions);

			result!.OriginalResponse = bodyJson;

			return result!;
		}

		private string SerializeObject<TOrder>(TOrder order)
			=> JsonSerializer.Serialize(order, _jsonSerializerOptions);

		private string ComputeHash<TOrder>(string systemId, string systemKey, TOrder order)
		{
			var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(SerializeObject(order))!;

			return _hasher.ComputeHash(systemId, systemKey, dict);
		}

		public async ValueTask<LeePayResponse<ExchangeRateResult>> ExchangeRateAsync(string systemId, string systemKey, ExchangeRateOrder order)
		{
			order.TranscationSigningCode = ComputeHash(systemId, systemKey, order);

			var requestContent = new StringContent(SerializeObject(order));
			requestContent.Headers.Add("SYSTEM_ID", systemId);
			requestContent.Headers.Add("SYSTEM_KEY", systemKey);

			requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json);

			var response = await _httpClient
				.PostAsync("/rate/exchange", requestContent)
				.ConfigureAwait(false);

			_logger.LogInformation($"ExchangeRate StatusCode: {response.StatusCode}");

			var bodyJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			_logger.LogInformation($"ExchangeRate Body: {bodyJson}");

			var result = JsonSerializer.Deserialize<LeePayResponse<ExchangeRateResult>>(bodyJson, _jsonSerializerOptions);

			result!.OriginalResponse = bodyJson;

			return result!;
		}
	}
}