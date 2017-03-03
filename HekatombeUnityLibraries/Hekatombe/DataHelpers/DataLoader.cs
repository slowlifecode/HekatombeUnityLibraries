using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Hekatombe.Base;
using System.Collections.Generic;

namespace Hekatombe.DataHelpers
{
	public class LoadDataResult
	{
		public bool IsSuccess = false;
		public string Contents;
		public string Error = "Not loaded yet";

		private const string k401 = "401";
		private const string k403 = "403";
		private const string k404 = "404";

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

		public bool IsUnauthorized()
		{
			return Contents.Contains (k401) || Error.Contains (k401) || Contents.Contains (k403) || Error.Contains (k403);
		}

		public bool IsNotFound()
		{
			return Contents.Contains (k404) || Error.Contains (k404);
		}
	}

	public class DataLoader : MonoBehaviour {

		public static DataLoader Instance;
		public static bool IsVerbose = false;

		public static void Init()
		{
			//Create an object if don't exist
			DataLoader dl = FindObjectOfType<DataLoader>();
			if (dl == null)
			{
				GameObject go = new GameObject ();
				go.name = "DataLoader";
				Instance = go.AddComponent<DataLoader> ();
				DontDestroyOnLoad (Instance.gameObject);
			} else {
				Debug.LogWarning ("Trying to create another DataLoader object, just one is enough :)");
			}
		}

		public static void LoadData(bool isRemote, string path, Action<LoadDataResult> onCallbackEnd)
		{
			//Debug.Log(string.Format("Load File Contents: {0}", path));
			bool isRemoteHelp = isRemote;
			#if UNITY_EDITOR
			#elif UNITY_ANDROID
			//In Android even the Local Files have to be loaded as Remote
			if (!isRemoteHelp)
			{
				isRemoteHelp = true;
			}
			#endif
			if (isRemoteHelp) {
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

		public static void RemoteLoadingHeadersSt(string path, Action<LoadDataResult> onCallbackEnd, string postData, Dictionary<string, string> postHeaders)
		{
			Instance.StartCoroutine(Instance.RemoteLoadingHeaders (path, onCallbackEnd, postData, postHeaders));
		}

		public IEnumerator RemoteLoadingHeaders(string path, Action<LoadDataResult> onCallbackEnd, string postData, Dictionary<string, string> postHeaders)
		{
			if (IsVerbose) {
				Debug.Log ("Path: " + path);
			}
			byte[] bytes = null;
			//If a null is send on postdata, it means is a GET method
			if (postData != null) {
				bytes = System.Text.Encoding.UTF8.GetBytes (postData);
			}
			WWW www = new WWW(path, bytes, postHeaders);
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