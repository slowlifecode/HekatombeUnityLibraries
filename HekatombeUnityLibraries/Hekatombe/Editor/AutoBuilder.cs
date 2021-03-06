﻿
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace Hekatombe.Base
{

    public static class AutoBuilder {

        static string ProjectName
        {
            get
            {
                string[] s = Application.dataPath.Split(Path.DirectorySeparatorChar);
                return s[s.Length - 2];
            }
        }

        static string[] ScenePaths
        {
            get
            {
                List<string> scenes = new List<string>();
                for(int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                {
                	if(EditorBuildSettings.scenes[i].enabled)
                	{
                		scenes.Add(EditorBuildSettings.scenes[i].path);
                	}
                }
                return scenes.ToArray();
            }
        }

        static string[] GetCommandLineArgs(string name)
        {
            List<string> values = new List<string>();

            string[] arguments = System.Environment.GetCommandLineArgs();
            foreach (var arg in arguments)
            {
                string argName = "+" + name + "=";
                if (arg.StartsWith(argName))
                {
                    values.Add(arg.Substring(argName.Length));
                }
            }
            return values.ToArray();
        }

        static void SetDefines(BuildTargetGroup group)
        {    
            string[] defines = GetCommandLineArgs("defines");
            string define = string.Empty;

            foreach(var def in defines)
            {
                define += def.ToUpper() + ";";
            }

            if(!string.IsNullOrEmpty(define))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, define);
            }
        }

        [MenuItem("File/AutoBuilder/Mac OSX/Intel")]
        static void PerformOSXIntelBuild ()
        {
            SetDefines(BuildTargetGroup.Standalone);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneOSXIntel);
            BuildPipeline.BuildPlayer(ScenePaths, "Builds/OSX-Intel/" + ProjectName + ".app",BuildTarget.StandaloneOSXIntel,BuildOptions.None);
        }

        [MenuItem("File/AutoBuilder/iOS")]
        static void PerformiOSBuild ()
        {
            SetDefines(BuildTargetGroup.iOS);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
            BuildPipeline.BuildPlayer(ScenePaths, "Builds/iOS",BuildTarget.iOS,BuildOptions.None);
        }

        [MenuItem("File/AutoBuilder/Android")]
        static void PerformAndroidBuild()
        {
            SetDefines(BuildTargetGroup.Android);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
            EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.ETC;
            EditorPrefs.SetString("AndroidSdkRoot", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/Development/android-sdk/");
            BuildPipeline.BuildPlayer(ScenePaths, "Builds/Android/" + ProjectName + ".apk", BuildTarget.Android, BuildOptions.None);
        }
    }
}
