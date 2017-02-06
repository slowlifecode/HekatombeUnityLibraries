using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

namespace Hekatombe.Utils
{
	public class TextureCacheManager : MonoBehaviour {

		private static TextureCacheManager _instance;

		public static void Init()
		{
			if (_instance == null)
			{
				GameObject go = new GameObject ();
				go.name = "TextureCacheManager";
				_instance = go.AddComponent<TextureCacheManager> ();
				DontDestroyOnLoad (_instance.gameObject);
			} else {
				Debug.LogWarning ("Trying to create another TextureCacheManager object, just one is enough :)");
			}
		}

		public static void SetCachedTextureToRawImage(string url, RawImage refImage)
		{
			GetRemoteOrCachedTexture (url, null, refImage);
		}

		public static void GetRemoteOrCachedTexture(string url, Action<TextureCacheCallback> callback)
		{
			GetRemoteOrCachedTexture (url, callback, null);
		}

		public static void GetRemoteOrCachedTexture(string url, Action<TextureCacheCallback> callback, RawImage refImage)
		{
			string filePath = Application.persistentDataPath;
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(url);
			filePath += "/" + System.Convert.ToBase64String(plainTextBytes);
			//string loadFilepath = filePath;
			bool web = false;
			WWW www;
			bool useCached = false;
			useCached = System.IO.File.Exists(filePath);
			if (useCached)
			{
				//check how old
				System.DateTime written = File.GetLastWriteTimeUtc(filePath);
				System.DateTime now = System.DateTime.UtcNow;
				double totalHours = now.Subtract(written).TotalHours;
				if (totalHours > 300)
				{
					//useCached = false;
				}
			}
			if (System.IO.File.Exists(filePath))
			{
				string pathforwww = "file://" + filePath;
				Debug.Log("TRYING FROM CACHE " + url + "  file " + pathforwww);
				www = new WWW(pathforwww);
			}
			else
			{
				web = true;
				www = new WWW(url);
			}
			TextureCacheManager._instance.StartCoroutine(doLoad(www, filePath, web, callback, refImage));
		}

		static IEnumerator doLoad(WWW www, string filePath, bool web, Action<TextureCacheCallback> callback, RawImage refImage)
		{
			yield return www;
			string message = "";

			if (www.error == null)
			{
				if (web)
				{
					//System.IO.Directory.GetFiles
					Debug.Log("Saving Download Image  " + www.url + " to " + filePath);
					// string fullPath = filePath;
					File.WriteAllBytes(filePath, www.bytes);
					message = "Saving DONE  " + www.url + " to " + filePath;
					//Debug.Log("FILE ATTRIBUTES  " + File.GetAttributes(filePath));
					//if (File.Exists(fullPath))
					// {
					//    Debug.Log("File.Exists " + fullPath);
					// }
				}
				else
				{
					message = "Success Load Cached image: " + www.url;
				}
				//Enganxa-la a la Imatge
				if (refImage != null) {
					Texture2D tex;
					tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
					www.LoadImageIntoTexture(tex);
					refImage.texture = tex;
				}
			}
			else
			{
				if (!web)
				{
					File.Delete(filePath);
				}
				message = "WWW ERROR " + www.error;
			}
			Debug.Log(message);
			if (callback != null) {
				callback (new TextureCacheCallback(www, message));
			}
		}
	}

	public class TextureCacheCallback
	{
		public string Message;
		public WWW Www;

		public TextureCacheCallback(WWW www, string message)
		{
			Www = www;
			Message = message;
		}
	}
}