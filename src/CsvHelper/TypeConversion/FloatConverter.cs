using System.Globalization;

namespace CsvHelper.TypeConversion
{
	/// <summary>
	/// Converts a Float to and from a string.
	/// </summary>
	public class FloatConverter : DefaultTypeConverter
	{
		/// <summary>
		/// Converts the string to an object.
		/// </summary>
		/// <param name="culture">The culture used when converting.</param>
		/// <param name="text">The string to convert to an object.</param>
		/// <returns>The object created from the string.</returns>
		public override object ConvertFromString( CultureInfo culture, string text )
		{
			float f;
			if( float.TryParse( text, NumberStyles.Float, culture, out f ) )
			{
				return f;
			}

			return base.ConvertFromString( culture, text );
		}
	}
}
