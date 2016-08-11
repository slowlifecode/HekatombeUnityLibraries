using System;

namespace Hekatombe.Utils
{
    public class EnumUtils
	{
		public static T ParseEnum<T>( string value )
		{
			return (T) Enum.Parse( typeof( T ), value, true );
		}

		public static bool IsDefined<T>(string value)
		{
			return Enum.IsDefined(typeof( T ), value);
		}

		//Warning, that will break in case the parameter is void, but also if there is some weird character, K-Boom!
		public static T ParseEnum<T>(string value, T defaultValue) 
		{
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}
			
			return ParseEnum<T>(value);
		}
    }
}