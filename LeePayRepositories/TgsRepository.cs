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
				data.Id,
				data.PlayerCode,
				data.BrandId,
				data.BrandPaymentId,
				data.Amount,
				data.Status,
				_serializer.Deserialize<LeePayDepositApplyInfo>(data.ApplyInfo),
				data.PaymentReturn);
		}

		public async ValueTask<WithdrawFormInfo> GetWithdrawFormByIdAsync(Guid id)
		{
			var data = await _dbContext.WithdrawForms.FindAsync(id).ConfigureAwait(false);

			return new WithdrawFormInfo(
				data.PlayerCode,
				data.BrandId,
				data.BrandPaymentId,
				data.Amount,
				data.Status,
				data.BankInfoId,
				data.BankInfo,
				_serializer.Deserialize<LeePayWithdrawApplyInfo>(data.ApplyInfo),
				data.PaymentReturn);
		}

		public async ValueTask SaveDepositReturnAsync(Guid id, LeePayDepositReturn depositReturn)
		{
			var form = await _dbContext.DepositForms
				.FindAsync(id)
				.ConfigureAwait(false);

			form.PaymentReturn = _serializer.Serialize(depositReturn);

			await _dbContext.SaveChangesAsync().ConfigureAwait(false);
		}
	}
}
