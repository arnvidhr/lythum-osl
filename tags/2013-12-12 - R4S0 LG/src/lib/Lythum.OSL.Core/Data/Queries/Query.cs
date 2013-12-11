using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Lythum.OSL.Core.Metadata;

namespace Lythum.OSL.Core.Data.Queries
{
	public abstract class Query : IStringRender
	{

		public override string ToString()
		{
			return RenderString();
		}

		#region IStringRender Members

		public abstract string RenderString();

		#endregion
	}
}
