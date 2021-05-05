namespace Tgs.Payment.LeePay.ViewModels
{
	public record DepositConfirmViewModel
	{
		public string Code { get; init; } = default!;

		public string Message { get; init; } = default!;
	}
}
