using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lythum.OSL.Core.Metadata
{
	public interface IDbTableField
	{
		/// <summary>
		/// Field name
		/// </summary>
		string Name { get; set; }
		/// <summary>
		/// Field type
		/// </summary>
		string Type { get; set; }
		/// <summary>
		/// Field alias, used for XML serialization to shortcut XML tags.
		/// Normally this field is automatically initialized on SQLiteTable creation 
		/// and don't require to manual initialization.
		/// </summary>
		string Alias { get; set; }
		/// <summary>
		/// Primary key
		/// </summary>
		bool PrimaryKey { get; set; }
		/// <summary>
		/// Auto increment if possible
		/// </summary>
		bool AutoIncrement { get; set; }
		/// <summary>
		/// Indexed field
		/// </summary>
		bool Index { get; set; }
		/// <summary>
		/// Default value as string
		/// </summary>
		string DefaultValue { get; set; }
	}
}
