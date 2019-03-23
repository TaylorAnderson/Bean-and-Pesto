<pre>
<head>
	<title>Super Save Loader Documentation</title>
	<link rel="shortcut icon" type="image/png" href="/SSLDocImages/favicon.png"/>
</head>
<style>
* {
	box-sizing:border-box;
}
body {
    background-color: #000000; 
    background-image: url("SSLDocImages/background.png");
    background-repeat: no-repeat;
    background-position: center top;
    background-attachment: fixed;
    background-size: 100% 100%;
    font-family: "Trebuchet MS", Helvetica, sans-serif;
    color: #000000;
    padding:0 15px;
}
a {
    color: #FF0000;
    font-weight: bold;
}
pre {
    font-family: "courier new", courier, monospace;
    font-size: 14px;
    font-weight: bold;
}
code {
    font-family: "courier new", courier, monospace;
    font-size: 14px;
    font-weight: bold;
}
.column{
    background-color: #FFFFFF;
    width: 100%;
    max-width:1200px;
    margin:30px auto;
    padding: 15px;
}
.banner {
    text-align: center;
    max-width: 100%;
    color: #000000;
}
.banner img {
    max-width: 100%;
}
.tableofcontents {
    background-color: #DDDDDD;
    max-width: 50%;
    padding: 15px;
    margin: 15px;
}
.textarea {
    margin: 50px 50px;
}
.rightbox {
    color: #444444;
    text-align: center;
    font-style: italic;
    float: right;
    background-color: #DDDDDD;
    padding: 15px;
    margin: 0px 0px 0px 15px;
    max-width: 33%;
}
.rightbox img {
    width: 100%;
}
h1 {
    font-weight: bold;
    font-size: 32px;
    text-decoration: underline;
}
h2 {
    font-weight: bold;
    font-size: 28px;
    text-decoration: underline;
}
h3 {
    font-weight: bold;
    font-size: 20px;
    text-decoration: underline;
}
h4 {
    font-weight: bold;
    font-size: 16px;
    text-decoration: underline;
}
hr {
    border-top: 2px solid #3c2707;
    border-bottom: 2px solid #3c2707;
    border-left: 2px solid #3c2707;
    border-right: 2px solid #3c2707;
}

/* Mobiley Styley */

@media only screen and (max-width: 1000px) { 

	.tableofcontents {
		max-width:100%;
	}
	
}

@media only screen and (max-width: 640px) { 

	.textarea {
		margin:15px;
		word-break:break-word;
	}
	.rightbox {
		float:none;
		max-width:100%;
		margin:0 0 15px;
		padding:5px;
	}
	ul{
		padding-left:0;
		list-style-type:none;
	}
	ol {
		padding-left:15px;
	}
	ul ul, ol ul {
		padding-left:20px;
		list-style-type:disc;
	}
	
}
</style></pre>
<br>
<br>
<br>
<article class="column"><article class="banner">
<img src="SSLDocImages/banner.png"/><br>
<br>
Super Save Loader Documentation - v2.1
</article><article class="tableofcontents"><article class="textarea">
#Table of Contents

