namespace Tgs.Payment.LeePay.ViewModels;

public record WithdrawConfirmViewModel
{
	public string Code { get; init; } = default!;

	public string Message { get; init; } = default!;
}
