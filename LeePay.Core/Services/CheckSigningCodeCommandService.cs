using System;
using System.Threading;
using System.Threading.Tasks;
using TGS.ServiceUtility;

namespace Tgs.Payment.LeePay.Services
{
	internal class CheckSigningCodeCommandService : ICommandService<CheckSigningCodeCommand>
	{
		private readonly LeePayHasher _hasher;

		public CheckSigningCodeCommandService(LeePayHasher hasher)
		{
			_hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
		}

		public ValueTask ExecuteAsync(
			CheckSigningCodeCommand command,
			CancellationToken cancellationToken = default)
		{
			var signCode = _hasher.ComputeHash(command.SystemId, command.SystemKey, command.Data);

			command.CommandResultSource.SetResult(signCode == command.SigningCode);

			return ValueTask.CompletedTask;
		}
	}
}