[Features](#Features)

1. [Adding Super Save Loader to Unity](#Adding Super Save Loader to Unity)
2. [Getting Started](#Getting Started)
    * [Intro to Saving & Loading](#Intro to Saving & Loading)
    * [Attributes](#Attributes)
    * [Tips & Tricks](#Tipes & Tricks)
3. [Functions](#Functions)

[Notes](#Notes)
[ChangeLog](#ChangeLog)
[Credits](#Credits)
</article></article><article class="textarea">

***

<a name="Features"></a>
#Features
* Super Save Loader is an asset that simplifies saving and loading in games.
* Single-line saving & loading.
* Able to save any serializable object.
* Save strings as .txt files and save Texture2Ds as .png or .jpeg files.
* Can load save data from other games, or see if other save data exists on a device.
* Files can be automatically obfuscated to discourage tampering with save data.
* Multiplatform support!
* Super-fast saving and loading, both with and without encryption.

***

<a name="Adding Super Save Loader to Unity"></a>
#1. Adding Super Save Loader to Unity

To import Super Save Loader into your project, either drag in the .unitypackage file, or get it from the asset store window.

###Imported folders, what they do, and if you need to import them or not, alphabetically:

* __"Documentation"__ contains these docs! You don't need to import them, but you probably should!
* __"Example"__ contains the demo scene. Not needed if you don't want the demo.

The rest of the files are required.

***

<a name="Getting Started"></a>
#2. Getting Started

<a name="Intro to Saving & Loading"></a>
#a. Intro to Saving & Loading

For the following examples on saving, I will be using the JSON serializer. To use the XML serializer, just replace "JSON" with "XML" in your code!

* ### How to save

    This line of code tells Super Save Loader to save an object named "myObject" as "savedobject.save" without using encryption:

    ``SSL.SaveJSON(myObject, "savedobject.save", false);``

    This line of code is a shorthand for the above one, saving without encryption:

    ``SSL.SaveJSON(myObject, "savedobject.save");``

* ### How to load

    There are two ways to load objects, either as a new object, or as an overwrite to old ones. Code to overwrite an object named "myObject" from a file named "savedobject.json" would look like this:

    ``SSL.LoadJSON(ref myObject, "savedobject.json", false);``

    While the code for creating a new object would look like this:

    ``MyObjectType myObject = SSL.LoadJSON<MyObjectType>("savedobject.json", false);``

    The boolean at the end controls whether encryption is used or not, and a shorthand like the one above for saving works for loading, too.

* ### How to load data from other games

    To load data from other games, you have to provide the Developer Name and the Product Name. These will be the names of the folders within the [appdata folder or equivalent](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html). For example, if you were to download my game "Hotel Paradise", my Developer Name is "KaiClavier" and my Product Name is "HotelParadise", so the folder route would be ``[appdata folder/KaiClavier/HotelParadise/]``. You can find the developer and product names of a game this way.

    The functions to load are the same, just with two extra strings for the developer name and product name, respectively.

    ``SSL.LoadJSON(ref myObject, "savedobject.json", "KaiClavier", "HotelParadise", false);``

    If you're loading from another game on Android, you also need to provide the "bundle identifier". This can be left out if your project isn't on Android.

    ``SSL.LoadJSON(ref myObject, "savedobject.json", "KaiClavier", "HotelParadise", "my.bundle.identifier", false);``

    You can still load obfuscated data from other games!

    <a name="Attributes"></a>
    #b. Attributes

    To serialize custom objects, you __need to use attributes!__ Attributes (also called "sub properties") are those lines of text between brackets like ``[this]``. To use them, place them before a class/variable, either in the previous or same line. Also, if applicable, attributes can be stacked!

    <code>
    [System.Serializable] <= This attribute is required for it to be saved! You need to use this on custom classes!  
    public class MyObject{  
    &nbsp;int foo;  
    &nbsp;[System.NonSerialized] float bar; <= This attribute means "bar" will be excluded from being saved with MyObject  
    }
    </code>

    The following attributes are to make XML code look cleaner. You only need to use them if you're picky about files no one will see.

    <code>
    using System.Xml.Serialization; <= Put this at the top of your script to use these attributes.  
    
    [System.Serializable]  
    [XmlRoot("GameData")] <= Renames root class in the XML file to "Game"!  
    public class MyObject{ <= By default, "MyObject" would be the root class instead.  
    &nbsp;int foo;  
    &nbsp;[XmlAttribute("extra")] <= Changes the name of "bar" to "extra" in the XML file!  
    &nbsp;float bar;  
    }  
    </code>

    You cannot use [XmlAttribute("myLabel")] on Lists, Arrays, Vector3s, or any other class that contains more than one variable. It won't work, and it'll return an error.

<a name="Tips & Tricks"></a>
#c. Tips & Tricks

* ### How do I set my company name and product name?
    To set Product Name & Company Name for your game:
    Go to ``[Edit > Project Settings > Player]``, and they're the top two things in the inspector!

    To set the Bundle Identifier: (for mobile games)
    Go to ``[Edit > Project Settings > Player]``, then click on Android or iOS, then look for "Bundle Identifier"!

* ### Saving PNGs
	Make sure the textures you want to save are read/write enabled. Textures may also save improperly if mipmapping is enabled or transparency is disabled.

* ### Loading TXTs
    LoadTXT() can be used on non-txt files to get them as a string! Try it out with JSON or XML files.

* ### Custom Extentions
    Files dont have to end with .json or .txt, you can give them a made-up extention like .save or .playerdata

* ### Subfolders
    If you include forward slashes in your filename ("mysubfolder/anotherfolder/file.txt"), your files can be sorted into folders within the persistent data folder!

* ### Where is data actually saved to?
   [You can check out where SSL is saving your files here!](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html) When specifying a filename in all functions, the filename refers to a file local to this folder.

***

<a name="Functions"></a>
#3. Functions



* ### void SSL.SaveJSON(T object, string fileName);

* ### void SSL.SaveJSON(T object, string fileName, bool obfuscate);
	Save an object as JSON.


* ### void SSL.LoadJSON(ref T object, string fileName);

* ### void SSL.LoadJSON(ref T object, string fileName, bool obfuscate);

* ### void SSL.LoadJSON(ref T object, string fileName, string devName, string gameName, bool obfuscate);

* ### void SSL.LoadJSON(ref T object, string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate);
	Overwrite an object from a JSON file.


* ### T SSL.LoadJSON<T>(string fileName);

* ### T SSL.LoadJSON<T>(string fileName, bool obfuscate);

* ### T SSL.LoadJSON<T>(string fileName, string devName, string gameName, bool obfuscate);

* ### T SSL.LoadJSON<T>(string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate);
	Create a new object from a JSON file.




* ### void SSL.SaveXML(T object, string fileName);

* ### void SSL.SaveXML(T object, string fileName, bool obfuscate);
	Save an object as XML.


* ### void SSL.LoadXML(ref T object, string fileName);

* ### void SSL.LoadXML(ref T object, string fileName, bool obfuscate);

* ### void SSL.LoadXML(ref T object, string fileName, string devName, string gameName, bool obfuscate);

* ### void SSL.LoadXML(ref T object, string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate);
	Overwrite an object from a XML file.


* ### T SSL.LoadXML<T>(string fileName);

* ### T SSL.LoadXML<T>(string fileName, bool obfuscate);

* ### T SSL.LoadXML<T>(string fileName, string devName, string gameName, bool obfuscate);

* ### T SSL.LoadXML<T>(string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate);
	Create a new object from a XML file.




* ### void SSL.SaveTXT(string saveThis, string fileName);

* ### void SSL.SaveTXT(string saveThis, string fileName, bool obfuscate);
	Save a string as a TXT file.


* ### string SSL.LoadTXT(string fileName);

* ### string SSL.LoadTXT(string fileName, bool obfuscate);

* ### string SSL.LoadTXT(string fileName, string devName, string gameName, bool obfuscate);

* ### string SSL.LoadTXT(string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate);
	Return a TXT file as a string, or read any file as a string.




* ### void SSL.SavePNG(Texture2D saveThis, string fileName);

* ### void SSL.SavePNG(Texture2D saveThis, string fileName, bool obfuscate);
	Save a Texture2D as a PNG. Be wary of quality.


* ### void SSL.SaveJPEG(Texture2D saveThis, int quality, string fileName);

* ### void SSL.SaveJPEG(Texture2D saveThis, int quality, string fileName, bool obfuscate);

	Save a Texture2D as a JPEG. Can control JPEG compression levels with the quality integer, which goes from 0 to 100.


* ### Texture2D SSL.LoadPNG(string fileName, bool obfuscate);

* ### Texture2D SSL.LoadPNG(string fileName, string devName, string gameName, bool obfuscate);

* ### Texture2D SSL.LoadPNG(string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate);


* ### Texture2D SSL.LoadJPEG(string fileName, bool obfuscate);

* ### Texture2D SSL.LoadJPEG(string fileName, string devName, string gameName, bool obfuscate);

* ### Texture2D SSL.LoadJPEG(string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate);


* ### Texture2D SSL.LoadImage(string fileName, bool obfuscate);

* ### Texture2D SSL.LoadImage(string fileName, string devName, string gameName, bool obfuscate);

* ### Texture2D SSL.LoadImage(string fileName, string devName, string gameName, string bundleIdentifier, bool obfuscate);

	Load an image as a Texture2D. Function used doesn't actually matter, as long as its a PNG or a JPEG that's being loaded.
    



* ### bool SSL.GameExists(string devName, string gameName);

* ### bool SSL.GameExists(string devName, string gameName, string bundleIdentifier);
	Returns true if a game's save data exists on this device.


* ### bool SSL.Exists(string fileName);    

* ### bool SSL.Exists(string devName, string gameName, string fileName);

* ### bool SSL.Exists(string devName, string gameName, string bundleIdentifier, string fileName);
	Returns true if a specific save file exists on this device.


* ### void SSL.Delete(string fileName);
    Deletes the specified file or directory.


* ### void SSL.Clear(string directoryName);
    Clears all files and directories in the directory. If directory is left blank ( "" ), then all data in base folder is cleared.


* ### string[] SSL.GetFilesInDirectory(string directoryName);
    Returns the filenames of every file in the specified directory. If directory is left blank ( "" ), then base folder is checked.

***

<a name="Notes"></a>
#Notes

* Serializing primitives (int, string, etc) is currently unsupported with JSON serialization. This is currently unsupported by Unity and may be added in the future. If you try to save a primitive, it will produce a txt file with just "{}" in it. 
* The XOR method is used to obfuscate files. The reason this method is used rather than a proper encryption method like v1.0 used is because: 
    1. The XOR method is much faster than triple DES.
    2. The files are breakable anyway, as the password is predictable no matter what.
    3. It's meant to just make files harder to tamper with, not hold secure data.
* WebGL and iOS cannot read save data from other files, due to the platforms' restrictions.
* WebGL cannot save files with encryption, and saves to playerprefs instead, as this platform doesn't allow permenant local storage.

##Known Bugs
* None at the moment!

##Planned Features / To Do List:
* More shortcuts for saving and loading?
* Option to save images to desktop?
* A way to move files around?

***

<a name="ChangeLog"></a>

#ChangeLog

### v2.0
* Clean up and overhaul!
* Changed function names.
* Can now save images and strings as PNGs, JPEGs, and TXTs.

### v2.1
* Files and directories can be deleted, now!
* Added shortcut to check if a file exists locally.
* Can check if directories exist, now.
* Renamed "FileExists()" to "Exists()".
* Added a way to clear a directory, including the root directory.
* Can now return an array of strings with all filenames in a directory.


***

<a name="Credits"></a>
#Credits
Coding and design by Kai Clavier ([@KaiClavier](http://twitter.com/KaiClavier))  
Extra CSS help by Tak ([@takorii](http://twitter.com/takorii))  
<br>
<br>
</article>
</article>