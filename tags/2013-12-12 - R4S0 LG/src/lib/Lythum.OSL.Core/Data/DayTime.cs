/*
 * Created by SharpDevelop.
 * User: Arvydas Grigonis, (C) www.lythum.lt
 * Date: 2013.11.27
 * Time: 17:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lythum.OSL.Core.Data
{
	/// <summary>
	/// Structure represents DayTime data type. 
	/// This data type contais one day time, which is internally stored as DaySeconds 
	/// what means seconds after midnight. 
	/// Used in some database level operations as stored int type DaySeconds. 
	/// In this way we can do math operations with time at database engine level.
	/// 
	/// WARNING! 
	/// 
	/// This is very first version of DayTime and may contain some bugs, use it at your own risk!
	/// This DayTime data type doesn't support imperial time format with PM/AM. Only 24 hour format is supported!
	/// It also not designed for serious calculations with time at .NET side, 
	/// mainly designed just to store time information in database and to represent it for user.
	/// </summary>
	public struct DayTime : IComparable<DayTime>// not yet implemented, IComparable, IConvertible, IFormattable, IEquatable<Time>
	{
		#region Const
		public const int HourToSeconds = 60 * 60;
		public const int MinuteToSeconds = 60;

		const string DefaultFormatPatternFullTime = "f";
		const string DefaultFormatPatternShortTime = "s";
		const string DefaultFormatPatternDaySeconds = "d";

		#endregion

		#region Attributes
		int _DaySeconds;

		#endregion

		#region Properties

		/// <summary>
		/// Secons from the midnight
		/// </summary>
		public int DaySeconds
		{
			get
			{
				return _DaySeconds;
			}
		}

		public int Hour
		{
			get
			{
				return _DaySeconds / HourToSeconds;
			}
		}

		public int Minute
		{
			get
			{
				return (_DaySeconds - (Hour * HourToSeconds)) / MinuteToSeconds;
			}
		}

		public int Second
		{
			get
			{
				return _DaySeconds - (Hour * HourToSeconds) - (Minute * MinuteToSeconds);
			}
		}


		#endregion

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="time">
		/// Time format can be: hh, hh:mm, hh:mm:ss
		/// As separator are accepted only : and space symbols
		/// </param>
		public DayTime(string time)
		{
			_DaySeconds = 0;

			string[] nodes = time.Split(new char[] { ':', ' ' });

			if (nodes != null)
			{
				if (nodes.Length > 0)
				{
					_DaySeconds = HourToSeconds * int.Parse(nodes[0]);
				}

				if (nodes.Length > 1)
				{
					_DaySeconds += MinuteToSeconds * int.Parse(nodes[1]);
					
				}

				if (nodes.Length > 2)
				{
					_DaySeconds += int.Parse(nodes[2]);
				}
			}
			

		}
		/// <summary>
		/// Initialize day seconds (seconds after last midnight)
		/// </summary>
		/// <param name="daySeconds"></param>
		public DayTime(int daySeconds)
		{
			_DaySeconds = daySeconds;
		}

		/// <summary>
		/// Initialize hour, minute and second
		/// </summary>
		/// <param name="hour"></param>
		/// <param name="minute"></param>
		/// <param name="second"></param>
		public DayTime(int hour, int minute, int second)
		{
			_DaySeconds = HourToSeconds * hour;
			_DaySeconds += MinuteToSeconds * minute;
			_DaySeconds += second;
		}

		/// <summary>
		/// Initialize only hour and minute
		/// </summary>
		/// <param name="hours"></param>
		/// <param name="minutes"></param>
		public DayTime(int hours, int minutes)
			: this(hours, minutes, 0)
		{
		}

		/// <summary>
		/// Initialize from DateTime
		/// </summary>
		/// <param name="dateTime"></param>
		public DayTime(DateTime dateTime)
			: this(dateTime.Hour, dateTime.Minute, dateTime.Second)
		{
		}

		#endregion

		#region Helpers

		#endregion

		#region Methods
		public override string ToString()
		{
			return ToString(DefaultFormatPatternFullTime);
		}

		/// <summary>
		/// Returns formatted as string DayTime value. 
		/// Possible format values are shown below:
		/// </summary>
		/// <param name="format">
		/// f - full time (hh:mm:ss)
		/// s - short time (hh:mm)
		/// d - daytime (database) int value
		/// </param>
		/// <returns></returns>
		public string ToString(string format)
		{
			switch (format)
			{
				default:
				case DefaultFormatPatternFullTime:

					return Hour.ToString().PadLeft(2, '0') + ":" +
						Minute.ToString().PadLeft(2, '0') + ":" +
						Second.ToString().PadLeft(2, '0');

				case DefaultFormatPatternShortTime:

					return Hour.ToString().PadLeft(2, '0') + ":" +
						Minute.ToString().PadLeft(2, '0');

				case DefaultFormatPatternDaySeconds:

					return DaySeconds.ToString();
			}

		}

		#endregion

		#region IComparable<Time> Members

		public int CompareTo(DayTime other)
		{
			return this.DaySeconds.CompareTo(other.DaySeconds);
		}

		#endregion

		#region Static
		public static DayTime Now
		{
			get
			{
				DateTime dt = DateTime.Now;
				return new DayTime(dt.Hour, dt.Minute, dt.Second);
			}
		}

		/// <summary>
		/// Convert specified datatable fields to DayTime DaySeconds int values
		/// </summary>
		/// <param name="table"></param>
		/// <param name="fields"></param>
		public static void ConvertToDayTime(DataTable table, string[] fields)
		{
			if (table != null && fields != null)
			{
				foreach (DataRow row in table.Rows)
				{
					foreach(string field in fields)
					{
						if (table.Columns.Contains(field))
						{
							if (DBNull.Value.Equals(row[field]) || row[field] == null)
							{
							}
							else
							{
								row.BeginEdit();

								DayTime dt = new DayTime(row[field].ToString());

								row[field] = dt.DaySeconds.ToString();

								row.EndEdit();
							}
						}
					}
				}
			}
		}

		#endregion

	}
}


