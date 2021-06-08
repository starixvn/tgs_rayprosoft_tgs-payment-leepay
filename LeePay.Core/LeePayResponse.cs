namespace Tgs.Payment.LeePay
{
	internal record LeePayResponse<TData>
	{
		public string Code { get; init; } = default!;

		public string Message { get; init; } = default!;

		public TData? Data { get; init; }

		public string? OriginalResponse { get; set; }

		public string? ClientCode { get; set; }

		public string? TraceId { get; set; }

		public string? DisplayMessage { get; set; }

		public string? Timestamp { get; set; }

		public string? Path { get; set; }
	}
}