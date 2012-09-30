using System;
using System.Globalization;

namespace CsvHelper.MissingFromRt45
{
	public class TypeConverter
	{
		public virtual bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
		{
			throw new NotImplementedException();
		}

		public virtual object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, object value )
		{
			throw new NotImplementedException();
		}
	}
}
