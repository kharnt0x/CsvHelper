using System.Collections.Generic;
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
	public class CsvReaderSubClassingTests
	{
		[TestMethod]
		public void GetRecordTest()
		{
			var data = new List<string[]>
			{
				new[] { "Id", "Name" },
				new[] { "1", "one" },
				new[] { "2", "two" },
				null
			};

			var parserMock = new ParserMock( new Queue<string[]>( data ) );

			var csvReader = new MyCsvReader( parserMock );
			csvReader.GetRecords<Test>().ToList();
		}

		private class MyCsvReader : CsvReader
		{
			public MyCsvReader( ICsvParser parser ) : base( parser ){}
		}

		private class Test
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}
	}
}
