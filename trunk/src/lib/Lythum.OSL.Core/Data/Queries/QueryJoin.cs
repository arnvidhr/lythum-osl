using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lythum.OSL.Core.Data.Queries
{
	public class QueryJoin
	{
		public string TableName;
		public QueryJoinType JoinType;
		public List<string> JoinConditions;

		public QueryJoin()
		{
			this.JoinConditions = new List<string>();
		}

		public QueryJoin(
			string tableName,
			QueryJoinType joinType,
			string[] joinConditions)
			: this()
		{
			this.TableName = tableName;
			this.JoinType = joinType;
			this.JoinConditions.AddRange(joinConditions);
		}

	}
}
