using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Lythum.OSL.Core.Errors;
using Lythum.OSL.Core.Metadata;

namespace Lythum.OSL.Core.Progress
{
	public abstract class ProgressUnit : ErrorInfo, IProgressUnit
	{
		#region ctor
		public ProgressUnit ()
		{
			Cancel = false;
			Message = string.Empty;
			State = ProgressState.Pristine;
			Total = 100;
			Processed = 0;
		}

		#endregion


		#region IProgressUnit Members

		public ProgressState State { get; set; }
		public string Message { get; set; }
		public virtual int Total { get; set; }
		public virtual int Processed { get; set; }
		public bool Cancel { get; set; }
		public bool CanCancel { get; protected set; }

		public abstract void Start ();


		#endregion
	}
}
