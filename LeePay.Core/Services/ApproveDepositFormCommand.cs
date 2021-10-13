using System;

namespace Tgs.Payment.LeePay.Services
{
	public record ApproveDepositFormCommand(Guid FormId, Guid BrandId, decimal? ActualAmount);
}
