using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lythum.OSL.Core.Metadata
{
	public interface ISimpleDataTransaction
	{
		bool IsTransactionInProgress { get; }
		bool BeginTransaction ();
		void EndTransaction (bool commit);
	}
}
