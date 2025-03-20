namespace CocodriloDog.App {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEditor.SceneManagement;
	using UnityEditorInternal;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	/// <summary>
	/// Custom editor for <see cref="BootPrefabs"/>. It instantiates the prefabs
	/// when no <see cref="BootInstantiator"/> is found. This would be the case
	/// when running the project from a scene other than the boot scene and helps
	/// to simulate a runtime state with the boot objects in place without needing
	/// to run the boot scene first.
	/// </summary>
	///
	/// <remarks>
	///	A <see cref="BootPrefabs"/> asset must be located in a Resources folder.
	/// </remarks>
	[CustomEditor(typeof(BootPrefabs))]
	public class BootPrefabsEditor : Editor {


		#region Private Static Methods

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void BeforeSceneLoad() {
			InstantiateBootPrefabs();
		}

		private static void InstantiateBootPrefabs() {
			// Look for the BootPrefabs assets.
			foreach (BootPrefabs bootPrefabs in Resources.LoadAll<BootPrefabs>("")) {
				if (CanRunInActiveScene(bootPrefabs)) {

					// Iterate through the prefabs
					for (int i = 0; i < bootPrefabs.PrefabsCount; i++) {
						// Instantiate each of the prefabs with PrefabUtility to preserve
						// the connection to the prefab.
						GameObject original = bootPrefabs.GetPrefabAt(i);
						GameObject clone = Instantiate<GameObject>(original);
						Debug.LogFormat("BootPrefabs: Instantiated boot object {0}", clone.name);
						clone.name = original.name;
					}

					// Mark this to prevent double instantiation
					bootPrefabs.HasInstantiatedPrefabs = true;

				}
			}
		}

		private static bool CanRunInActiveScene(BootPrefabs bootPrefabs) {

			var activeScenePath = SceneManager.GetActiveScene().path;

			return bootPrefabs.EditorInstantiationMode switch {

				BootPrefabsEditorInstantiationMode.InstantiatePrefabsInAllScenes => true,

				BootPrefabsEditorInstantiationMode.InstantiatePrefabsOnlyInSpecificScenes =>
					Enumerable.Range(0, bootPrefabs.SpecificScenesCount)
						.Select(i => bootPrefabs.GetSpecificSceneAt(i))
						.Any(scene => activeScenePath == AssetDatabase.GetAssetPath(scene)),

				BootPrefabsEditorInstantiationMode.InstantiatePrefabsInAllScenesExcept =>
					!Enumerable.Range(0, bootPrefabs.ExceptionScenesCount)
						.Select(i => bootPrefabs.GetExceptionSceneAt(i))
						.Any(scene => activeScenePath == AssetDatabase.GetAssetPath(scene)),

				_ => false

			};

		}

		#endregion


		#region Unity Methods

		private void OnEnable() {
			ScriptProperty = serializedObject.FindProperty("m_Script");
			PrefabsProperty = serializedObject.FindProperty("m_Prefabs");
			EditorInstantiationModeProperty = serializedObject.FindProperty("m_EditorInstantiationMode");
			SpecificScenesProperty = serializedObject.FindProperty("m_SpecificScenes");
			ExceptionScenesProperty = serializedObject.FindProperty("m_ExceptionScenes");
		}

		public override void OnInspectorGUI() {

			serializedObject.Update();

			CDEditorUtility.DrawDisabledField(ScriptProperty);

			EditorGUILayout.PropertyField(PrefabsProperty);

			DrawHelpBox1();

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(EditorInstantiationModeProperty);

			DrawHelpBox2();

			switch (EditorInstantiationModeProperty.enumValueIndex) {

				case (int)BootPrefabsEditorInstantiationMode.InstantiatePrefabsOnlyInSpecificScenes:
					EditorGUILayout.PropertyField(SpecificScenesProperty);
					break;

				case (int)BootPrefabsEditorInstantiationMode.InstantiatePrefabsInAllScenesExcept:
					EditorGUILayout.PropertyField(ExceptionScenesProperty);
					break;

			}

			serializedObject.ApplyModifiedProperties();

		}

		#endregion


		#region Private Properties

		private SerializedProperty ScriptProperty { get; set; }

		private SerializedProperty PrefabsProperty { get; set; }

		private SerializedProperty EditorInstantiationModeProperty { get; set; }

		private SerializedProperty SpecificScenesProperty { get; set; }

		private SerializedProperty ExceptionScenesProperty { get; set; }

		#endregion


		#region Private Methods

		private void DrawHelpBox1() {
			string message = "These prefabs will be instantiated by a BootInstantiator component in a scene.";
			EditorGUILayout.HelpBox(message, MessageType.Info);
		}

		private void DrawHelpBox2() {

			string message = EditorInstantiationModeProperty.enumValueIndex switch {

				(int)BootPrefabsEditorInstantiationMode.InstantiatePrefabsInAllScenes =>
					"Additionally, in the Unity editor, these prefabs will be instantiated in all scenes, " +
					"for testing purposes. " +
					"\n\nThis happens before any object awakes.",

				(int)BootPrefabsEditorInstantiationMode.InstantiatePrefabsOnlyInSpecificScenes =>
					"Additionally, in the Unity editor, these prefabs will be instantiated in the scenes " +
					"specified below, for testing purposes. " +
					"\n\nThis happens before any object awakes.",

				(int)BootPrefabsEditorInstantiationMode.InstantiatePrefabsInAllScenesExcept =>
					"Additionally, in the Unity editor, these prefabs will be instantiated in all scenes " +
					"except the ones specified below, for testing purposes. " +
					"\n\nThis happens before any object awakes.",

				_ => ""

			};

			EditorGUILayout.HelpBox(message, MessageType.Info);

		}

		#endregion


	}
}