using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Hekatombe.Utils;

namespace Hekatombe.Localization
{	
	public class LocalizationDataHardCodedBase {

		protected Dictionary<string, string[]> _locData = new Dictionary<string, string[]>();

		protected void AddLoc(string id, string es, string ca, string en)
		{
			_locData.Add(id, new string[]{
				es,
				ca,
				en
			});
		}

		protected void AddLoc(string id, string es)
		{
			AddLoc (id, es, es, es);
		}

		public Dictionary<string, string[]> GetLocData()
		{
			return _locData;
		}
	}
}
