using System;
using System.Collections.Generic;
using Hekatombe.Utils;
using UnityEngine;
using MiniJSON;
using Hekatombe.DataHelpers;

namespace Hekatombe.Localization
{
    public class LocalizationDataParser
    {
		public void Parse(Dictionary<string, string[]> locData, string json)
		{
			//Debug.Log ("JSON:\n" + json);
			Dictionary<string, object> dict = Json.Deserialize (json) as Dictionary<string, object>;

			if (JsonHelper.IsEmpty (json)) {
				return;
			}

			foreach(var kvp in dict)
            {
				//Check the keys aren't duplicated
				if (locData.ContainsKey(kvp.Key))
				{
					Debug.LogError(string.Format("A key already exists: {0} Saved text: '{1}' New Text: \n{2} ", kvp.Key, locData[kvp.Key][0], kvp.Value.ToString()));
					continue;
				}
				//Debug.Log ("KEY: " + kvp.Key + " Value: " + kvp.Value);
				//This implementation ensures the text is translated to all languages
				Dictionary<string,object> languagesDic = kvp.Value as Dictionary<string, object>;
				string[] arLanguages = new string[Localization.NumLanguages];

				for (int i=0; i < arLanguages.Length; i++)
				{
					if (languagesDic.ContainsKey(Localization.Languages[i].Name))
					{
						arLanguages[i] = languagesDic[Localization.Languages[i].Name].ToString();
						//Debug.Log (string.Format("K: {0} L: {1} T: '{2}'", kvp.Key, Localization.ArLanguages[i], arLanguages[i]));
					} else {
						arLanguages[i] = string.Format("Key {0} not found for language {1}", kvp.Key, Localization.Languages[i].Name);
						if (Localization.LogVerbose) {
							Debug.LogError (arLanguages [i]);
						}
					}
				}

				/*
				 * This other implementation find all received values instead of make sure that the translation is set on all languages
				Dictionary<ELanguage, string> keyLanguages = new Dictionary<ELanguage, string>();
				foreach(var kvp2 in kvp.Value.AsDic)
				{
					keyLanguages.Add (EnumUtils.ParseEnum<ELanguage>(kvp2.Key), kvp2.Value.ToString());
					//Debug.Log (kvp2.Key + " Val: " + kvp2.Value);
				}*/
				locData.Add(kvp.Key, arLanguages);
			}
		}
    }
}