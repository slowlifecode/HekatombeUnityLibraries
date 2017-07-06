using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


namespace Hekatombe.DataHelpers
{
	public class DataSaver {
		//This is an implementation that works on all tested platforms:
		//Recommended use on Application.persistentDataPath + "\\example.json"; //With two back-slashes!
		//Remember that on Application.streamingAssetsPath, should be loaded as a Remote file! 
		// -Editor
		// -Android
		public static string PersistFile(string path, string data)
		{
			try {
				StreamWriter w;
				FileInfo f = new FileInfo(path);
				if(!f.Exists)
				{
					f.Delete();
					w = f.CreateText(); 
				}
				else
				{
					w = f.CreateText();   
				}
				w.Write(data);
				w.Close();
			} catch (Exception e)
			{
				string strError = "PersistFile Error: " + e.Message;
				Debug.LogError(strError);
				return strError;
			}
			return "success";
		}

		public static bool Exists(string path)
		{
			FileInfo f = new FileInfo(path);
			return f.Exists;
		}
	}
}