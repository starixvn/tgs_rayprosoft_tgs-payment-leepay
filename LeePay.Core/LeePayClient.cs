using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Tgs.Payment.LeePay
{
	internal class LeePayClient
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions? _jsonSerializerOptions;
		private readonly ILogger<LeePayClient> _logger;

		public LeePayClient(HttpClient httpClient, JsonSerializerOptions? jsonSerializerOptions, ILogger<LeePayClient> logger)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_jsonSerializerOptions = jsonSerializerOptions;
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

			return result!;
		}

		private string SerializeObject(DepositOrder order)
			=> JsonSerializer.Serialize(order, _jsonSerializerOptions);

		private string ComputeHash(string systemId, string systemKey, DepositOrder order)
		{
			var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(SerializeObject(order))!;

			var content = string.Join("&", dict.OrderBy(p => p.Key).Select(p => $"{p.Key}={p.Value}"));

			var fullText = $"SYSTEM_ID={systemId}&{content}&SYSTEM_KEY={systemKey}";

			var base64Code = Convert.ToBase64String(Encoding.UTF8.GetBytes(fullText));

			var hash = string.Join(
				string.Empty,
				SHA256.HashData(Encoding.UTF8.GetBytes(base64Code)).Select(b => b.ToString("X2")));

			return hash;
		}
	}
}
