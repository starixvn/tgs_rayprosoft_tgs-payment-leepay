using System;

namespace Tgs.Payment.LeePay
{
	public class LeePayOptions
	{
		public string NatsUrl { get; set; } = default!;

		public Uri PaymentApiUrl { get; set; } = default!;
	}
}
