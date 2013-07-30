/*
 * Created by SharpDevelop.
 * User: Arvydas Grigonis, (C) www.lythum.lt
 * Date: 2013.07.30
 * Time: 18:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lythum.OSL.Core.Errors
{
	/// <summary>
	/// Description of LythumException.
	/// </summary>
	public class LythumException : Exception
	{
		public LythumException (string message)
			: base (message)
		{
		}

		public LythumException (string message, Exception innerException)
			: base (message, innerException)
		{

		}

		public LythumException (Exception innerException)
			: base (innerException.Message, innerException)
		{
		}

	}
}
