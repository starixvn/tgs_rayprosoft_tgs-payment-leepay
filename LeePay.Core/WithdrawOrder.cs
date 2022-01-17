using System.Text.Json.Serialization;

namespace Tgs.Payment.LeePay;

internal class WithdrawOrder
{
	public decimal Amount { get; set; }

	public string? Channel { get; set; }

	public string ChannelCurrency { get; set; } = default!;

	public string CustomOrderNo { get; set; } = default!;

	[JsonPropertyName("destAddress")]
	public string DestinationAddress { get; set; } = default!;

	public string? NotifyCustomMsg { get; set; }

	public string? NotifyUrl { get; set; }

	[JsonPropertyName("txnSign")]
	public string TranscationSigningCode { get; set; } = default!;
}
