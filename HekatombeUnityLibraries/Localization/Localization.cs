using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Hekatombe.Utils;
using Hekatombe.DataHelpers;
using Hekatombe.Base;

namespace Hekatombe.Localization
{
	/*
	//Inherit the following EnumClass in the project code by doing something like this:
	public class ELanguage : Hekatombe.Localization.ELanguage {
		public static readonly ELanguage English = new ELanguage(0, "en");
		public static readonly ELanguage Catala = new ELanguage(1, "ca");
		public static readonly ELanguage Espanol = new ELanguage(2, "es");

		public ELanguage( int value, string name ):base( value, name){}

		//Set an array with the Languages to pass it to Localization.Init()
		public static ELanguage[] LanguageAr = new ELanguage[3]{ELanguage.English, ELanguage.Catala, ELanguage.Espanol};
	}
	*/

    public class ELanguage : EnumClass
    {
        //Constructor, just copy it on every inherited class
        public ELanguage( int value, string name ):base( value, name){}
    }

	public partial class Localization {
        private static ELanguage _forcedLanguage;

		public Dictionary<string, string[]> LocData = new Dictionary<string, string[]>();

		public static ELanguage[] Languages;
		
        private static ELanguage _defaultLanguage;
		private static ELanguage _selectedLanguage;
		private static int IndexSelectedLanguage;

		private static Localization Instance;

		private const string kPPKeyLanguage = "SelectedLanguage";

        //To allow Force a Language for test purposes
        public static void SetForcedLanguage(ELanguage eLang)
        {
            _forcedLanguage = eLang;
        }

		public static string Get(string key)
		{
			return Get (key, _selectedLanguage);
		}

		public static string Get(string key, ELanguage language)
		{
			if (!Exists(key)) {
				string strError = string.Format("LocMiss-{0}: {1}", language, key);
				Debug.LogError(strError);
				return strError;
			}
            return Instance.LocData[key][language.Value];
		}

		public static bool Exists(string key)
		{
			return Instance.LocData.ContainsKey (key);
		}

		public static string GetFormat(string key, params object[] args)
		{
			return string.Format (Get (key), args);
		}
            
        public static void Init(ELanguage[] languages,  ELanguage defaultLanguage)
		{
			if (Instance == null) {
				Instance = new Localization();
                _defaultLanguage = defaultLanguage;
                _selectedLanguage = _defaultLanguage;

                Languages = languages;

				//Set initial language
				if (_forcedLanguage != null)
				{
                    Debug.LogWarning("Tha Language is Forced by Config: " + _forcedLanguage + ". Set Language: " + PlayerPrefs.GetString(kPPKeyLanguage));
                    SetLanguage(_forcedLanguage);
				} else if (IsLanguageSetByUser())
				{
					SetLanguage(GetLanguageByName(PlayerPrefs.GetString(kPPKeyLanguage)));
				} else {
					SetLanguage(_defaultLanguage);
				}
			}
		}

        //To Hardcode text data (Not recommended but...)
		public static void SetDataHardCoded(Dictionary<string, string[]> locData)
		{
			Instance.LocData = locData;
		}

		public static bool IsLanguageSetByUser()
		{
			return PlayerPrefs.HasKey (kPPKeyLanguage);
		}

		public static void SetLanguageByUser(ELanguage language)
		{
			SetLanguage (language);
			PlayerPrefs.SetString(kPPKeyLanguage, _selectedLanguage.Name);
		}

		private static void SetLanguage(ELanguage language)
		{
			_selectedLanguage = language;
			IndexSelectedLanguage = _selectedLanguage.Value;
		}

		public static ELanguage GetLanguage()
		{
			return _selectedLanguage;
		}
		
		public static void StaticLoad(string localizationJsonStr)
		{
			Instance.Load(localizationJsonStr);
		}

		private void AddLoc(string id, string es)
		{
			AddLoc (id, es, es, es);
		}

		private void AddLoc(string id, string es, string ca, string en)
		{
			LocData.Add(id, new string[]{
				es,
				ca,
				en
			});
		}
		
		private void Load(string localizationJsonStr)
		{
            if (string.IsNullOrEmpty (localizationJsonStr)) {
                throw new ArgumentNullException ("Localization Json NULL");
            }
			LocalizationDataParser parser = new LocalizationDataParser();
			parser.Parse(LocData, localizationJsonStr);
		}
            
		public static string GetResult()
		{
			return string.Format ("Get {0} Localizations in {1} Languages", Instance.LocData.Count, NumLanguages);
		}

        public static int NumLanguages
        {
            get{
				return Languages.Length;
            }
        }

		public static ELanguage GetLanguageByName(string name)
		{
			for (int i = 0; i < NumLanguages; i++) {
				if (Languages [i].Name == name) {
					return Languages [i];
				}
			}
			Debug.LogError ("No language found with Name: " + name);
			return null;
		}
	}
}
