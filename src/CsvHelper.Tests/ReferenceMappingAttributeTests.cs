// Copyright 2009-2011 Josh Close
// This file is a part of CsvHelper and is licensed under the MS-PL
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html
// http://csvhelper.com

using System.Collections.Generic;
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
	public class ReferenceMappingAttributeTests
	{
		[TestMethod]
		public void ReferenceMappingTest()
		{
			var queue = new Queue<string[]>();
			queue.Enqueue(new[]
			{
						"FirstName",
						"LastName",
						"HomeStreet",
						"HomeCity",
						"HomeState",
						"HomeZip",
						"WorkStreet",
						"WorkCity",
						"WorkState",
						"WorkZip"
			} );
			queue.Enqueue( new[]
			{
				"John",
				"Doe",
				"1234 Home St",
				"Home Town",
				"Home State",
				"12345",
				"5678 Work Rd",
				"Work City",
				"Work State",
				"67890"
			} );

			var parserMock = new ParserMock( queue );

			var reader = new CsvReader( parserMock );
			reader.Read();
			var person = reader.GetRecord<Person>();

			Assert.AreEqual( "FirstName", person.FirstName );
			Assert.AreEqual( "LastName", person.LastName );
			Assert.AreEqual( "HomeStreet", person.HomeAddress.Street );
			Assert.AreEqual( "HomeCity", person.HomeAddress.City );
			Assert.AreEqual( "HomeState", person.HomeAddress.State );
			Assert.AreEqual( "HomeZip", person.HomeAddress.Zip );
			Assert.AreEqual( "WorkStreet", person.WorkAddress.Street );
			Assert.AreEqual( "WorkCity", person.WorkAddress.City );
			Assert.AreEqual( "WorkState", person.WorkAddress.State );
			Assert.AreEqual( "WorkZip", person.WorkAddress.Zip );
		}

		private class Person
		{
			[CsvField( Name = "FirstName" )]
			public string FirstName { get; set; }

			[CsvField( Name = "LastName" )]
			public string LastName { get; set; }

			[CsvField( ReferenceKey = "Home" )]
			public Address HomeAddress { get; set; }

			[CsvField( ReferenceKey = "Work" )]
			public Address WorkAddress { get; set; }
		}

		private class Address
		{
			[CsvField( ReferenceKey = "Home", Name = "HomeStreet" )]
			[CsvField( ReferenceKey = "Work", Name = "WorkStreet" )]
			public string Street { get; set; }

			[CsvField( ReferenceKey = "Home", Name = "HomeCity" )]
			[CsvField( ReferenceKey = "Work", Name = "WorkCity" )]
			public string City { get; set; }

			[CsvField( ReferenceKey = "Home", Name = "HomeState" )]
			[CsvField( ReferenceKey = "Work", Name = "WorkState" )]
			public string State { get; set; }

			[CsvField( ReferenceKey = "Home", Name = "HomeZip" )]
			[CsvField( ReferenceKey = "Work", Name = "WorkZip" )]
			public string Zip { get; set; }
		}
	}
}
