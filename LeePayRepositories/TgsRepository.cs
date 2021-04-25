using System;
using System.Threading.Tasks;
using LeePayRepositories.Entities;

namespace Tgs.Payment.LeePay.Repositories
{
	internal class TgsRepository : ITgsRepository
	{
		private readonly TgsContext _dbContext;
		private readonly LeePaySerializer _serializer;

		public TgsRepository(TgsContext dbContext, LeePaySerializer serializer)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
		}

		public async ValueTask<DepositFormInfo> GetDepositFormByIdAsync(Guid id)
		{
			var data = await _dbContext.DepositForms.FindAsync(id).ConfigureAwait(false);

			return new DepositFormInfo(
				data.PlayerCode,
				data.BrandId,
				data.BrandPaymentId,
				data.Amount,
				_serializer.Deserialize<LeePayApplyInfo>(data.ApplyInfo),
				data.PaymentReturn);
		}
	}
}
