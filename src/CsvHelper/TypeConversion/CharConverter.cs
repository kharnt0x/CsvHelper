using System.Globalization;

namespace CsvHelper.TypeConversion
{
	/// <summary>
	/// Converts a Char to and from a string.
	/// </summary>
	public class CharConverter : DefaultTypeConverter
	{
		/// <summary>
		/// Converts the string to an object.
		/// </summary>
		/// <param name="culture">The culture used when converting.</param>
		/// <param name="text">The string to convert to an object.</param>
		/// <returns>The object created from the string.</returns>
		public override object ConvertFromString( CultureInfo culture, string text )
		{
			if( text != null )
			{
				text = text.Trim();
			}

			char c;
			if( char.TryParse( text, out c ) )
			{
				return c;
			}

			return base.ConvertFromString( culture, text );
		}
	}
}
