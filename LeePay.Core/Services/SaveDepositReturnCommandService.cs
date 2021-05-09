using System;
using System.Threading;
using System.Threading.Tasks;
using TGS.ServiceUtility;

namespace Tgs.Payment.LeePay.Services
{
	internal class SaveDepositReturnCommandService : ICommandService<SaveDepositReturnCommand>
	{
		private readonly ITgsRepository _tgsRepository;

		public SaveDepositReturnCommandService(ITgsRepository tgsRepository)
		{
			_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
		}

		public ValueTask ExecuteAsync(SaveDepositReturnCommand command, CancellationToken cancellationToken = default)
		{
			return _tgsRepository.SaveDepositReturnAsync(command.FormId, command.ReturnData);
		}
	}
}
