﻿/*
 * Created by SharpDevelop.
 * User: Arvydas Grigonis, (C) www.lythum.lt
 * Date: 2013.08.07
 * Time: 14:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Lythum.OSL.Core.Progress;

namespace Lythum.OSL.Core.Metadata
{
	/// <summary>
	/// Description of IProgressUnit.
	/// </summary>
	public interface IProgressUnit : ILythumOSLBase
	{
		/// <summary>
		/// Progress state, default: Pristine
		/// </summary>
		ProgressState State { get; }
		/// <summary>
		/// Current process name
		/// </summary>
		string Message { get; }
		/// <summary>
		/// Total units to process
		/// </summary>
		int Total { get; }
		/// <summary>
		/// Total units processed
		/// </summary>
		int Processed { get; }

		/// <summary>
		/// Progress process
		/// </summary>
		/// <returns></returns>
		bool Start ();

		/// <summary>
		/// True if needs to cancel
		/// </summary>
		bool Cancel { get; set; }
	}
}
