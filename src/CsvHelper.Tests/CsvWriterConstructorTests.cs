using System;
using System.IO;
using CsvHelper.Configuration;
#if WINRT_4_5
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CsvHelper.Tests
{
	[TestClass]
	public class CsvWriterConstructorTests
	{
		[TestMethod]
		public void EnsureInternalsAreSetupWhenPasingWriterAndConfigTest()
		{
			using( var stream = new MemoryStream() )
			using( var writer = new StreamWriter( stream ) )
			{
				var config = new CsvConfiguration();
				using( var csv = new CsvWriter( writer, config ) )
				{
					Assert.AreSame( config, csv.Configuration );
				}
			}
		}
	}
}
