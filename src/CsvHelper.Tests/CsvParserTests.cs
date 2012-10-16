﻿#region License
// Copyright 2009-2011 Josh Close
// This file is a part of CsvHelper and is licensed under the MS-PL
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html
// http://csvhelper.com
#endregion
using System.IO;
using System.Text;
#if WINRT_4_5
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CsvHelper.Tests
{
	[TestClass]
	public class CsvParserTests
	{
		[TestMethod]
		public void ReadNewRecordTest()
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter( stream );
			writer.WriteLine( "one,two,three" );
			writer.WriteLine( "four,five,six" );
			writer.Flush();
			stream.Position = 0;
			var reader = new StreamReader( stream );

			var parser = new CsvParser( reader );

			var count = 0;
			while( parser.Read() != null )
			{
				count++;
			}

			Assert.AreEqual( 2, count );
		}

		[TestMethod]
		public void ReadEmptyRowsTest()
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter( stream );
			writer.WriteLine( "one,two,three" );
			writer.WriteLine( "four,five,six" );
			writer.WriteLine( ",," );
			writer.WriteLine( "" );
			writer.WriteLine( "" );
			writer.WriteLine( "seven,eight,nine" );
			writer.Flush();
			stream.Position = 0;
			var reader = new StreamReader( stream );

			var parser = new CsvParser( reader );

			var count = 0;
			while( parser.Read() != null )
			{
				count++;
			}

			Assert.AreEqual( 4, count );
		}

		[TestMethod]
		public void ReadTest()
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter( stream );
			writer.WriteLine( "one,two,three" );
			writer.WriteLine( "four,five,six" );
			writer.Flush();
			stream.Position = 0;
			var reader = new StreamReader( stream );

			var parser = new CsvParser( reader );

			var record = parser.Read();
			Assert.AreEqual( "one", record[0] );
			Assert.AreEqual( "two", record[1] );
			Assert.AreEqual( "three", record[2] );

			record = parser.Read();
			Assert.AreEqual( "four", record[0] );
			Assert.AreEqual( "five", record[1] );
			Assert.AreEqual( "six", record[2] );

			record = parser.Read();
			Assert.IsNull( record );
		}

		[TestMethod]
		public void ReadFieldQuotesTest()
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter( stream );
			writer.WriteLine( "one,\"two\",three" );
			writer.WriteLine( "four,\"\"\"five\"\"\",six" );
			writer.Flush();
			stream.Position = 0;
			var reader = new StreamReader( stream );

			var parser = new CsvParser( reader ) { Configuration = { BufferSize = 2000 } };

			var record = parser.Read();
			Assert.AreEqual( "one", record[0] );
			Assert.AreEqual( "two", record[1] );
			Assert.AreEqual( "three", record[2] );

			record = parser.Read();
			Assert.AreEqual( "four", record[0] );
			Assert.AreEqual( "\"five\"", record[1] );
			Assert.AreEqual( "six", record[2] );

			record = parser.Read();
			Assert.IsNull( record );
		}

		[TestMethod]
		public void ReadSpacesTest()
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter( stream );
			writer.WriteLine( " one , \"two three\" , four " );
			writer.WriteLine( " \" five \"\" six \"\" seven \" " );
			writer.Flush();
			stream.Position = 0;
			var reader = new StreamReader( stream );

			var parser = new CsvParser( reader ) { Configuration = { BufferSize = 2 } };

			var record = parser.Read();
			Assert.AreEqual( " one ", record[0] );
			Assert.AreEqual( " two three ", record[1] );
			Assert.AreEqual( " four ", record[2] );

			record = parser.Read();
			Assert.AreEqual( "  five \" six \" seven  ", record[0] );

			record = parser.Read();
			Assert.IsNull( record );
		}

		[TestMethod]
		public void CallingReadMultipleTimesAfterDoneReadingTest()
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter( stream );
			writer.WriteLine( "one,two,three" );
			writer.WriteLine( "four,five,six" );
			writer.Flush();
			stream.Position = 0;
			var reader = new StreamReader( stream );

			var parser = new CsvParser( reader );

			parser.Read();
			parser.Read();
			parser.Read();
			parser.Read();
		}

		[TestMethod]
		public void ParseEmptyTest()
		{
			using( var memoryStream = new MemoryStream() )
			using( var streamReader = new StreamReader( memoryStream ) )
			using( var parser = new CsvParser( streamReader ) )
			{
				var record = parser.Read();
				Assert.IsNull( record );
			}
		}

		[TestMethod]
		public void ParseCrOnlyTest()
		{
			using( var stream = new MemoryStream() )
			using( var reader = new StreamReader( stream ) )
			using( var writer = new StreamWriter( stream ) )
			using( var parser = new CsvParser( reader ) )
			{
				writer.Write( "\r" );
				writer.Flush();
				stream.Position = 0;

				var record = parser.Read();
				Assert.IsNull( record );
			}
		}

		[TestMethod]
		public void ParseLfOnlyTest()
		{
			using (var stream = new MemoryStream())
			using (var reader = new StreamReader(stream))
			using (var writer = new StreamWriter(stream))
			using (var parser = new CsvParser(reader))
			{
				writer.Write("\n");
				writer.Flush();
				stream.Position = 0;

				var record = parser.Read();
				Assert.IsNull(record);
			}
		}

		[TestMethod]
		public void ParseCrLnOnlyTest()
		{
			using (var stream = new MemoryStream())
			using (var reader = new StreamReader(stream))
			using (var writer = new StreamWriter(stream))
			using (var parser = new CsvParser(reader))
			{
				writer.Write("\r\n");
				writer.Flush();
				stream.Position = 0;

				var record = parser.Read();
				Assert.IsNull(record);
			}
		}

		[TestMethod]
		public void Parse1RecordWithNoCrlfTest()
		{
			using( var memoryStream = new MemoryStream() )
			using( var streamReader = new StreamReader( memoryStream ) )
			using( var streamWriter = new StreamWriter( memoryStream ) )
			using( var parser = new CsvParser( streamReader ) )
			{
				streamWriter.Write( "one,two,three" );
				streamWriter.Flush();
				memoryStream.Position = 0;

				var record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 3, record.Length );
				Assert.AreEqual( "one", record[0] );
				Assert.AreEqual( "two", record[1] );
				Assert.AreEqual( "three", record[2] );
			}
		}

		[TestMethod]
		public void Parse2RecordsLastWithNoCrlfTest()
		{
			using( var memoryStream = new MemoryStream() )
			using( var streamReader = new StreamReader( memoryStream ) )
			using( var streamWriter = new StreamWriter( memoryStream ) )
			using( var parser = new CsvParser( streamReader ) )
			{
				streamWriter.WriteLine( "one,two,three" );
				streamWriter.Write( "four,five,six" );
				streamWriter.Flush();
				memoryStream.Position = 0;

				parser.Read();
				var record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 3, record.Length );
				Assert.AreEqual( "four", record[0] );
				Assert.AreEqual( "five", record[1] );
				Assert.AreEqual( "six", record[2] );
			}
		}

		[TestMethod]
		public void ParseFirstFieldIsEmptyQuotedTest()
		{
			using( var memoryStream = new MemoryStream() )
			using( var streamReader = new StreamReader( memoryStream ) )
			using( var streamWriter = new StreamWriter( memoryStream ) )
			using( var parser = new CsvParser( streamReader ) )
			{
				streamWriter.WriteLine( "\"\",\"two\",\"three\"" );
				streamWriter.Flush();
				memoryStream.Position = 0;

				var record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 3, record.Length );
				Assert.AreEqual( "", record[0] );
				Assert.AreEqual( "two", record[1] );
				Assert.AreEqual( "three", record[2] );
			}
		}

		[TestMethod]
		public void ParseLastFieldIsEmptyQuotedTest()
		{
			using( var memoryStream = new MemoryStream() )
			using( var streamReader = new StreamReader( memoryStream ) )
			using( var streamWriter = new StreamWriter( memoryStream ) )
			using( var parser = new CsvParser( streamReader ) )
			{
				streamWriter.WriteLine( "\"one\",\"two\",\"\"" );
				streamWriter.Flush();
				memoryStream.Position = 0;

				var record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 3, record.Length );
				Assert.AreEqual( "one", record[0] );
				Assert.AreEqual( "two", record[1] );
				Assert.AreEqual( "", record[2] );
			}
		}

		[TestMethod]
		public void ParseQuoteOnlyQuotedFieldTest()
		{
			using( var memoryStream = new MemoryStream() )
			using( var streamReader = new StreamReader( memoryStream ) )
			using( var streamWriter = new StreamWriter( memoryStream ) )
			using( var parser = new CsvParser( streamReader ) )
			{
				streamWriter.WriteLine( "\"\"\"\",\"two\",\"three\"" );
				streamWriter.Flush();
				memoryStream.Position = 0;

				var record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 3, record.Length );
				Assert.AreEqual( "\"", record[0] );
				Assert.AreEqual( "two", record[1] );
				Assert.AreEqual( "three", record[2] );
			}
		}

		[TestMethod]
		public void ParseRecordsWithOnlyOneField()
		{
			using( var memoryStream = new MemoryStream() )
			using( var streamReader = new StreamReader( memoryStream ) )
			using( var streamWriter = new StreamWriter( memoryStream ) )
			using( var parser = new CsvParser( streamReader ) )
			{
				streamWriter.WriteLine( "row one" );
				streamWriter.WriteLine( "row two" );
				streamWriter.WriteLine( "row three" );
				streamWriter.Flush();
				memoryStream.Position = 0;

				var record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 1, record.Length );
				Assert.AreEqual( "row one", record[0] );

				record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 1, record.Length );
				Assert.AreEqual( "row two", record[0] );

				record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 1, record.Length );
				Assert.AreEqual( "row three", record[0] );
			}
		}

		[TestMethod]
		public void ParseRecordWhereOnlyCarriageReturnLineEndingIsUsed()
		{
			using( var memoryStream = new MemoryStream() )
			using( var streamReader = new StreamReader( memoryStream ) )
			using( var streamWriter = new StreamWriter( memoryStream ) )
			using( var parser = new CsvParser( streamReader ) )
			{
				streamWriter.Write( "one,two\r" );
				streamWriter.Write( "three,four\r" );
				streamWriter.Write( "five,six\r" );
				streamWriter.Flush();
				memoryStream.Position = 0;

				var record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 2, record.Length );
				Assert.AreEqual( "one", record[0] );
				Assert.AreEqual( "two", record[1] );

				record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 2, record.Length );
				Assert.AreEqual( "three", record[0] );
				Assert.AreEqual( "four", record[1] );

				record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 2, record.Length );
				Assert.AreEqual( "five", record[0] );
				Assert.AreEqual( "six", record[1] );
			}
		}

		[TestMethod]
		public void ParseRecordWhereOnlyLineFeedLineEndingIsUsed()
		{
			using( var memoryStream = new MemoryStream() )
			using( var streamReader = new StreamReader( memoryStream ) )
			using( var streamWriter = new StreamWriter( memoryStream ) )
			using( var parser = new CsvParser( streamReader ) )
			{
				streamWriter.Write( "one,two\n" );
				streamWriter.Write( "three,four\n" );
				streamWriter.Write( "five,six\n" );
				streamWriter.Flush();
				memoryStream.Position = 0;

				var record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 2, record.Length );
				Assert.AreEqual( "one", record[0] );
				Assert.AreEqual( "two", record[1] );

				record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 2, record.Length );
				Assert.AreEqual( "three", record[0] );
				Assert.AreEqual( "four", record[1] );

				record = parser.Read();
				Assert.IsNotNull( record );
				Assert.AreEqual( 2, record.Length );
				Assert.AreEqual( "five", record[0] );
				Assert.AreEqual( "six", record[1] );
			}
		}

		[TestMethod]
		public void ParseCommentedOutLineWithCommentsOn()
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter( stream );
			writer.WriteLine( "one,two,three" );
			writer.WriteLine( "#four,five,six" );
			writer.WriteLine( "seven,eight,nine" );
			writer.Flush();
			stream.Position = 0;
			var reader = new StreamReader( stream );

			var parser = new CsvParser( reader ) { Configuration = { AllowComments = true } };

			parser.Read();
			var record = parser.Read();
			Assert.AreEqual( "seven", record[0] );
		}

		[TestMethod]
		public void ParseCommentedOutLineWithCommentsOff()
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter( stream );
			writer.WriteLine( "one,two,three" );
			writer.WriteLine( "#four,five,six" );
			writer.WriteLine( "seven,eight,nine" );
			writer.Flush();
			stream.Position = 0;
			var reader = new StreamReader( stream );

			var parser = new CsvParser( reader ) { Configuration = { AllowComments = false } };

			parser.Read();
			var record = parser.Read();
			Assert.AreEqual( "#four", record[0] );
		}

		[TestMethod]
		public void ParseCommentedOutLineWithDifferentCommentCommentsOn()
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter( stream );
			writer.WriteLine( "one,two,three" );
			writer.WriteLine( "*four,five,six" );
			writer.WriteLine( "seven,eight,nine" );
			writer.Flush();
			stream.Position = 0;
			var reader = new StreamReader( stream );

			var parser = new CsvParser( reader ) { Configuration = { AllowComments = true, Comment = '*' } };

			parser.Read();
			var record = parser.Read();
			Assert.AreEqual( "seven", record[0] );
		}

		[TestMethod]
		public void ParseUsingDifferentDelimiter()
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter( stream );
			writer.WriteLine( "one\ttwo\tthree" );
			writer.Flush();
			stream.Position = 0;
			var reader = new StreamReader( stream );

			var parser = new CsvParser( reader ) { Configuration = { Delimiter = '\t' } };

			var record = parser.Read();
			Assert.AreEqual( "one", record[0] );
			Assert.AreEqual( "two", record[1] );
			Assert.AreEqual( "three", record[2] );
		}

		[TestMethod]
		public void ParseUsingDifferentQuote()
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter( stream );
			writer.WriteLine( "'one','two','three'" );
			writer.Flush();
			stream.Position = 0;
			var reader = new StreamReader( stream );

			var parser = new CsvParser( reader ) { Configuration = { Quote = '\'' } };

			var record = parser.Read();
			Assert.AreEqual( "one", record[0] );
			Assert.AreEqual( "two", record[1] );
			Assert.AreEqual( "three", record[2] );
		}

		[TestMethod]
		public void ReadFinalRecordWithNoEndOfLineTest()
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter( stream );
			writer.WriteLine( "one,two,three," );
			writer.Write( "four,five,six," );
			writer.Flush();
			stream.Position = 0;
			var reader = new StreamReader( stream );

			var parser = new CsvParser( reader );

			var record = parser.Read();

			Assert.IsNotNull( record );
			Assert.AreEqual( "", record[3] );

			record = parser.Read();

			Assert.IsNotNull( record );
			Assert.AreEqual( "", record[3] );
		}

		[TestMethod]
		public void CharReadTotalTest()
		{
			using( var stream = new MemoryStream() )
			using( var writer = new StreamWriter( stream ) )
			using( var reader = new StreamReader( stream ) )
			using( var parser = new CsvParser( reader ) )
			{
				parser.Configuration.AllowComments = true;

				// This is a breakdown of the char counts.
				// Read() will read up to the first line end char
				// and any more on the line will get read with the next read.

				// [I][d][,][N][a][m][e][\r][\n]
				//  1  2  3  4  5  6  7   8   9
				// [1][,][o][n][e][\r][\n]
				// 10 11 12 13 14  15  16
				// [,][\r][\n]
				// 17  18  19
				// [\r][\n]
				//  20  21
				// [#][ ][c][o][m][m][e][n][t][s][\r][\n]
				// 22 23 24 25 26 27 28 29 30 31  32  33
				// [2][,][t][w][o][\r][\n]
				// 34 35 36 37 38  39  40
				// [3][,]["][t][h][r][e][e][,][ ][f][o][u][r]["][\r][\n]
				// 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55  56  57
				
				writer.WriteLine( "Id,Name" );
				writer.WriteLine( "1,one" );
				writer.WriteLine( "," );
				writer.WriteLine( "" );
				writer.WriteLine( "# comments" );
				writer.WriteLine( "2,two" );
				writer.WriteLine( "3,\"three, four\"" );
				writer.Flush();
				stream.Position = 0;

				parser.Read();
				Assert.AreEqual( 8, parser.CharPosition );

				parser.Read();
				Assert.AreEqual( 15, parser.CharPosition );

				parser.Read();
				Assert.AreEqual( 18, parser.CharPosition );

				parser.Read();
				Assert.AreEqual( 39, parser.CharPosition );

				parser.Read();
				Assert.AreEqual( 56, parser.CharPosition );

				parser.Read();
				Assert.AreEqual( 57, parser.CharPosition );
			}
		}

		[TestMethod]
		public void StreamSeekingUsingCharPositionTest()
		{
			using( var stream = new MemoryStream() )
			using( var writer = new StreamWriter( stream ) )
			using( var reader = new StreamReader( stream ) )
			using( var parser = new CsvParser( reader ) )
			{
				parser.Configuration.AllowComments = true;

				// This is a breakdown of the char counts.
				// Read() will read up to the first line end char
				// and any more on the line will get read with the next read.

				// [I][d][,][N][a][m][e][\r][\n]
				//  1  2  3  4  5  6  7   8   9
				// [1][,][o][n][e][\r][\n]
				// 10 11 12 13 14  15  16
				// [,][\r][\n]
				// 17  18  19
				// [\r][\n]
				//  20  21
				// [#][ ][c][o][m][m][e][n][t][s][\r][\n]
				// 22 23 24 25 26 27 28 29 30 31  32  33
				// [2][,][t][w][o][\r][\n]
				// 34 35 36 37 38  39  40
				// [3][,]["][t][h][r][e][e][,][ ][f][o][u][r]["][\r][\n]
				// 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55  56  57

				writer.WriteLine( "Id,Name" );
				writer.WriteLine( "1,one" );
				writer.WriteLine( "," );
				writer.WriteLine( "" );
				writer.WriteLine( "# comments" );
				writer.WriteLine( "2,two" );
				writer.WriteLine( "3,\"three, four\"" );
				writer.Flush();
				stream.Position = 0;

				var record = parser.Read();
				Assert.AreEqual( "Id", record[0] );
				Assert.AreEqual( "Name", record[1] );

				stream.Position = 0;
				stream.Seek( parser.CharPosition, SeekOrigin.Begin );
				record = parser.Read();
				Assert.AreEqual( "1", record[0] );
				Assert.AreEqual( "one", record[1] );

				stream.Position = 0;
				stream.Seek( parser.CharPosition, SeekOrigin.Begin );
				record = parser.Read();
				Assert.AreEqual( "", record[0] );
				Assert.AreEqual( "", record[1] );

				stream.Position = 0;
				stream.Seek( parser.CharPosition, SeekOrigin.Begin );
				record = parser.Read();
				Assert.AreEqual( "2", record[0] );
				Assert.AreEqual( "two", record[1] );

				stream.Position = 0;
				stream.Seek( parser.CharPosition, SeekOrigin.Begin );
				record = parser.Read();
				Assert.AreEqual( "3", record[0] );
				Assert.AreEqual( "three, four", record[1] );
			}
		}

		[TestMethod]
		public void RowTest()
		{
			using( var stream = new MemoryStream() )
			using( var writer = new StreamWriter( stream ) )
			using( var reader = new StreamReader( stream ) )
			using( var parser = new CsvParser( reader ) )
			{
				writer.Write( "1,2\r\n" );
				writer.Write( "3,4\r\n" );
				writer.Flush();
				stream.Position = 0;

				var rowCount = 0;
				while( parser.Read() != null )
				{
					rowCount++;
					Assert.AreEqual( rowCount, parser.Row );
				}
			}
		}

		[TestMethod]
		public void RowBlankLinesTest()
		{
			using( var stream = new MemoryStream() )
			using( var writer = new StreamWriter( stream ) )
			using( var reader = new StreamReader( stream ) )
			using( var parser = new CsvParser( reader ) )
			{
				writer.Write( "1,2\r\n" );
				writer.Write( "\r\n" );
				writer.Write( "3,4\r\n" );
				writer.Write( "\r\n" );
				writer.Write( "5,6\r\n" );
				writer.Flush();
				stream.Position = 0;

				var rowCount = 1;
				while( parser.Read() != null )
				{
					Assert.AreEqual( rowCount, parser.Row );
					rowCount += 2;
				}
			}
		}

		[TestMethod]
		public void RowCommentLinesTest()
		{
			using( var stream = new MemoryStream() )
			using( var writer = new StreamWriter( stream ) )
			using( var reader = new StreamReader( stream ) )
			using( var parser = new CsvParser( reader ) )
			{
				writer.Write( "1,2\r\n" );
				writer.Write( "# comment 1\r\n" );
				writer.Write( "3,4\r\n" );
				writer.Write( "# comment 2\r\n" );
				writer.Write( "5,6\r\n" );
				writer.Flush();
				stream.Position = 0;

				parser.Configuration.AllowComments = true;
				var rowCount = 1;
				while( parser.Read() != null )
				{
					Assert.AreEqual( rowCount, parser.Row );
					rowCount += 2;
				}
			}
		}

		[TestMethod]
		public void ByteCountTest()
		{
			using( var stream = new MemoryStream() )
			using( var writer = new StreamWriter( stream ) )
			using( var reader = new StreamReader( stream ) )
			using( var parser = new CsvParser( reader ) )
			{
				parser.Configuration.CountBytes = true;
				writer.Write( "1,2\r\n" );
				writer.Write( "3,4\r\n" );
				writer.Flush();
				stream.Position = 0;

				parser.Read();
				Assert.AreEqual( 4, parser.BytePosition );

				parser.Read();
				Assert.AreEqual( 9, parser.BytePosition );

				parser.Read();
				Assert.AreEqual( 10, parser.BytePosition );
			}
		}

		[TestMethod]
		public void ByteCountUsingCharWithMoreThanSingleByteTest()
		{
			var encoding = Encoding.Unicode;
			using( var stream = new MemoryStream() )
			using( var writer = new StreamWriter( stream, encoding ) )
			using( var reader = new StreamReader( stream, encoding ) )
			using( var parser = new CsvParser( reader ) )
			{
				//崔钟铉
				parser.Configuration.CountBytes = true;
				parser.Configuration.Encoding = encoding;
				writer.Write( "1,崔\r\n" );
				writer.Write( "3,钟\r\n" );
				writer.Write( "5,铉\r\n" );
				writer.Flush();
				stream.Position = 0;

				parser.Read();
				Assert.AreEqual( 8, parser.BytePosition );

				parser.Read();
				Assert.AreEqual( 18, parser.BytePosition );

				parser.Read();
				Assert.AreEqual( 28, parser.BytePosition );

				parser.Read();
				Assert.AreEqual( 30, parser.BytePosition );
			}
		}

		[TestMethod]
		public void StreamSeekingUsingByteCountTest()
		{
			var encoding = Encoding.Unicode;
			using( var stream = new MemoryStream() )
			using( var writer = new StreamWriter( stream, encoding ) )
			using( var reader = new StreamReader( stream, encoding ) )
			using( var parser = new CsvParser( reader ) )
			{
				parser.Configuration.CountBytes = true;
				parser.Configuration.Encoding = encoding;
				parser.Configuration.AllowComments = true;

				// This is a breakdown of the char counts.
				// Read() will read up to the first line end char
				// and any more on the line will get read with the next read.

				// [I][d][,][N][a][m][e][\r][\n]
				//  1  2  3  4  5  6  7   8   9
				// [1][,][o][n][e][\r][\n]
				// 10 11 12 13 14  15  16
				// [,][\r][\n]
				// 17  18  19
				// [\r][\n]
				//  20  21
				// [#][ ][c][o][m][m][e][n][t][s][\r][\n]
				// 22 23 24 25 26 27 28 29 30 31  32  33
				// [2][,][t][w][o][\r][\n]
				// 34 35 36 37 38  39  40
				// [3][,]["][t][h][r][e][e][,][ ][f][o][u][r]["][\r][\n]
				// 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55  56  57

				writer.WriteLine( "Id,Name" );
				writer.WriteLine( "1,one" );
				writer.WriteLine( "," );
				writer.WriteLine( "" );
				writer.WriteLine( "# comments" );
				writer.WriteLine( "2,two" );
				writer.WriteLine( "3,\"three, four\"" );
				writer.Flush();
				stream.Position = 0;

				var record = parser.Read();
				Assert.AreEqual( "Id", record[0] );
				Assert.AreEqual( "Name", record[1] );

				stream.Position = 0;
				stream.Seek( parser.BytePosition, SeekOrigin.Begin );
				record = parser.Read();
				Assert.AreEqual( "1", record[0] );
				Assert.AreEqual( "one", record[1] );

				stream.Position = 0;
				stream.Seek( parser.BytePosition, SeekOrigin.Begin );
				record = parser.Read();
				Assert.AreEqual( "", record[0] );
				Assert.AreEqual( "", record[1] );

				stream.Position = 0;
				stream.Seek( parser.BytePosition, SeekOrigin.Begin );
				record = parser.Read();
				Assert.AreEqual( "2", record[0] );
				Assert.AreEqual( "two", record[1] );

				stream.Position = 0;
				stream.Seek( parser.BytePosition, SeekOrigin.Begin );
				record = parser.Read();
				Assert.AreEqual( "3", record[0] );
				Assert.AreEqual( "three, four", record[1] );
			}
		}
	}
}
