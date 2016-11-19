using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Hekatombe.Utils
{
	//This Dirty class it's used to have the references to all the sprites you need to access from the Atlas packers
	//Put one prefab of this on every scene and they will take of them-selves automatically
	public class SpritesReferences : MonoBehaviour {

		public static SpritesReferences Instance;
		public List<Sprite> SpritesList;
		public Dictionary<string, Sprite> _spritesDictionary;

		void Awake () 
		{
			if (Instance == null) 
			{
				Instance = this;
				Init();
				DontDestroyOnLoad(this.gameObject);
			} else {
				Destroy (gameObject);
			}
		}

		private void Init()
		{
			_spritesDictionary = new Dictionary<string, Sprite> ();
			for(int i=0; i<SpritesList.Count; i++)
			{
				if (SpritesList[i] !=null && !_spritesDictionary.ContainsKey(SpritesList[i].name))
				{
					_spritesDictionary.Add (SpritesList[i].name ,SpritesList[i]);
					//Debug.Log ("Sprite Name: " + SpritesList[i].name);
				}
			}
		}

		public static Sprite GetSpriteByName(string name)
		{
			if (!ExistsSpriteByName(name)) 
			{
				Debug.LogError("Sprite not found: " + name);
				return null;
			}
			return Instance._spritesDictionary[name];
		}

		public static bool ExistsSpriteByName(string name)
		{
			if (Instance._spritesDictionary.ContainsKey (name)) 
			{
				return true;
			}
			return false;
		}
	}
}
