using System.Text.Json.Serialization;

namespace Tgs.Payment.LeePay;

internal class DepositOrder
{
	public decimal Amount { get; set; }

	public string? Channel { get; set; }

	public string ChannelCurrency { get; set; } = default!;

	[JsonPropertyName("customMsg")]
	public string? CustomMessage { get; set; }

	public string CustomOrderNo { get; set; } = default!;

	public int? DepositLimitTime { get; set; }

	public string? NotifyCustomMessage { get; set; }

	public string? NotifyUrl { get; set; }

	public string? ReturnUrl { get; set; }

	[JsonPropertyName("txnSign")]
	public string? TranscationSigningCode { get; set; }
}
