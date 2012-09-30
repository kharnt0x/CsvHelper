using System;

namespace CsvHelper.MissingFromRt45
{
	[AttributeUsage( AttributeTargets.All )]
	public class TypeConverterAttribute : Attribute
	{
		private readonly string name;

		public string ConverterTypeName
		{
			get { return name;  }
		}

		public TypeConverterAttribute()
		{
			name = string.Empty;
		}

		public TypeConverterAttribute( string name )
		{
			this.name = name.ToUpperInvariant();
		}

		public TypeConverterAttribute( Type type )
		{
			name = type.AssemblyQualifiedName;
		}

		public override bool Equals( object obj )
		{
			var attribute = obj as TypeConverterAttribute;
			if( attribute == null )
			{
				return false;
			}
			return attribute.ConverterTypeName == name;
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}
	}
}
