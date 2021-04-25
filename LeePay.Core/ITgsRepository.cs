using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.Payment.LeePay
{
	public interface ITgsRepository
	{
		ValueTask<DepositFormInfo> GetDepositFormByIdAsync(Guid id);
	}
}
