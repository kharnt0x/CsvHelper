using System;
using System.Globalization;

namespace CsvHelper.TypeConversion
{
	/// <summary>
	/// Converts an object to and from a string.
	/// </summary>
	public class DefaultTypeConverter : ITypeConverter
	{
		/// <summary>
		/// Converts the object to a string.
		/// </summary>
		/// <param name="value">The object to convert to a string.</param>
		/// <returns>The string representation of the object.</returns>
		public virtual string ConvertToString( object value )
		{
			return ConvertToString( CultureInfo.CurrentCulture, value );
		}

		/// <summary>
		/// Converts the object to a string.
		/// </summary>
		/// <param name="culture">The culture used when converting.</param>
		/// <param name="value">The object to convert to a string.</param>
		/// <returns>The string representation of the object.</returns>
		public virtual string ConvertToString( CultureInfo culture, object value )
		{
			if( value == null )
			{
				return string.Empty;
			}

// ReSharper disable PossibleUnintendedReferenceComparison
			if( culture != null && culture != CultureInfo.CurrentCulture )
// ReSharper restore PossibleUnintendedReferenceComparison
			{
				var formattable = value as IFormattable;
				if( formattable != null )
				{
					return formattable.ToString( null, culture );
				}
			}

			return value.ToString();
		}

		/// <summary>
		/// Converts the string to an object.
		/// </summary>
		/// <param name="text">The string to convert to an object.</param>
		/// <returns>The object created from the string.</returns>
		public virtual object ConvertFromString( string text )
		{
			return ConvertFromString( CultureInfo.CurrentCulture, text );
		}

		/// <summary>
		/// Converts the string to an object.
		/// </summary>
		/// <param name="culture">The culture used when converting.</param>
		/// <param name="text">The string to convert to an object.</param>
		/// <returns>The object created from the string.</returns>
		public virtual object ConvertFromString( CultureInfo culture, string text )
		{
			throw new NotSupportedException( "The conversion cannot be performed." );
		}
	}
}
