using System.Globalization;

namespace CsvHelper.TypeConversion
{
	/// <summary>
	/// Converts a Byte to and from a string.
	/// </summary>
	public class ByteConverter : DefaultTypeConverter
	{
		/// <summary>
		/// Converts the string to an object.
		/// </summary>
		/// <param name="culture">The culture used when converting.</param>
		/// <param name="text">The string to convert to an object.</param>
		/// <returns>The object created from the string.</returns>
		public override object ConvertFromString( CultureInfo culture, string text )
		{
			byte b;
			if( byte.TryParse( text, NumberStyles.Integer, culture, out b ) )
			{
				return b;
			}

			return base.ConvertFromString( culture, text );
		}
	}
}
