using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Mediatonic.Tools
{
	public class PackageExtractor : EditorWindow
	{
		private const string _defaultOutputPath = "./";
		private const string _defaultPackagePath = "./";

		private string _outputPath = "./";
		private string _packagePath = "./";

		[MenuItem("Window/Extract Package")]
		private static void ExtractHardcodedPackage()
		{
			GetWindow<PackageExtractor>().Show();
		}

		private void Awake()
		{
			_outputPath = Path.GetFullPath(_defaultOutputPath);
			_packagePath = Path.GetFullPath(_defaultPackagePath);
		}

		private void OnGUI()
		{
			EditorGUILayout.BeginHorizontal();
			GUI.enabled = false;
			EditorGUILayout.TextField("Unity package", _packagePath);
			GUI.enabled = true;
			if (GUILayout.Button("..."))
			{
				_packagePath = EditorUtility.OpenFilePanel("Find Unity Package", "./", "unitypackage");
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUI.enabled = false;
			EditorGUILayout.TextField("Destination", _outputPath);
			GUI.enabled = true;
			if (GUILayout.Button("..."))
			{
				_outputPath = EditorUtility.OpenFolderPanel("Output location", _outputPath, "");
			}
			EditorGUILayout.EndHorizontal();

			if (GUILayout.Button("Extract"))
			{
				try
				{
					ExtractPackage(_packagePath, _outputPath);
				}
				finally
				{
					EditorUtility.ClearProgressBar();
				}
			}
		}

		public static void ExtractPackage(string packagePath, string outPath = null)
		{
			string name = Path.GetFileNameWithoutExtension(packagePath);
			if (string.IsNullOrEmpty(outPath))
			{
				outPath = Application.dataPath;
			}

			outPath = Path.Combine(outPath, name);
			if (Directory.Exists(outPath))
			{
				throw new Exception($"Output path {outPath} already exists");
			}

			string workingDir = Path.Combine(outPath, ".working");

			if (Directory.Exists(workingDir))
			{
				Directory.Delete(workingDir, true);
			}
			Directory.CreateDirectory(workingDir);

			EditorUtility.DisplayProgressBar("Extracting", "Extracting package", 0.0f);
			var inStream = File.OpenRead(packagePath);
			var gzipStream = new GZipInputStream(inStream);
			var tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
			tarArchive.ExtractContents(workingDir);
			tarArchive.Close();
			gzipStream.Close();
			inStream.Close();

			var dirs = Directory.GetDirectories(workingDir);
			for (int i = 0; i < dirs.Length; ++i)
			{
				string dir = dirs[i];
				string assetPath = Path.Combine(dir, "asset");
				EditorUtility.DisplayProgressBar("Extracting", $"Moving {assetPath} to output folder", (float)i / dirs.Length);

				string pathnamePath = Path.Combine(dir, "pathname");
				if (!File.Exists(assetPath) || !File.Exists(pathnamePath))
				{
					continue;
				}

				string assetTargetPathRelative = File.ReadAllText(pathnamePath);
				string assetTargetPath = Path.Combine(outPath, assetTargetPathRelative);
				string assetTargetPathDir = Path.GetDirectoryName(assetTargetPath);
				if (!Directory.Exists(assetTargetPathDir))
				{
					Directory.CreateDirectory(assetTargetPathDir);
				}
				File.Move(assetPath, assetTargetPath);
			}

			Directory.Delete(workingDir, true);
		}

	}
}
