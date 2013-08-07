﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Lythum.OSL.Core.Errors;
using Lythum.OSL.Core.Metadata;

namespace Lythum.OSL.Core.Progress
{
	public abstract class ProgressUnit : IProgressUnit
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

		public ProgressState State { get; protected set; }
		public string Message { get; protected set; }
		public int Total { get; protected set; }
		public int Processed { get; protected set; }
		public bool Cancel { get; set; }

		public abstract bool Start ();


		#endregion
	}
}
