using System.Globalization;

namespace CsvHelper.TypeConversion
{
	/// <summary>
	/// Converts an Int16 to and from a string.
	/// </summary>
	public class Int16Converter : DefaultTypeConverter
	{
		/// <summary>
		/// Converts the string to an object.
		/// </summary>
		/// <param name="culture">The culture used when converting.</param>
		/// <param name="text">The string to convert to an object.</param>
		/// <returns>The object created from the string.</returns>
		public override object ConvertFromString( CultureInfo culture, string text )
		{
			short s;
			if( short.TryParse( text, NumberStyles.Integer, culture, out s ) )
			{
				return s;
			}

			return base.ConvertFromString( culture, text );
		}
	}
}
