<pre>
<head>
	<title>Super Camera Anchor Documentation</title>
	<link rel="shortcut icon" type="image/png" href="/SCADocImages/favicon.png"/>
</head>
<style>
* {
	box-sizing:border-box;
}
body {
    background-color: #170f02; 
    background-image: url("SCADocImages/background.png");
    background-repeat: no-repeat;
    background-position: center top;
    background-attachment: fixed;
    background-size: cover;
    font-family: "Trebuchet MS", Helvetica, sans-serif;
    color: #3c2707;
    padding:0 15px;
}
a {
    color: #64B0FF;
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
    background-color: #FFCD82;
    width: 100%;
	max-width:1200px;
    margin:30px auto;
    padding: 15px;
}
.banner {
    text-align: center;
    max-width: 100%;
    color: #e4941b;
}
.banner img {
    max-width: 100%;
}
.tableofcontents {
    background-color: #FFF;
    max-width: 50%;
    padding: 15px;
    margin: 15px;
}
.textarea {
    margin: 50px 50px;
}
.rightbox {
    color: #e4941b;
    text-align: center;
    font-style: italic;
    float: right;
    background-color: #FFF;
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
<img src="SCADocImages/banner.png"/><br>
<br>
Super Camera Anchor Documentation - v1.1
</article><article class="tableofcontents"><article class="textarea">
#Table of Contents

[Features](#Features)

1. [Adding Super Camera Anchor to Unity](#Adding Super Camera Anchor to Unity)
	* [Importing Super Camera Anchor into Unity](#Importing Super Camera Anchor into Unity)
	* [Adding Super Camera Anchor to a Scene](#Adding Super Camera Anchor to a Scene)
2. [Getting Started](#Getting Started)
	* [Tips & Tricks](#Tips & Tricks)
3. [Functions & Variables](#Functions & Variables)
	* [Public Functions](#Public Functions)
	* [Public Variables](#Public Variables)

[Notes](#Notes)
[ChangeLog](#ChangeLog)
[Credits](#Credits)
</article></article><article class="textarea">

***

<a name="Features"></a>
#Features
* Super Camera Anchor is a Unity tool meant to replace Unity UI. It can be used to pin 3D objects or sprites to a camera, doing exactly what Unity UI does but with more freedom.

***

<a name="Adding Super Camera Anchor to Unity"></a>
#1. Adding Super Camera Anchor to Unity
<a name="Importing Super Camera Anchor into Unity"></a>
##a. Importing Super Camera Anchor into Unity 

To import Super Camera Anchor into your project, either drag in the .unitypackage file, or get it from the asset store window.

###Imported folders, what they do, and if you need to import them or not, alphabetically:

* __"Documentation"__ contains these docs! You don't need to import them, but you probably should!
* __"Example"__ contains the demo scene. Not needed if you don't want the demo.

The rest of the files are required.
Anchor.obj is used as a gizmo, Icon.png is the script's icon, and SuperCameraAnchor.cs is the actual code!

<a name="Adding Super Camera Anchor to a Scene"></a>
##b. Adding Super Camera Anchor to a Scene

To add Super Camera Anchor to an object, select "Add Component" in the inspector of an existing object. It can be found under "Utility > Super Camera Anchor".

***

<a name="Getting Started"></a>
#2. Getting Started
<a name="Tips & Tricks"></a>
##a. Tips & Tricks

* Mimicking Unity UI
	You can use camera culling masks to make it so UI is drawn on top of everything else, seperate from your scene. First, create a new camera to be used as your UI camera, and set the culling mask to only see the "UI" layer. Next, on your game's main camera, set it to ignore the "UI" layer. Finally, on each anchor set the Layer to be "UI", and the "cam" variable to the new UI camera.

<article class="rightbox">
<a href="SCADocImages/inspector1.png"><img src="SCADocImages/inspector1.png"/></a><br><br>
The inspector window.
</article>

***

<a name="Functions & Variables"></a>
#3. Functions & Variables

<a name="Public Functions"></a>
##a. Public Functions

* ###void SuperCameraAnchor.Refresh()
	Forces Super Camera Anchor to update. For setting resolution manually.

* ###[STATIC] void SuperCameraAnchor.RefreshAll()
	Calls Refresh() on every Super Camera Anchor in the scene. Should only be called when loading a scene, or when player changes resolution, as it uses FindGameobjectsOfType(), which is intensive.

<a name="Public Variables"></a>
##b. Public Variables

* ###[READ ONLY] Transform SuperCameraAnchor.t
	A quick reference to the transform on this object.

* ###bool SuperCameraAnchor.checkForResolutionChange
	Controls whether the anchor will automatically check to see if the resolution has changed in play mode or not.

* ###bool SuperCameraAnchor.followCameraMovement
	Controls if the object will follow the camera when it moves/rotates. This bool isn't used if the anchor is a child of the camera.

* ###Camera SuperCameraAnchor.cam
	The camera the anchor is being pinned to.

* ###float SuperCameraAnchor.horizontalPosition
	Horizontal position of the object, in viewport space. (Ranged 0 to 1)

* ###float SuperCameraAnchor.verticalPosition
	Vertical position of the object, in viewport space. (Ranged 0 to 1)

* ###Vector3 SuperCameraAnchor.offset
	In world space, an offset from the horizontal/vertical position's anchor.

* ###float SuperCameraAnchor.distance
	The distance the anchor will be from the camera.

* ###SuperTextMesh.LookAtCamMode SuperTextMesh.lookAtCamMode
	The way the anchor will rotate, relative to the camera.

	* LookAtCamMode.Match - Match rotation with the camera.
	* LookAtCamMode.Stare - Face towards the camera.
	* LookAtCamMode.None - Don't do any rotation changes.

* ###Vector3 SuperTextMesh.lookOffset
	If lookAtCamMode is Match or Stare, this will control a rotational offset with euler angles. (t.rotation is used and shown in the inspector if lookAtCamMode is set to None)

***

<a name="Notes"></a>
#Notes
* Adding Super Camera Anchor to an object replaces its transform, but the transform is still there to reference in other scripts. You can quickly access this with "SuperCameraAnchor.t".
* If a prefab has a Super Camera Anchor component on it, it cannot remember which camera it was linked to. Whenever the camera isn't set (including when Super Camera Anchor is first added to a scene), it will prioritize find a camera in this order: Camera in parent > main camera > any camera.

##Known Bugs
* None at the moment!

##Planned Features / To Do List:
* Scaling options, similar to Unity UI.

***

<a name="ChangeLog"></a>
#ChangeLog
###v1.0
* Initial release!

###v1.0.1
* Compiles properly now.

###v1.0.2
* Having a prefab with Super Camera Anchor on it will no longer crash when loading a new scene.
* Parent camera is prioritized over main camera when finding a camera, now.

###v1.1
* Removed manual refresh, as it was causing issues with Unity 2017.1

***

<a name="Credits"></a>
#Credits
Coding and design by Kai Clavier ([@KaiClavier](http://twitter.com/KaiClavier))  
Extra CSS help by Tak ([@takorii](http://twitter.com/takorii))  
<br>
<br>
</article>
</article>