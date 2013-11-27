using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lythum.OSL.Core.Data.Queries
{
	public class Conditions
	{
		/// <summary>
		/// Query condition is
		/// </summary>
		/// <param name="field">table field</param>
		/// <param name="value">table field filter value</param>
		/// <param name="conditionType">defines condition type, equal, not equal, more than and etc.</param>
		/// <param name="allowNulls">do allow nulls</param>
		/// <returns>query condition</returns>
		public static string ConditionIs(
			string field,
			string value,
			ConditionType conditionType,
			bool allowNulls)
		{
			if (allowNulls)
			{
				return "(" + field + GetConditionSign(conditionType) + value + " OR " + ConditionIsNull(field) + ")";
			}
			else
			{
				return field + GetConditionSign(conditionType) + value;
			}
		}

		public static string ConditionIsNull(string field)
		{
			return field + " IS NULL";
		}

		/// <summary>
		/// Simplified only for equal conditions
		/// </summary>
		/// <param name="field"></param>
		/// <param name="value"></param>
		/// <param name="allowNulls"></param>
		/// <returns></returns>
		public static string ConditionIsEqual(
			string field,
			string value,
			bool allowNulls)
		{
			return ConditionIs(field, value, ConditionType.Equal, allowNulls);
		}

		/// <summary>
		/// Subquery condition processing
		/// </summary>
		/// <param name="field">table field</param>
		/// <param name="query">match subquery</param>
		/// <param name="allowNulls">table field allows nulls</param>
		/// <param name="isInCondition">is IN or NOT IN subquery condition, true is IN</param>
		/// <returns>query condition</returns>
		public static string ConditionSubQuery(
			string field,
			SelectQuery query,
			bool allowNulls,
			bool isInCondition)
		{
			string subquery = field + " " + (isInCondition ? "IN" : "NOT IN") + " (" + query.RenderString() + ")";

			if (allowNulls)
			{
				return "(" + subquery + " OR " + ConditionIsNull(field) + ")";
			}
			else
			{
				return subquery;
			}
		}

		/// <summary>
		/// Only for IN subqueries, not for NOT IN ones
		/// </summary>
		/// <param name="field">table field</param>
		/// <param name="query">match subquery</param>
		/// <param name="allowNulls">table field allows nulls</param>
		/// <returns>query condition</returns>
		public static string ConditionSubQuery(
			string field,
			SelectQuery query,
			bool allowNulls)
		{
			return ConditionSubQuery(field, query, allowNulls, true);
		}

		/// <summary>
		/// Returns condition symbols by condition enumeration
		/// </summary>
		/// <param name="conditionType"></param>
		/// <returns></returns>
		public static string GetConditionSign(ConditionType conditionType)
		{
			switch (conditionType)
			{
				default:
				case ConditionType.Equal:
					return " = ";
				case ConditionType.NotEqual:
					return " <> ";
				case ConditionType.LessThan:
					return " > ";
				case ConditionType.MoreThan:
					return " < ";
				case ConditionType.LessOrEqual:
					return " >= ";
				case ConditionType.MoreOrEqual:
					return " <= ";

			}
		}
	}
}
