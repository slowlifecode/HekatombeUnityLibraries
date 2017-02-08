using System;
using System.Collections.Generic;

namespace Hekatombe.DataHelpers
{
	public static class JsonHelper {

		public static T[] FromJsonToArray<T>(string json) {
			UnityEngine.Debug.Log("JSON to ARRAY: " + json);
			Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
			return wrapper.Items;
		}

		public static T[] FromJsonToArrayAddParent<T>(string json)
		{
			string jsonParent = string.Concat("{\"Items\":\n", json, "}");
			return FromJsonToArray<T> (jsonParent);
		}

		public static List<T> FromJsonToListAddParent<T>(string json)
		{
			return new List<T> (FromJsonToArrayAddParent<T>(json));
		}

		public static string ToJson<T>(T[] array) {
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = array;
			return UnityEngine.JsonUtility.ToJson(wrapper);
		}

		[Serializable]
		private class Wrapper<T> {
			public T[] Items;
		}

		//This could be better done with Serializing, but I have to create to many classes... Boring :P
		public static string SetJsonKeyValues(params object[] values)
		{
			string json = "";
			int length = values.Length;
			if (values.Length % 2 != 0) {
				UnityEngine.Debug.LogError ("SetPostJsonData not correct because it has NOT pair values. The last value will be ignored.");
				length--;
			}
			//Oju que s'afegeixen cada 2 per simplificar!
			string comma = ", ";
			for (int i=0; i< length; i+=2)
			{
				if (i >= length - 2) {
					comma = "";
				}
				json += string.Format("\"{0}\": \"{1}\"{2}", values[i], values[i+1], comma);
			}
			//Add the { }
			json = "{" + json + "}";
			//UnityEngine.Debug.Log ("JSON DATA: " + json);
			return json;
		}

		//check if a json string is empty
		public static bool IsEmpty(string json)
		{
			string jsonClean = json.Trim ().Replace ("\n", "");
			//Empty means "null", "", "{}"...
			if (jsonClean == null || jsonClean.Length<=2)
			{
				return true;
			}
			return false;;
		}
	}
}