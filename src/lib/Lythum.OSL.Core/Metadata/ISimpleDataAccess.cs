/*
 * Created by SharpDevelop.
 * User: Arvydas Grigonis, (C) www.lythum.lt
 * Date: 2013.07.30
 * Time: 18:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;

namespace Lythum.OSL.Core.Metadata
{
	/// <summary>
	/// Description of ISimpleDataAccess.
	/// </summary>
	public interface ISimpleDataAccess : ILythumOSLBase
	{
		DataTable Query (string sql);
		void Execute (string sql);
		string QueryScalar (string sql);
	}
}
