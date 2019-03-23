//Copyright (c) 2016-2017 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using System.Text;
using System.Security.Cryptography;

using System.Xml;
using System.Xml.Serialization;

public class SSL {
//JSON
	//Save
		public static void SaveJSON<T>(T obj, string fileName){ //save w/o encryption shorthand
			SaveJSON(obj, fileName, false);
		}
		public static void SaveJSON<T>(T obj, string fileName, bool obfuscate){ //mutiple things can be saved to the same path... nifty?
			SaveBytes(Encoding.UTF8.GetBytes(JsonUtility.ToJson(obj)), fileName, obfuscate);
		}
	//Load replace
		public static void LoadJSON<T>(ref T obj, string fileName){ //load w/o encryption shorthand
			LoadJSON(ref obj, fileName, false);
		}
		public static void LoadJSON<T>(ref T obj, string fileName, bool obfuscate){ //no folder name supplied, generate one based on this game
			LoadJSON(ref obj, fileName, Application.companyName, Application.productName, Application.identifier, obfuscate);
		}
		//load from other game
		public static void LoadJSON<T>(ref T obj, string fileName, string devName, string gameName, bool obfuscate){ //load from other game
			LoadJSON(ref obj, fileName, devName, gameName, Application.identifier, obfuscate);
		}
		//load from other game, mobile/universal
		public static void LoadJSON<T>(ref T obj, string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate){ //path is the complete path from the appdata folder to the file
			JsonUtility.FromJsonOverwrite(Encoding.UTF8.GetString(LoadBytes(fileName, devName, gameName, bundleIdentifier, obfuscate)),obj);
		}
	//Load new
		public static T LoadJSON<T>(string fileName){ //load w/o encryption shorthand
			return LoadJSON<T>(fileName, false);
		}
		public static T LoadJSON<T>(string fileName, bool obfuscate){ //load from this game
			return LoadJSON<T>(fileName, Application.companyName, Application.productName, Application.identifier, obfuscate);
		}
		public static T LoadJSON<T>(string fileName, string devName, string gameName, bool obfuscate){ //load from other game
			return LoadJSON<T>(fileName, devName, gameName, Application.identifier, obfuscate);
		}
		public static T LoadJSON<T>(string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate){
			return JsonUtility.FromJson<T>(Encoding.UTF8.GetString(LoadBytes(fileName, devName, gameName, bundleIdentifier, obfuscate)));
		}
//XML
	//Save
		public static void SaveXML<T>(T obj, string fileName){//save w/o encryption shorthand
			SaveXML<T>(obj, fileName, false);
		}
		public static void SaveXML<T>(T obj, string fileName, bool obfuscate){
			var serializer = new XmlSerializer(obj.GetType());
			byte[] data;
			using(var ms = new MemoryStream()) {
				serializer.Serialize(ms, obj);
				data = ms.ToArray();
			}
			SaveBytes(data, fileName, obfuscate);
		}
	//Load replace
		public static void LoadXML<T>(ref T obj, string fileName){ //load w/o encryption shorthand
			LoadXML(ref obj, fileName, Application.companyName, Application.productName, Application.identifier, false);
		}
		public static void LoadXML<T>(ref T obj, string fileName, bool obfuscate){ //no folder name supplied, generate one based on this game
			LoadXML(ref obj, fileName, Application.companyName, Application.productName, Application.identifier, obfuscate);
		}
		//load from other game
		public static void LoadXML<T>(ref T obj, string devName, string gameName, string fileName, bool obfuscate){
			//use app's bundle id here cause it wont matter
			LoadXML(ref obj, fileName, devName, gameName, Application.identifier, obfuscate);
		}
		//load from other game, mobile/universal.
		public static void LoadXML<T>(ref T obj, string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate){ //path is the complete path from the appdata folder to the file
			var serializer = new XmlSerializer(obj.GetType());
			byte[] data = LoadBytes(fileName, devName, gameName, bundleIdentifier, obfuscate);
			using(var ms = new MemoryStream(data)) {
				obj = (T)serializer.Deserialize(ms);
			}
		}
	//Load new
		public static T LoadXML<T>(string fileName){ //load w/o encryption shorthand
			return LoadXML<T>(fileName, Application.companyName, Application.productName, Application.identifier, false);
		}
		public static T LoadXML<T>(string fileName, bool obfuscate){ //no folder name supplied, generate one based on this game
			return LoadXML<T>(fileName, Application.companyName, Application.productName, Application.identifier, obfuscate);
		}
		public static T LoadXML<T>(string devName, string gameName, string fileName, bool obfuscate){//load from other game
			//use app's bundle id here cause it wont matter
			return LoadXML<T>(fileName, devName, gameName, Application.identifier, obfuscate);
		}
		public static T LoadXML<T>(string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate){ //path is the complete path from the appdata folder to the file
			var serializer = new XmlSerializer(typeof(T));
			byte[] data = LoadBytes(fileName, devName, gameName, bundleIdentifier, obfuscate);
			using(var ms = new MemoryStream(data)) {
				return (T)serializer.Deserialize(ms);
			}
		}
//Text
	//Save
		public static void SaveTXT(string text, string fileName){ //shorthand for saving without encryption
			SaveTXT(text, fileName, false);
		}
		public static void SaveTXT(string text, string fileName, bool obfuscate){
			SaveBytes(Encoding.UTF8.GetBytes(text), fileName, obfuscate);
		}
	//Load, or read file as string
		 //return file as a string, assume no encryption
		public static string LoadTXT(string fileName){
			return LoadTXT(fileName, Application.companyName, Application.productName, Application.identifier, false);
		}
		public static string LoadTXT(string fileName, bool obfuscate){
			return LoadTXT(fileName, Application.companyName, Application.productName, Application.identifier, obfuscate);
		}
		//return file as a string
		public static string LoadTXT(string fileName, string devName, string gameName){
			return LoadTXT(fileName, devName, gameName, Application.identifier, false);
		}
		public static string LoadTXT(string fileName, string devName, string gameName, bool obfuscate){
			return LoadTXT(fileName, devName, gameName, Application.identifier, obfuscate);
		}
		public static string LoadTXT(string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate){
			return Encoding.UTF8.GetString( LoadBytes(fileName,devName,gameName,bundleIdentifier,obfuscate) );
		}
//Images
	//Save PNG
		public static void SavePNG(Texture2D tex, string fileName){
			SaveBytes(tex.EncodeToPNG(), fileName, false);
		}
		public static void SavePNG(Texture2D tex, string fileName, bool obfuscate){
			SaveBytes(tex.EncodeToPNG(), fileName, obfuscate);
		}
	//Save JPEG
		public static void SaveJPEG(Texture2D texture, int jpegQuality, string fileName){
			SaveBytes(texture.EncodeToJPG(jpegQuality), fileName, false);
		}
		public static void SaveJPEG(Texture2D texture, int jpegQuality, string fileName, bool obfuscate){
			SaveBytes(texture.EncodeToJPG(jpegQuality), fileName, obfuscate);
		}
	//Load Image
		//UX shortcuts
			public static Texture2D LoadPNG(string fileName){
				return LoadImage(fileName, false);
			}
			public static Texture2D LoadPNG(string fileName, bool obfuscate){
				return LoadImage(fileName, obfuscate);
			}
			public static Texture2D LoadPNG(string fileName, string devName, string gameName, bool obfuscate){
				return LoadImage(fileName, devName, gameName, obfuscate);
			}
			public static Texture2D LoadPNG(string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate){
				return LoadImage(fileName, devName, gameName, bundleIdentifier, obfuscate);
			}
			public static Texture2D LoadJPEG(string fileName){
				return LoadImage(fileName, false);
			}
			public static Texture2D LoadJPEG(string fileName, bool obfuscate){
				return LoadImage(fileName, obfuscate);
			}
			public static Texture2D LoadJPEG(string fileName, string devName, string gameName, bool obfuscate){
				return LoadImage(fileName, devName, gameName, obfuscate);
			}
			public static Texture2D LoadJPEG(string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate){
				return LoadImage(fileName, devName, gameName, bundleIdentifier, obfuscate);
			}
		//Actual loading scripts
			public static Texture2D LoadImage(string fileName){
				return LoadImage(fileName, Application.companyName, Application.productName, Application.identifier, false);
			}
			public static Texture2D LoadImage(string fileName, bool obfuscate){
				return LoadImage(fileName, Application.companyName, Application.productName, Application.identifier, obfuscate);
			}
			public static Texture2D LoadImage(string fileName, string devName, string gameName, bool obfuscate){
				return LoadImage(fileName, devName, gameName, Application.identifier, obfuscate);
			}
			public static Texture2D LoadImage(string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate){
				Texture2D newTex = Texture2D.blackTexture;
				byte[] data = LoadBytes(fileName, devName, gameName, bundleIdentifier, obfuscate);
				if(data.Length > 0){
					newTex.LoadImage(data);
					newTex.name = fileName;
				}
				return newTex;
			}
//WWW
	//Load
		public static UnityWebRequest LoadWWW(string fileName, string devName, string gameName, string bundleIdentifier){
			return new UnityWebRequest("file://" + FullPath(devName, gameName, bundleIdentifier, fileName));
		}
//Bytes (internal)
	//Save
		public static void SaveBytes(byte[] data, string fileName, bool obfuscate){
			#if !UNITY_EDITOR && (UNITY_WEBPLAYER || UNITY_WEBGL)
			//#if UNITY_EDITOR
			Debug.Log("Saving to PlayerPrefs key " + fileName);
			PlayerPrefs.SetString(fileName, Encoding.UTF8.GetString(data) );
			#else //save to file
			if(obfuscate) CryptBytes(data, Application.companyName + Application.productName); //encrypt if told to
			string path = PathAndCreate(Application.persistentDataPath, fileName);
			Debug.Log("Encryption = " + obfuscate + ", Saving to " + path);
			WriteBytes(data,path);
			#endif
		}
	//Load
		public static byte[] LoadBytes(string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate){
			byte[] data = new byte[0];
			#if !UNITY_EDITOR && (UNITY_WEBPLAYER || UNITY_WEBGL)
			//#if UNITY_EDITOR
			if (!PlayerPrefs.HasKey(fileName)){
				Debug.Log("PlayerPref key '" + fileName + "' not found!");
			}else{
				Debug.Log("Loading from PlayerPrefs key " + fileName);
				data = Encoding.UTF8.GetBytes( PlayerPrefs.GetString(fileName) );
			}
			#else //load from file
			UnityWebRequest www = LoadWWW(fileName, devName, gameName, bundleIdentifier);
			if(string.IsNullOrEmpty(www.error)){ //no errors were found if it's null or empty
				Debug.Log("Encryption = " + obfuscate + ", Loading from " + www.url);
				data = www.downloadHandler.data;
				if(obfuscate) CryptBytes(data, devName + gameName);
			}else{
				Debug.Log("File doesn't exist at path " + www.url + " Make sure file name is correct, and Company Name/Product Name is case-sensitive, too!");
			}
			#endif
			return data;
		}
//Tools
	//Write byte array to file
		private static void WriteBytes(byte[] bytes, string path){
			FileStream stream = new FileStream(path,FileMode.Create);
			stream.Write(bytes,0,bytes.Length);
			stream.Close();
		}
	//Obfuscate bytes
		/*
		it's XOR since...
		password would be the same for every project anyway
		this is faster than other methods
		just meant to obfuscate, not provide encryption
		*/
		public static void CryptBytes(byte[] data, string password){
			byte[] keys = Encoding.UTF8.GetBytes(password);
			for(int i=0; i<data.Length; i++){
				data[i] = (byte)(data[i] ^ keys[i % keys.Length]);
			}
		}
	//Get appdata folder from any OS
		//NOTE: this will probably be broken for windows store apps
		public static string AppDataFolder(){ //get the root folder from Application.persistentDataPath (APPDATA/, Application Support/, Android/data/ )
			#if !UNITY_EDITOR && UNITY_IOS
			return Application.persistentDataPath; //since iOS can't load from other games anyway
			#else
			//go back twice in directory, even on android to get root folder, locally
			return Directory.GetParent( Directory.GetParent(Application.persistentDataPath).ToString() ).ToString();
			#endif
		}
	//Get full path to a file, even from another game
		public static string FullPath(string devName, string gameName, string bundleIdentifier, string fileName){
			//return full path to the file
			string path = AppDataFolder();
			#if !UNITY_EDITOR && UNITY_IOS
			//don't append game name, dev name or bundle!
			#elif !UNITY_EDITOR && UNITY_ANDROID
			path = Path.Combine(path,bundleIdentifier);
			path = Path.Combine(path,"files");
			#else
			path = Path.Combine(path,devName);
			path = Path.Combine(path,gameName);
			#endif
			path = PathDontCreate(path,fileName); //this should still work on iOS
			return path;
		}
	//Get a specific path from a string, creating files along the way. for saving files.
		public static string PathAndCreate(string path, string fileName){ //for when saving to "/dev name/game name/extra folder/myfile.txt", or similar
			//get a certain path, and create folders if they dont exist yet
			string[] fileLoc = fileName.Split('/');
			for(int i=0,iL=fileLoc.Length;i<iL;i++){
				path = Path.Combine(path,fileLoc[i]);
				if(i != iL - 1){//AND see if you need to create a directory at this pont, if it's not the last one!
					Directory.CreateDirectory(path); //will not create a new directory if it already exists, so run it anyway!
				}
			}
			return path;
		}
	//Get a path, without creating folders. for loading files
		public static string PathDontCreate(string path, string fileName){ //same as above script, but it doesn't create a folder if it doesn't exist. For loading, not saving
			string[] fileLoc = fileName.Split('/');
			for(int i=0,iL=fileLoc.Length;i<iL;i++){
				path = Path.Combine(path,fileLoc[i]);
			}
			return path;
		}
	//Check if a game exists. Does not work for iOS/WebGL
		//for loading other games, some doube-checking code you can use:
		public static bool GameExists(string devName, string gameName){
			return GameExists(devName, gameName, Application.identifier);
		}
		//for mobile/universal
		public static bool GameExists (string devName, string gameName, string bundleIdentifier){ //for mobile/universal
			string path = AppDataFolder();
			#if !UNITY_EDITOR && UNITY_ANDROID
			path = Path.Combine(path,bundleIdentifier);
			path = Path.Combine(path,"files");
			#else
			path = Path.Combine(path,devName);
			path = Path.Combine(path,gameName);
			#endif
			if(File.Exists(path)){
				return true;
			}
			return false;
		}
	//Check if a file OR directory exists
		public static bool Exists(string fileName){
			return Exists(Application.companyName, Application.productName, Application.identifier, fileName);
		}
		public static bool Exists(string devName, string gameName, string fileName){
			return Exists(devName, gameName, Application.identifier, fileName);
		}
		public static bool Exists(string devName, string gameName, string bundleIdentifier, string fileName){ //see if file OR directory exists
			if(FileExists(devName, gameName, bundleIdentifier, fileName)){
				Debug.Log("File '" + fileName + "' exists!");
				return true;
			}else if(DirectoryExists(devName, gameName, bundleIdentifier, fileName)){
				Debug.Log("Directory '" + fileName + "' exists!");
				return true;
			}
			Debug.Log("File/Directory '" + fileName + "' does not exist!");
			return false;
		}
	//check if a file exists (backend)
		private static bool FileExists(string devName, string gameName, string bundleIdentifier, string fileName){ //mobile/universal
			#if !UNITY_EDITOR && (UNITY_IOS || UNITY_WEBGL)
			if(devName == Application.companyName && gameName == Application.productName && Application.identifier == bundleIdentifier){ //checking locally works!
				return PlayerPrefs.HasKey(fileName);
			}
			Debug.Log("Cant check if file exists in other game on this platform! Returning false.");
			return false;
			#else
			return File.Exists( FullPath(devName, gameName, bundleIdentifier, fileName) );
			#endif
		}
	//Check if directory exists (backend)
		private static bool DirectoryExists(string devName, string gameName, string bundleIdentifier, string fileName){
			#if !UNITY_EDITOR && (UNITY_WEBPLAYER || UNITY_WEBGL)
			//cant do directory stuff on this platform
			Debug.Log("Cant check if directory exists on this platform! Returning false.");
			return false;
			#else
			return Directory.Exists( FullPath(devName, gameName, bundleIdentifier, fileName) );
			#endif
		}
	//Delete a file OR directory
		public static void Delete(string fileName){ //delete file OR directory
			//see if it's a file
			if(FileExists(Application.companyName, Application.productName, Application.identifier, fileName)){
				Debug.Log("A file of this name exists, deleting it!");
				DeleteFile(fileName);
			}else if(DirectoryExists(Application.companyName, Application.productName, Application.identifier, fileName)){
				Debug.Log("A directory of this name exists, deleting it!");
				DeleteDirectory(fileName);
			}
		}
	//delete a file (backend)
		private static void DeleteFile(string fileName){
			#if !UNITY_EDITOR && (UNITY_WEBPLAYER || UNITY_WEBGL)
			PlayerPrefs.DeleteKey(fileName);
			#else
			string path = FullPath(Application.companyName, Application.productName, Application.identifier, fileName);
			File.Delete(path);
			#endif
		}
	//delete a directory (backend)
		private static void DeleteDirectory(string fileName){
			#if !UNITY_EDITOR && (UNITY_WEBPLAYER || UNITY_WEBGL)
			Debug.Log("Directories cannot be deleted on this platform!");
			#else
			string path = FullPath(Application.companyName, Application.productName, Application.identifier, fileName);
			Directory.Delete(path, true); //crashes if u try to delete a file, just for directories
			#endif
		}
	//Clear all files in a directory
		public static void ClearDirectory(string fileName){
			string path = FullPath(Application.companyName, Application.productName, Application.identifier, fileName);
			Debug.Log("Clearing all files in '" + path + "'!");

			DirectoryInfo di = new DirectoryInfo(path);
			foreach (FileInfo file in di.GetFiles()){
				file.Delete();
			}
			foreach (DirectoryInfo dir in di.GetDirectories()){
				dir.Delete(true);
			}
		}
	//Get all filenames in a directory as a string array
		public static string[] GetFilesInDirectory(string fileName){
			string path = FullPath(Application.companyName, Application.productName, Application.identifier, fileName);
			Debug.Log("Getting all filenames in path '" + path + "'!");
			//get all files
			string[] allFiles = Directory.GetFiles(path);
			for(int i=0, iL=allFiles.Length; i<iL; i++){
				allFiles[i] = Path.GetFileName(allFiles[i]); //strip path data
			}
			//get all directories
			string[] allDirectories = Directory.GetDirectories(path);
			for(int i=0, iL=allDirectories.Length; i<iL; i++){
				allDirectories[i] = Path.GetFileName(allDirectories[i]); //strip path data
			}
			//combine arrays together
			string[] combined = new string[allFiles.Length + allDirectories.Length];
			Array.Copy(allFiles, combined, allFiles.Length);
			Array.Copy(allDirectories, 0, combined, allFiles.Length, allDirectories.Length);
			//sort alphabetically
			Array.Sort(combined, (x,y) => String.Compare(x, y));
			return combined;
		}
}
