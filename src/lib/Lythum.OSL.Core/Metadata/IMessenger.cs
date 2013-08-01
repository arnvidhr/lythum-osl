/*
 * Created by SharpDevelop.
 * User: Arvydas Grigonis, (C) www.lythum.lt
 * Date: 2013.08.01
 * Time: 17:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lythum.OSL.Core.Metadata
{
	/// <summary>
	/// Description of IMessenger.
	/// </summary>
	public interface IMessenger : ILythumOSLBase
	{
		void Warning (string msg);
		void Error (string msg);
		bool Question (string msg);
	}
}
