using System.Globalization;

namespace CsvHelper.TypeConversion
{
	/// <summary>
	/// Converts a UInt64 to and from a string.
	/// </summary>
	public class UInt64Converter : DefaultTypeConverter
	{
		/// <summary>
		/// Converts the string to an object.
		/// </summary>
		/// <param name="culture">The culture used when converting.</param>
		/// <param name="text">The string to convert to an object.</param>
		/// <returns>The object created from the string.</returns>
		public override object ConvertFromString( CultureInfo culture, string text )
		{
			ulong ul;
			if( ulong.TryParse( text, NumberStyles.Integer, culture, out ul ) )
			{
				return ul;
			}

			return base.ConvertFromString( culture, text );
		}
	}
}
