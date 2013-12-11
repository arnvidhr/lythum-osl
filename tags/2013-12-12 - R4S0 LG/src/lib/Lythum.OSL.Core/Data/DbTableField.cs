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
		public virtual string Name { get; set; }
		/// <summary>
		/// Field type
		/// </summary>
		public virtual string Type { get; set; }
		/// <summary>
		/// Field alias, used for XML serialization to shortcut XML tags.
		/// Normally this field is automatically initialized on SQLiteTable creation 
		/// and don't require to manual initialization.
		/// </summary>
		public virtual string Alias { get; set; }
		/// <summary>
		/// PK
		/// </summary>
		public virtual bool PrimaryKey { get; set; }
		/// <summary>
		/// Auto increment
		/// </summary>
		public virtual bool AutoIncrement { get; set; }
		/// <summary>
		/// Indexed db field
		/// </summary>
		public virtual bool Index { get; set; }
		/// <summary>
		/// Default value
		/// </summary>
		public virtual string DefaultValue { get; set; }

		public DbTableField()
		{
			this.Name = string.Empty;
			this.Type = string.Empty;
			this.Alias = string.Empty;
			this.PrimaryKey = false;
			this.AutoIncrement = false;
			this.Index = false;
			this.DefaultValue = string.Empty;
		}
	}
}
