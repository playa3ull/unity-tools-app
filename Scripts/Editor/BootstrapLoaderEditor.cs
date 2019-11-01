namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEditor.SceneManagement;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	[CustomEditor(typeof(BootstrapLoader))]
	public class BootstrapLoaderEditor : Editor {


		#region Static Event Handlers

		private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj) {
			switch(obj) {
				case PlayModeStateChange.ExitingEditMode:
					foreach(BootstrapLoader loader in Resources.LoadAll<BootstrapLoader>("")) {
						EditorSceneManager.OpenScene(
							AssetDatabase.GetAssetPath(loader.Scene), 
							OpenSceneMode.Additive
						);
					}
					break;
				case PlayModeStateChange.EnteredEditMode:
					foreach (BootstrapLoader loader in Resources.LoadAll<BootstrapLoader>("")) {
						EditorSceneManager.CloseScene(
							EditorSceneManager.GetSceneByPath(AssetDatabase.GetAssetPath(loader.Scene)), 
							true
						);
					}
					break;
			}
		}

		#endregion


		#region Private Static Methods

		[InitializeOnLoadMethod]
		private static void InitializeOnLoad() {
			EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
		}

		#endregion


	}
}