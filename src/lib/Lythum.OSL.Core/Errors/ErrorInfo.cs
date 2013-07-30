/*
 * Created by SharpDevelop.
 * User: Arvydas Grigonis, (C) www.lythum.lt
 * Date: 2013.07.30
 * Time: 18:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

using Lythum.OSL.Core.Metadata;

namespace Lythum.OSL.Core.Errors
{
	/// <summary>
	/// Description of ErrorInfo.
	/// </summary>
	public class ErrorInfo : IErrorInfo
	{
		public ErrorInfo()
		{
			Error();
		}
		
		#region IErrorInfo

		public bool HasError 
		{
			get 
			{
				return !string.IsNullOrEmpty(ErrorText) ||
					Exception != null;
			}
		}
		
		public Exception Exception { get; protected set; }
		
		public string ErrorText { get; protected set; }
		
		public void Error()
		{
			Exception = null;
			ErrorText = string.Empty;
		}
		
		public void Error(string errorText)
		{
			Error();
			
			ErrorText = errorText;
		}
		
		public void Error(Exception ex)
		{
			Error();
			
			Exception = ex;
		}

		#endregion
	}
}
