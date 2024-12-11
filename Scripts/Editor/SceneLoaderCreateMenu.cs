namespace CocodriloDog.App {

	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Creates a SceneLoader instance in the project panel.
	/// </summary>
	public class SceneLoaderCreateMenu {


		#region Private Static Methods

		[MenuItem("Assets/Create/Cocodrilo Dog/App/Scene Loader", false)]
		private static void CreateSceneLoader() {

			// Get the selected folder path, or default to the root Assets folder
			string folderPath = GetSelectedFolderPath() ?? "Assets";

			GameObject prefab = LoadPrefab();
			if (prefab == null) return;

			GameObject instance = GameObject.Instantiate(prefab);
			instance.name = prefab.name;

			string newPrefabPath = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{instance.name}.prefab");
			PrefabUtility.SaveAsPrefabAsset(instance, newPrefabPath);
			Object.DestroyImmediate(instance);

		}

		[MenuItem("Assets/Create/Cocodrilo Dog/App/Scene Loader", true)]
		private static bool ValidateCreateSceneLoader() {
			return true; // Always valid since we can fall back to the root Assets folder
		}

		private static GameObject LoadPrefab() {
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
			if (prefab == null) {
				Debug.LogError($"Prefab not found at path: {PrefabPath}");
			}
			return prefab;
		}

		private static string GetSelectedFolderPath() {
			foreach (Object obj in Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.Assets)) {
				string path = AssetDatabase.GetAssetPath(obj);
				if (AssetDatabase.IsValidFolder(path)) {
					return path;
				}
			}
			return null;
		}

		#endregion


		#region Private Constants

		private const string PrefabPath = "Packages/com.cocodrilodog.app/Prefabs/SceneLoader.prefab";

		#endregion


	}

}