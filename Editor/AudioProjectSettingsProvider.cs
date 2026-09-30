using UnityEditor;
using UnityEngine;

namespace Depra.Sound.Unity.Editor
{
	public static class AudioProjectSettingsProvider
	{
		private const string SETTINGS_ASSET_PATH = "Assets/Resources/Depra.Sound/AudioProjectSettings.asset";
		private static readonly AudioLibraryEditorGUI LIBRARY_EDITOR = new();

		public static AudioProjectSettings LoadTable() =>
			AssetDatabase.LoadAssetAtPath<AudioProjectSettings>(SETTINGS_ASSET_PATH);

		[SettingsProvider]
		private static SettingsProvider CreateProvider() => new("Project/Sound", SettingsScope.Project)
		{
			label = "Sound",
			guiHandler = Draw
		};

		private static void Draw(string searchContext)
		{
			var settings = LoadTable();
			if (settings == null)
			{
				EditorGUILayout.HelpBox(
					"Create project audio settings to author the banks compiled for the runtime API.",
					MessageType.Info);
				if (GUILayout.Button("Create Audio Project Settings", GUILayout.Height(28f)))
				{
					settings = CreateSettingsAsset();
				}
			}

			if (settings == null)
			{
				return;
			}

			using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
			{
				EditorGUILayout.LabelField("Project Audio Library", EditorStyles.boldLabel);
				if (settings.Banks.Count == 0)
				{
					EditorGUILayout.HelpBox("Add a bank to build the runtime audio library.", MessageType.Info);
				}
			}

			LIBRARY_EDITOR.Draw(settings);
		}

		private static AudioProjectSettings CreateSettingsAsset()
		{
			EnsureFolder("Assets", "Resources");
			EnsureFolder("Assets/Resources", "Depra.Sound");

			var settings = ScriptableObject.CreateInstance<AudioProjectSettings>();
			AssetDatabase.CreateAsset(settings, SETTINGS_ASSET_PATH);
			AssetDatabase.SaveAssets();
			Selection.activeObject = settings;

			return settings;
		}

		private static void EnsureFolder(string parent, string folderName)
		{
			var path = $"{parent}/{folderName}";
			if (!AssetDatabase.IsValidFolder(path))
			{
				AssetDatabase.CreateFolder(parent, folderName);
			}
		}
	}
}