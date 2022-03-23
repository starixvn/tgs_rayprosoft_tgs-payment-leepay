namespace Tgs.Payment.LeePay;

public record LeePayQueryExchangeRateResponse(
	string Code,
	decimal? BuyPrice = null,
	decimal? SellPrice = null,
	string? BizTime = null,
	string? ClientCode = null,
	string? Message = null,
	string? ResponseContent = null);
