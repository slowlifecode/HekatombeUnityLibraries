using System;

namespace Hekatombe.Utils
{
    public class EnumUtils
	{
		public static T Parse<T>( string value )
		{
			return (T) Enum.Parse( typeof( T ), value, true );
		}

		public static bool IsDefined<T>(string value)
		{
			return Enum.IsDefined(typeof( T ), value);
		}

		public static T ParseDefault<T>(string value, T defaultValue) 
		{
			if (string.IsNullOrEmpty(value) || ! IsDefined<T>(value))
			{
				return defaultValue;
			}
			
			return Parse<T>(value);
		}
    }
}