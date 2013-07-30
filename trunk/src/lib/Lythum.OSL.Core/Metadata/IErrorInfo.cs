/*
 * Created by SharpDevelop.
 * User: Arvydas Grigonis, (C) www.lythum.lt
 * Date: 2013.07.30
 * Time: 18:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lythum.OSL.Core.Metadata
{
	/// <summary>
	/// Description of IErrorInfo.
	/// </summary>
	public interface IErrorInfo : ILythumOSLBase
	{
		bool HasError { get; }
		Exception Exception { get; }
		string ErrorText { get; }
		
		/// <summary>
		/// Resets error state
		/// </summary>
		void Error ();
		/// <summary>
		/// Initialized error state with error message
		/// </summary>
		/// <param name="errorMsg"></param>
		void Error (string errorText);
		
		/// <summary>
		/// Initializes error state with exception
		/// </summary>
		/// <param name="ex"></param>
		void Error (Exception ex);
	}
}
