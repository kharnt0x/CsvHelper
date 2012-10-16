using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;
using CsvHelper.Tests.Mocks;
#if WINRT_4_5
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CsvHelper.Tests
{
	[TestClass]
	public class CsvReaderMappingTests
	{
		[TestMethod]
		public void ReadMultipleNamesTest()
		{
			var data = new List<string[]>
			{
				new[] { "int2", "string3" },
				new[] { "1", "one" },
				new[] { "2", "two" },
				null
			};

			var queue = new Queue<string[]>( data );
			var parserMock = new ParserMock( queue );

			var csvReader = new CsvReader( parserMock );
			csvReader.Configuration.AttributeMapping<MultipleNamesAttributeClass>();

			var records = csvReader.GetRecords<MultipleNamesAttributeClass>().ToList();

			Assert.IsNotNull( records );
			Assert.AreEqual( 2, records.Count );
			Assert.AreEqual( 1, records[0].IntColumn );
			Assert.AreEqual( "one", records[0].StringColumn );
			Assert.AreEqual( 2, records[1].IntColumn );
			Assert.AreEqual( "two", records[1].StringColumn );
		}

		private class MultipleNamesAttributeClass
		{
			[CsvField( Names = new[] { "int1", "int2", "int3" } )]
			public int IntColumn { get; set; }

			[CsvField( Names = new[] { "string1", "string2", "string3" } )]
			public string StringColumn { get; set; }
		}
	}
}
