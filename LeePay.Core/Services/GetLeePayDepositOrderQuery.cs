namespace Tgs.Payment.LeePay.Services
{
	public record GetLeePayDepositOrderQuery(
		string SystemId,
		string SystemKey,
		string OrderNo,
		string CustomOrderNo);
}
