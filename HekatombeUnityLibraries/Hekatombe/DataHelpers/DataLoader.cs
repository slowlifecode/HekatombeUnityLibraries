﻿using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Hekatombe.Base;
using System.Collections.Generic;
using Hekatombe.Utils;

namespace Hekatombe.DataHelpers
{
	public class LoadDataResult
	{
		public bool IsSuccess = false;
		public string Contents;
		public string Error = "Not loaded yet";
		public string ErrorMessage = "";

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

		//Pot ser "unauthorized" o "401 UNAUTHORIZED\r"
		public bool IsUnauthorized()
		{
			return Error.ToLower().Contains("unauthorized");
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
			LoadData(isRemote, path, onCallbackEnd, true);
		}

		public static void LoadData(bool isRemote, string path, Action<LoadDataResult> onCallbackEnd, bool isOnStreamingAssets)
		{
			if (IsVerbose)
			{
				Debug.Log(string.Format("Load File Contents: {0}", path));
			}
			bool isRemoteHelp = isRemote;
			#if UNITY_EDITOR
			#elif UNITY_ANDROID
			//In Android even the Local Files that ARE on StreamingAssets have to be loaded as Remote (But not the ones that are on Application.persistentDataPath)
			if (!isRemoteHelp && isOnStreamingAssets)
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
                Debug.LogError("Remote Loading Error: " + www.error + " Path: " + path);
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
				Debug.Log ("Path: " + path + " POST: " + postData);
			}
			byte[] bytes = null;
			//If a null is send on postdata, it means is a GET method
			if (postData != null) {
				bytes = System.Text.Encoding.UTF8.GetBytes (postData);
			}
			WWW www = new WWW(path, bytes, postHeaders);
			yield return www;
			if (string.IsNullOrEmpty(www.error))
			{
				onCallbackEnd (new LoadDataResult(true, www.text));
			}
			else
			{
				//Mira si és un json i té algun missatge
				JSONObject j = new JSONObject(www.text);
				string strError = www.error;
				if (j.HasField("message")){
					strError = WWW.UnEscapeURL(j.GetField("message").str).FixLineBreaks();
				}
				Debug.LogError("Remote Loading Error: " +strError+ " Error:" + www.error + " Text: " + www.text);
				onCallbackEnd (new LoadDataResult(false, strError));
			}
		}

		public void LocalLoading(string path, Action<LoadDataResult> onCallbackEnd)
		{
			if (!DataSaver.Exists (path)) {
				string error = "Local Loading Error: " + path + " not found!";
				onCallbackEnd (new LoadDataResult (false, error));
			} else {
				StreamReader r = File.OpenText(path);
				string data = r.ReadToEnd();
				r.Close();
				onCallbackEnd (new LoadDataResult (true, data));
			}
		}
	}
}