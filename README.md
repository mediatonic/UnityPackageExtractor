# Unity Package Extractor

## What?

This is a simple tool to extract [Unity Asset packages](https://docs.unity3d.com/Manual/AssetPackages.html) (`.unitypackage`s) outside of Unity's package importer. This allows you to extract into a custom location, e.g. for a specific folder structure policy, which can be repeated for version upgrades etc.

Tested with Unity 2019.2.1f1

## Installation

### Manual

Drop `PackageExtractor.cs` into an editor folder in your project along with `ICSharpCode.SharpZipLib.dll`

### Using UPM (manifest)

Add the following to the manifest

    ```
	"dependencies": {
	"uk.co.mediatonic.unity-package-extractor": "https://github.com/hannesdelbeke/UnityPackageExtractor.git",
	...
	}
	```

### using UPM (GUI)

* in the menu window/packagemanager
* click + in left top corner
* add package from git URL
* enter the giturl of this project


## Use

* Through the Editor window *Window/Extract Package*
* Programatically: call `PackageExtractor.ExtractPackage("packagepath.unitypackage", "outputpath");`
