﻿using UnityEngine; using System.Collections; using System.IO; using UnityEditor;  namespace Dreamteck {     public static class ResourceUtility     {         private static string dreamteckFolder;         private static string dreamteckLocalFolder;         private static bool directoryIsValid = false;          static ResourceUtility()         {             string defaultPath = Application.dataPath + "/Dreamteck";             dreamteckFolder = EditorPrefs.GetString("Dreamteck.ResourceUtility.dreamteckProjectFolder", defaultPath);             if (!dreamteckFolder.StartsWith(Application.dataPath))             {                 dreamteckFolder = defaultPath;             }             if (!Directory.Exists(dreamteckFolder))             {                 dreamteckFolder = FindFolder(Application.dataPath, "Dreamteck");                 directoryIsValid = Directory.Exists(dreamteckFolder);             }             else             {                 directoryIsValid = true;             }             if (directoryIsValid)             {                 dreamteckLocalFolder = dreamteckFolder.Substring(Application.dataPath.Length + 1);                 EditorPrefs.SetString("Dreamteck.ResourceUtility.dreamteckProjectFolder", dreamteckFolder);             }         }          //Attempts to find the input directory pattern inside a given directory and if it fails, proceeds with looking up all subfolders         public static string FindFolder(string dir, string folderPattern)         {             if (folderPattern.StartsWith("/")) folderPattern = folderPattern.Substring(1);             if (!dir.EndsWith("/")) dir += "/";             if (folderPattern == "") return "";             string[] folders = folderPattern.Split('/');             if (folders.Length == 0) return "";             string foundDir = "";             try             {                 foreach (string d in Directory.GetDirectories(dir))                 {                     DirectoryInfo dirInfo = new DirectoryInfo(d);                     if (dirInfo.Name == folders[0])                     {                         foundDir = d;                         string searchDir = FindFolder(d, string.Join("/", folders, 1, folders.Length - 1));                         if (searchDir != "")                         {                             foundDir = searchDir;                             break;                         }                     }                 }                 if (foundDir == "")                 {                     foreach (string d in Directory.GetDirectories(dir))                     {                         foundDir = FindFolder(d, string.Join("/", folders));                         if (foundDir != "") break;                     }                 }             }             catch (System.Exception excpt)             {                 Debug.LogError(excpt.Message);                 return "";             }             return foundDir;         }          public static Texture2D LoadTexture(string dreamteckPath, string textureFileName)         {             string path = Application.dataPath + "/Dreamteck/" + dreamteckPath;             if (!Directory.Exists(path))             {                 path = FindFolder(Application.dataPath, "Dreamteck/" + dreamteckPath);                 if (!Directory.Exists(path)) return null;             }             if (!File.Exists(path + "/" + textureFileName)) return null;             byte[] bytes = File.ReadAllBytes(path + "/" + textureFileName);             Texture2D result = new Texture2D(1, 1);             result.name = textureFileName;             result.LoadImage(bytes);             return result;         }          public static Texture2D LoadTexture(string path)         {             if (!File.Exists(path)) return null;             byte[] bytes = File.ReadAllBytes(path);             Texture2D result = new Texture2D(1, 1);             FileInfo finfo = new FileInfo(path);             result.name = finfo.Name;             result.LoadImage(bytes);             return result;         }          public static Texture2D[] EditorLoadTextures(string dreamteckLocalPath)         {             string path = "Assets/" + dreamteckLocalFolder + "/" + dreamteckLocalPath;             string[] textureGUIDs = AssetDatabase.FindAssets("t:texture2d", new string[] { path });             Texture2D[] textures = new Texture2D[textureGUIDs.Length];             for (int i = 0; i < textureGUIDs.Length; i++)             {                 textures[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(textureGUIDs[i]));             }             return textures;         }          public static Texture2D EditorLoadTexture(string dreamteckLocalPath, string textureName)         {             string path = "Assets/" + dreamteckLocalFolder + "/" + dreamteckLocalPath + "/" + textureName + ".png";             Texture2D texture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;             return texture;         }     } }