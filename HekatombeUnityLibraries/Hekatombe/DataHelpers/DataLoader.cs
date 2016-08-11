using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Hekatombe.Base;

namespace Hekatombe.DataHelpers
{
	public class LoadDataResult
	{
		public bool IsSuccess = false;
		public string Contents;
		public string Error = "Not loaded yet";

		public LoadDataResult(bool isSuccess, string text)
		{
			if (isSuccess) {
				Init (isSuccess, text, "");
			} else {
				Init (isSuccess, "", text);
			}
		}

		public void Init(bool isSuccess, string contents, string error)
		{
			IsSuccess = isSuccess;
			Contents = contents;
			Error = error;
		}
	}

	public class DataLoader : MonoBehaviour {

		public static DataLoader Instance;

		public static void Init()
		{
			//Create an object if don't exist
			DataLoader dl = FindObjectOfType<DataLoader>();
			if (dl == null)
			{
				GameObject go = new GameObject ();
				Instance = go.AddComponent<DataLoader> ();
				DontDestroyOnLoad (Instance.gameObject);
			} else {
				Debug.LogWarning ("Trying to create another DataLoader object, just one is enough :)");
			}
		}

		public static void LoadData(bool isRemote, string path, Action<LoadDataResult> onCallbackEnd)
		{
			Debug.Log(string.Format("Load File Contents: {0}", path));
			if (isRemote) {
				Instance.StartCoroutine(Instance.RemoteLoading (path, onCallbackEnd));
			} else {
				Instance.LocalLoading (path, onCallbackEnd);
			}
		}

		public IEnumerator RemoteLoading(string path, Action<LoadDataResult> onCallbackEnd)
		{
			WWW www = new WWW(path);
			yield return www;
			if (www.error == null)
			{
				onCallbackEnd (new LoadDataResult(true, www.text));
			}
			else
			{
				Debug.LogError("Remote Loading Error: " + www.error);
				onCallbackEnd (new LoadDataResult(false, www.error));
			}
		}

		public void LocalLoading(string path, Action<LoadDataResult> onCallbackEnd)
		{
			if (!File.Exists (path)) {
				string error = "Local Loading Error: " + path + " not found!";
				onCallbackEnd (new LoadDataResult (false, error));
			} else {
				onCallbackEnd (new LoadDataResult (true, File.ReadAllText (path)));
			}
		}
	}
}