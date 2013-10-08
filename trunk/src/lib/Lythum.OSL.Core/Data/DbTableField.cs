using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Lythum.OSL.Core.Metadata;

namespace Lythum.OSL.Core.Data
{
	public abstract class DbTableField : IDbTableField
	{
		/// <summary>
		/// Field name
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Field type
		/// </summary>
		public string Type { get; set; }
		/// <summary>
		/// Field alias, used for XML serialization to shortcut XML tags.
		/// Normally this field is automatically initialized on SQLiteTable creation 
		/// and don't require to manual initialization.
		/// </summary>
		public string Alias { get; set; }
		/// <summary>
		/// PK
		/// </summary>
		public bool PrimaryKey { get; set; }
		/// <summary>
		/// Auto increment
		/// </summary>
		public bool AutoIncrement { get; set; }
		/// <summary>
		/// Indexed db field
		/// </summary>
		public bool Index { get; set; }
	}
}
