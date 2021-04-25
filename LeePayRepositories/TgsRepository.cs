using System;
using System.Threading.Tasks;
using LeePayRepositories.Entities;

namespace Tgs.Payment.LeePay.Repositories
{
	internal class TgsRepository : ITgsRepository
	{
		private readonly TgsContext _dbContext;

		public TgsRepository(TgsContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public async ValueTask<DepositFormInfo> GetDepositFormByIdAsync(Guid id)
		{
			var data = await _dbContext.DepositForms.FindAsync(id).ConfigureAwait(false);

			return new DepositFormInfo(
				data.PlayerCode,
				data.BrandId,
				data.BrandPaymentId,
				data.Amount,
				data.ApplyInfo,
				data.PaymentReturn);
		}
	}
}
