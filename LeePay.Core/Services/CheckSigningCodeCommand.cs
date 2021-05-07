using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tgs.Payment.LeePay.Services
{
	public record CheckSigningCodeCommand(
		IDictionary<string, object> Data,
		string SigningCode,
		string SystemId,
		string SystemKey)
	{
		public TaskCompletionSource<bool> CommandResultSource { get; } = new TaskCompletionSource<bool>();
	};
}
