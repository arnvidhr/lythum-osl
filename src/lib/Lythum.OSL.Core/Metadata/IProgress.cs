/*
 * Created by SharpDevelop.
 * User: Arvydas Grigonis, (C) www.lythum.lt
 * Date: 2013.08.01
 * Time: 17:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lythum.OSL.Core.Metadata
{
	/// <summary>
	/// Description of IProgress.
	/// </summary>
	public interface IProgress : ILythumOSLBase
	{
		int Minimum { get; set; }
		int Maximum { get; set; }
		int Value { get; set; }
		void Update ();
	}
}
