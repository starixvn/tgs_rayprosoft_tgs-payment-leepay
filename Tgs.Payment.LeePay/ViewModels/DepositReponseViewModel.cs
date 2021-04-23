using System;
using System.Text.Json.Serialization;

namespace Tgs.Payment.LeePay.ViewModels
{
	public record DepositReponseViewModel
	{
		public string CustomOrderNo { get; set; }

		[JsonPropertyName("notifyCustomMsg")]
		public string NotifyCustomMessage { get; set; }

		public string OrderNo { get; set; }

		public string Uid { get; set; }

		public string Status { get; set; }

		public decimal OrderAmount { get; set; }

		public decimal FeeRate { get; set; }

		public decimal Fee { get; set; }

		public string ToAddress { get; set; }

		public string TransactionHash { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public string Channel { get; set; }

		public string ActualAmount { get; set; }

		[JsonPropertyName("txnSign")]
		public string TranscationSigningCode { get; set; }
	}
}
