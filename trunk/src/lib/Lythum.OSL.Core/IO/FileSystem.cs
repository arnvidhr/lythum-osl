using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Lythum.OSL.Core.Errors;

namespace Lythum.OSL.Core.IO
{
	public class FileSystem
	{
		public static void DirectoryCreateIfNotExist (DirectoryInfo directoryInfo)
		{
			Validation.RequireValid (directoryInfo, "dirInfo");

			if (!directoryInfo.Exists)
			{
				directoryInfo.Create ();
			}
		}

		public static void DirectoryCreateIfNotExist (FileInfo fileInfo)
		{
			Validation.RequireValid (fileInfo, "fileInfo");

			DirectoryCreateIfNotExist (fileInfo.Directory);
		}
	}
}
