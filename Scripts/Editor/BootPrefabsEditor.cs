namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
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
	[CustomEditor(typeof(BootPrefabs))]
	public class BootPrefabsEditor : Editor {


		#region Private Static Methods

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void BeforeSceneLoad() {
			if (FindObjectOfType<BootInstantiator>() == null) {
				InstantiateBootPrefabs();
			}
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
				}
			}
		}

		private static bool CanRunInActiveScene(BootPrefabs bootPrefabs) {
			if (!bootPrefabs.BootOnlyOnSpecificScenes) { // <- Can run in any scene
				return true;
			} else {
				// Only run if the active scene is allowed.
				for (int i = 0; i < bootPrefabs.SpecificScenesCount; i++) {
					SceneAsset sceneAsset = bootPrefabs.GetSpecificSceneAt(i);
					if(SceneManager.GetActiveScene().path == AssetDatabase.GetAssetPath(sceneAsset)) {
						return true;
					}
				}
				return false;
			}
		}

		#endregion


		#region Unity Methods

		public override void OnInspectorGUI() {
			serializedObject.Update();
			EditorGUILayout.Space();
			DrawHelpBox();
			EditorGUILayout.Space();
			PrefabsList.DoLayoutList();
			DrawBootOnlyOnSpecificScenes();
			DrawSpecificScenes();
			serializedObject.ApplyModifiedProperties();
		}

		#endregion


		#region Private Fields

		private ReorderableList m_PrefabsList;

		private SerializedProperty m_PrefabsProperty;

		private ReorderableList m_SpecificScenesList;

		private SerializedProperty m_SpecificScenesProperty;

		private SerializedProperty m_BootOnlyOnSpecificScenesProperty;

		#endregion


		#region Private Properties

		private ReorderableList PrefabsList {
			get {
				if(m_PrefabsList == null) {
					m_PrefabsList = new ReorderableList(serializedObject, PrefabsProperty);
					m_PrefabsList.drawHeaderCallback = PrefabsList_drawHeaderCallback;
					m_PrefabsList.drawElementCallback = PrefabsList_drawElementCallback;
				}
				return m_PrefabsList;
			}
		}

		private SerializedProperty PrefabsProperty {
			get { return m_PrefabsProperty = m_PrefabsProperty ?? serializedObject.FindProperty("m_Prefabs"); }
		}

		private ReorderableList SpecificScenesList {
			get {
				if (m_SpecificScenesList == null) {
					m_SpecificScenesList = new ReorderableList(serializedObject, SpecificScenesProperty);
					m_SpecificScenesList.drawHeaderCallback = SpecificScenes_drawHeaderCallback;
					m_SpecificScenesList.drawElementCallback = SpecificScenes_drawElementCallback;
				}
				return m_SpecificScenesList;
			}
		}

		private SerializedProperty SpecificScenesProperty {
			get { return m_SpecificScenesProperty = m_SpecificScenesProperty ?? serializedObject.FindProperty("m_SpecificScenes"); }
		}

		private SerializedProperty BootOnlyOnSpecificScenesProperty {
			get { return m_BootOnlyOnSpecificScenesProperty = m_BootOnlyOnSpecificScenesProperty ?? serializedObject.FindProperty("m_BootOnlyOnSpecificScenes"); }
		}

		#endregion


		#region Private Methods

		private void DrawHelpBox() {
			if (BootOnlyOnSpecificScenesProperty.boolValue) {
				EditorGUILayout.HelpBox(
					"These prefabs will be instantiated if no BootInstantiator is found in the " +
					"active scene, but only if the active scene is one of the specific scenes.",
					MessageType.Info
				);
			} else {
				EditorGUILayout.HelpBox(
					"These prefabs will be instantiated if no BootInstantiator is found in the " +
					"active scene.",
					MessageType.Info
				);
			}
		}

		private void PrefabsList_drawHeaderCallback(Rect rect) {
			EditorGUI.LabelField(rect, ObjectNames.NicifyVariableName(PrefabsProperty.name));
		}

		private void PrefabsList_drawElementCallback(Rect rect, int index, bool isActive, bool isFocused) {
			SerializedProperty prefabProperty = PrefabsProperty.GetArrayElementAtIndex(index);
			Rect propertyRect = rect;
			propertyRect.y += 2;
			propertyRect.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField(propertyRect, prefabProperty);
		}

		private void DrawBootOnlyOnSpecificScenes() {
			EditorGUIUtility.labelWidth = 160;
			EditorGUILayout.PropertyField(BootOnlyOnSpecificScenesProperty);
			EditorGUIUtility.labelWidth = 0;
		}

		private void DrawSpecificScenes() {
			if (BootOnlyOnSpecificScenesProperty.boolValue) {
				SpecificScenesList.DoLayoutList();
			}
		}

		private void SpecificScenes_drawHeaderCallback(Rect rect) {
			EditorGUI.LabelField(rect, ObjectNames.NicifyVariableName(SpecificScenesProperty.name));
		}

		private void SpecificScenes_drawElementCallback(Rect rect, int index, bool isActive, bool isFocused) {
			SerializedProperty prefabProperty = SpecificScenesProperty.GetArrayElementAtIndex(index);
			Rect propertyRect = rect;
			propertyRect.y += 2;
			propertyRect.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField(propertyRect, prefabProperty);
		}

		#endregion


	}
}