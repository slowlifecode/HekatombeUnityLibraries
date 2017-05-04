using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using Hekatombe.Base;

namespace Hekatombe.Utils
{
	public class TextureCacheManager : MonoBehaviour {

		private static TextureCacheManager _instance;

		public enum EAdaptProportion
		{
			No,
			KeepHeight,
			KeepWidth,
			NativeSize
		}

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

		public static void SetCachedTextureToRawImage(string url, RawImage refImage, EAdaptProportion adaptProportion)
		{
			GetRemoteOrCachedTexture (url, null, refImage, adaptProportion);
		}

		public static void GetRemoteOrCachedTexture(string url, Action<TextureCacheCallback> callback, EAdaptProportion adaptProportion)
		{
			GetRemoteOrCachedTexture (url, callback, null, adaptProportion);
		}

		public static void GetRemoteOrCachedTexture(string url, Action<TextureCacheCallback> callback, RawImage refImage, EAdaptProportion adaptProportion)
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
			TextureCacheManager._instance.StartCoroutine(doLoad(www, filePath, web, callback, refImage, adaptProportion));
		}

		static IEnumerator doLoad(WWW www, string filePath, bool web, Action<TextureCacheCallback> callback, RawImage refImage, EAdaptProportion adaptProportion)
		{
			string url = www.url;
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
					www.Dispose();
					www = null;
					refImage.texture = tex;
					Resources.UnloadUnusedAssets ();
					RectTransform rt = refImage.rectTransform;
					//Adapt Image Size
					switch (adaptProportion) {
					case EAdaptProportion.No:
						break;
					case EAdaptProportion.KeepHeight:
						rt.sizeDelta = rt.sizeDelta.CopyVectorButModifyX ((rt.sizeDelta.y * tex.width) / tex.height);
						break;
					case EAdaptProportion.KeepWidth:
						rt.sizeDelta = rt.sizeDelta.CopyVectorButModifyX ((rt.sizeDelta.x * tex.height) / tex.width);
						break;
					case EAdaptProportion.NativeSize:
						rt.sizeDelta = new Vector2 (tex.width, tex.height);
						break;
					}
				}
			}
			else
			{
				if (!web)
				{
					File.Delete(filePath);
				}
				message = "WWW ERROR " + www.error + "\nURL: " + www.url;
			}
			Debug.Log(message + " Path: " + filePath);
			if (callback != null) {
				string strError = null;
				//Pot ser que hagi borrat l'objecte www al carregar correctament la imatge
				if (www != null)
				{
					strError = www.error;
				}
				callback (new TextureCacheCallback(strError, message, refImage));
			}
		}
	}

	public class TextureCacheCallback
	{
		public string Message;
		public string Error;
		public RawImage RawImage;

		public TextureCacheCallback(string error, string message, RawImage rawImage)
		{
			Error = error;
			Message = message;
			RawImage = rawImage;
		}

		//Basically it's been a Success if Error is null
		public bool HasBeenSuccess()
		{
			if (Error == null)
			{
				return true;
			}
			return false;
		}
	}
}