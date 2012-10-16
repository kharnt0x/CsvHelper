#if WINRT_4_5
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CsvHelper.Tests
{
	[TestClass]
	public class StringExtensionTests
	{
		[TestMethod]
		public void IsNullOrWhiteSpaceTest()
		{
			string nullString = null;
			Assert.IsTrue( nullString.IsNullOrWhiteSpace() );
			Assert.IsTrue( "".IsNullOrWhiteSpace() );
			Assert.IsTrue( " ".IsNullOrWhiteSpace() );
			Assert.IsTrue( "	".IsNullOrWhiteSpace() );
			Assert.IsFalse( "a".IsNullOrWhiteSpace() );
		}
	}
}
