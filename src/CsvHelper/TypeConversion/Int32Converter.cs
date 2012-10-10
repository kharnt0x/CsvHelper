using System.Globalization;

namespace CsvHelper.TypeConversion
{
	/// <summary>
	/// Converts an Int32 to and from a string.
	/// </summary>
	public class Int32Converter : DefaultTypeConverter
	{
		/// <summary>
		/// Converts the string to an object.
		/// </summary>
		/// <param name="culture">The culture used when converting.</param>
		/// <param name="text">The string to convert to an object.</param>
		/// <returns>The object created from the string.</returns>
		public override object ConvertFromString( CultureInfo culture, string text )
		{
			int i;
			if( int.TryParse( text, NumberStyles.Integer, culture, out i ) )
			{
				return i;
			}

			return base.ConvertFromString( culture, text );
		}
	}
}
