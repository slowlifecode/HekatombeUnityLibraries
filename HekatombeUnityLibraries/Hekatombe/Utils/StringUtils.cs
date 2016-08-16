using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Hekatombe.Utils
{
	public static class StringUtils
	{
		public static string DictToString<T, V>(IEnumerable<KeyValuePair<T, V>> items, string format = "")
		{
			format = String.IsNullOrEmpty(format) ? "{0}='{1}' " : format; 

			StringBuilder itemString = new StringBuilder();

			for(int k = 0; k < items.Count(); k++)
			{
				KeyValuePair<T,V> item = items.ElementAt(k);
				itemString.AppendFormat(format, item.Key, item.Value);
			}

			return itemString.ToString(); 
		}

		public static Dictionary<string,string> QueryToDictionary(string query)
		{
			var dict = new Dictionary<string, string>();
			if(string.IsNullOrEmpty(query))
			{
				return dict;
			}
			var parts = query.Split('&');
			for(int i=0; i<parts.Length; ++i)
			{
				var part = parts[i].Split('=');
				if(part.Length > 1)
				{
					dict[Uri.UnescapeDataString(part[0])] = Uri.UnescapeDataString(part[1]);
				}
				else
				{
					dict[Uri.UnescapeDataString(part[0])] = "";
				}
			}
			return dict;
		}

		public static string GetJoinedUrlParams (KeyValuePair<string,string>[] parms)
		{
			string result = "";
			foreach (KeyValuePair<string,string> param in parms)
				result += "&" + param.Key + "=" + Uri.EscapeDataString (param.Value);
			return result;
		}        

		public static string WithFormat(this string str, params object[] args)
		{
			return string.Format(str, args);
		}


		public static string JoinUrls(this string baseUrl, params object[] subUrls)
		{
			var sb = new StringBuilder();
			sb.Append(baseUrl);

			if (!baseUrl.EndsWith(@"/")) sb.Append(@"/");

			for (int i = 0; i < subUrls.Length; i++)
			{
				var subUrl = subUrls[i];
				sb.Append(subUrl.ToString().Replace(@"/", string.Empty));
				// do not append a slash  for the last element
				if (i < subUrls.Length-1) sb.Append("/");
			}

			return sb.ToString();
		}

		public static string UppercaseFirstLetter(this string value)
		{
			// Uppercase first letter
			if (value.Length > 0)
			{
				char[] array = value.ToCharArray();
				array[0] = char.ToUpper(array[0]);
				return new string(array);
			}
			return value;
		}
	}
}