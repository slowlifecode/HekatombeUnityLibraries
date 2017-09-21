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
		public void Parse(Dictionary<string, Dictionary<int, string>> locData, string json)
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
					if (Localization.LogVerbose)
					{
						Debug.LogError(string.Format("A key already exists: {0} Saved text: '{1}' New Text: \n{2} ", kvp.Key, locData[kvp.Key].Values, kvp.Value.ToString()));
					}
					continue;
				}
				//Debug.Log ("KEY: " + kvp.Key + " Value: " + kvp.Value);
				//This implementation ensures the text is translated to all languages
				Dictionary<string,object> languagesDic = kvp.Value as Dictionary<string, object>;
				Dictionary<int, string> arLanguages = new Dictionary<int, string>();

				for (int i=0; i < Localization.NumLanguages; i++)
				{
					string strText = "";
					//Comprova si està aquella traducció per aquell idioma
					if (languagesDic.ContainsKey(Localization.Languages[i].Name))
					{
						strText = languagesDic[Localization.Languages[i].Name].ToString();
						//Debug.Log (string.Format("K: {0} L: {1} T: '{2}'", kvp.Key, Localization.ArLanguages[i], arLanguages[Localization.Languages[i]]));
					} else {
						strText = string.Format("Miss-Lang-{0}:{1}",Localization.Languages[i].Name, kvp.Key);
						if (Localization.LogVerbose) {
							Debug.LogError (arLanguages [i]);
						}
					}
					arLanguages.Add(Localization.Languages[i].Value, strText);
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