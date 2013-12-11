/*
 * Created by SharpDevelop.
 * User: Arvydas Grigonis, (C) www.lythum.lt
 * Date: 2013.07.30
 * Time: 18:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lythum.OSL.Core.Errors
{
	/// <summary>
	/// Description of Validation.
	/// </summary>
	public class Validation
	{
		
		/// <summary>
		/// Validate do object is not null
		/// </summary>
		/// <param name="parameter"></param>
		/// <param name="parameterName"></param>
		public static void RequireValid<T>(T parameter, string parameterName)
		{
			if(parameter == null)
			{
#warning TODO: ML
				throw new LythumException(string.Format(
					"Object '{0}' is not valid!",
					parameterName));
			}
		}
		

		public static void RequireValidString(string parameter, string parameterName)
		{
			if (string.IsNullOrEmpty(parameter))
			{
#warning TODO: ML
				throw new LythumException(string.Format(
					"String '{0}' is not valid!",
					parameterName));
			}
		}

	}
}
