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

		public static void GetRemoteOrCachedTexture(string url, Action<TextureCacheCallback> callback)
		{
			GetRemoteOrCachedTexture (url, callback, null, EAdaptProportion.No);
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
			TextureCacheManager._instance.StartCoroutine(doLoad(www, filePath, url, web, callback, refImage, adaptProportion));
		}

		static IEnumerator doLoad(WWW www, string filePath, string originalUrl, bool web, Action<TextureCacheCallback> callback, RawImage refImage, EAdaptProportion adaptProportion)
		{
			yield return www;
			string message = "";
			Texture2D tex = null;

			if (www.error == null)
			{
				if (web)
				{
					//System.IO.Directory.GetFiles
					Debug.Log("Saving Download Image  " + originalUrl + " to " + filePath);
					// string fullPath = filePath;
					File.WriteAllBytes(filePath, www.bytes);
					message = "Saving DONE  " + originalUrl + " to " + filePath;
					//Debug.Log("FILE ATTRIBUTES  " + File.GetAttributes(filePath));
					//if (File.Exists(fullPath))
					// {
					//    Debug.Log("File.Exists " + fullPath);
					// }
				}
				else
				{
					message = "Success Load Cached image: " + originalUrl;
				}
				//Enganxa-la a la Imatge
				tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
				www.LoadImageIntoTexture(tex);
				www.Dispose();
				www = null;
				//If there is a RawImage attached, paste it automatically
				//Warning: It's not reccomended to do it this way: Better to get the image on Callback and do with it whatever you want
				if (refImage != null) {
					AttachToRawImage(refImage, tex, adaptProportion);
				}
			}
			else
			{
				if (!web)
				{
					File.Delete(filePath);
				}
				message = "WWW ERROR " + www.error + "\nURL: " + originalUrl;
			}
			Debug.Log(message + " Path: " + filePath);
			if (callback != null) {
				string strError = null;
				//Pot ser que hagi borrat l'objecte www al carregar correctament la imatge
				if (www != null)
				{
					strError = www.error;
				}
				callback (new TextureCacheCallback(tex, refImage, originalUrl, message, strError));
			}
		}
			
		public static void AttachToRawImage(RawImage rawImage, Texture2D tex, EAdaptProportion adaptProportion)
		{
			rawImage.texture = tex;
			Resources.UnloadUnusedAssets ();
			RectTransform rt = rawImage.rectTransform;
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

		/*************
		 * Load a List of Images
		 */
		private static List<string> _images;
		private static int _indexLoadImage;
		private static Action _onLoadImagesEndCallback;
		private static Action<string> _onLoadOneImageErrorCallback;

		public static void LoadImageList(List<string> images, Action onEndCallback, Action<string> onLoadOneImageErrorCallback)
		{
			_images = images;
			_indexLoadImage = 0;
			_onLoadImagesEndCallback = onEndCallback;
			_onLoadOneImageErrorCallback = onLoadOneImageErrorCallback;
			LoadOneImageList(null);
		}

		private static void LoadOneImageList(TextureCacheCallback tc)
		{
			//Informa del resultat de la descàrrega
			if (tc != null && !tc.HasBeenSuccess() && _onLoadOneImageErrorCallback != null)
			{
				_onLoadOneImageErrorCallback(tc.Message);
			}
			//Si es la ultima imatge
			if (_indexLoadImage >= _images.Count)
			{
				_onLoadImagesEndCallback();
				return;
			}
			//Carrega la imatge
			TextureCacheManager.GetRemoteOrCachedTexture(_images[_indexLoadImage], LoadOneImageList);
			_indexLoadImage++;
		}
	}

	public class TextureCacheCallback
	{
		public Texture2D Texture;
		public RawImage RawImage;
		//URL: Useful to compare if the image it has being loaded is the same it was called, in case the user has switched to another page
		public string Url;
		public string Message;
		public string Error;

		public TextureCacheCallback(Texture2D tex, RawImage rawImage, string url, string message, string error)
		{
			Texture = tex;
			RawImage = rawImage;
			Url = url;
			Message = message;
			Error = error;
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