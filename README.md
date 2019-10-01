# Unity Package Extractor

## What?

This is a simple tool to extract `.unitypackage`s outside of Unity's package importer. This allows you to extract into a custom location, e.g. for a specific folder structure policy, which can be repeated for version upgrades etc.

Tested with Unity 2019.2.1f1

## Installation

Drop `PackageExtractor.cs` into an editor folder in your project along with `ICSharpCode.SharpZipLib.dll`

## Use

* Through the Editor window *Window/Extract Package*
* Programatically: call `PackageExtractor.ExtractPackage("packagepath.unitypackage", "outputpath");`
