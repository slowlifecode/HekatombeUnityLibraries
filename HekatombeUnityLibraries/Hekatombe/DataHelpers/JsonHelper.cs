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
	}
}