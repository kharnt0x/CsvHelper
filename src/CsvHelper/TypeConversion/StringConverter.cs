using System.Globalization;

namespace CsvHelper.TypeConversion
{
	/// <summary>
	/// Converts a string to and from a string.
	/// </summary>
	public class StringConverter : DefaultTypeConverter
	{
		/// <summary>
		/// Converts the string to an object.
		/// </summary>
		/// <param name="culture">The culture used when converting.</param>
		/// <param name="text">The string to convert to an object.</param>
		/// <returns>The object created from the string.</returns>
		public override object ConvertFromString( CultureInfo culture, string text )
		{
			if( text == null )
			{
				return string.Empty;
			}

			return base.ConvertFromString( text );
		}
	}
}
