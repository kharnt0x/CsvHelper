using System;

namespace CsvHelper.TypeConversion
{
	[AttributeUsage( AttributeTargets.Property | AttributeTargets.Field )]
	public class TypeConverterAttribute : Attribute
	{
		public Type Type { get; set; }

		public TypeConverterAttribute( Type type )
		{
			Type = type;
		}
	}
}
