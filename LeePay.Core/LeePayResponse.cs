namespace Tgs.Payment.LeePay
{
	internal record LeePayResponse<TData>
	{
		public string Code { get; init; } = default!;

		public string Message { get; init; } = default!;

		public TData? Data { get; init; }

		public string? OriginalResponse { get; set; }
	}
}