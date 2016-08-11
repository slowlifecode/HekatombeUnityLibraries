using UnityEngine;
using UnityEditor;
using System.Collections;


public class PlayerPrefsDeleteAll : ScriptableObject 
{
	[MenuItem ("Tools/PlayerPrefs/DeleteAll")]
	public static void PlayerPrefsDeleteAllAction ()
	{
		PlayerPrefs.DeleteAll();
	}
}