namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(RuntimeScriptableStorage))]
	public class PlayerScriptableStorageEditor : Editor {


		#region Unity Methods

		private void OnEnable() {
			EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
		}

		private void OnDisable() {
			EditorApplication.playModeStateChanged -= EditorApplication_PlayModeStateChanged;
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();
			DrawScript();
			EditorGUILayout.PropertyField(FilePathProperty); 
			EditorGUILayout.PropertyField(DefaultScriptableObjectProperty);
			DrawScriptableObjectHelpBox();
			DrawFileHelpBox();
			DrawMainButtons();
			DrawPlayerScriptableObject();
			DrawInspectButton();
			serializedObject.ApplyModifiedProperties();
			Repaint();
		}

		#endregion


		#region Event Handlers

		private void EditorApplication_PlayModeStateChanged(PlayModeStateChange obj) {
			if(obj == PlayModeStateChange.ExitingEditMode) {
				serializedObject.Update();
				RuntimeScriptableObjectProperty.objectReferenceValue = null;
				serializedObject.ApplyModifiedProperties();
			}
		}

		#endregion


		#region Private Fields

		private RuntimeScriptableStorage m_RuntimeScriptableStorage;

		private SerializedProperty m_ScriptProperty;

		private SerializedProperty m_FilePathProperty;

		private SerializedProperty m_DefaultScriptableObjectProperty;

		private SerializedProperty m_RuntimeScriptableObjectProperty;

		private SerializedProperty m_AssetsGUIDProperty;

		#endregion


		#region Private Properties

		private RuntimeScriptableStorage RuntimeScriptableStorage {
			get {
				if(m_RuntimeScriptableStorage == null) {
					m_RuntimeScriptableStorage = (RuntimeScriptableStorage)target;
				}
				return m_RuntimeScriptableStorage;
			}
		}

		private SerializedProperty ScriptProperty {
			get { return m_ScriptProperty = m_ScriptProperty ?? serializedObject.FindProperty("m_Script"); }
		}

		private SerializedProperty FilePathProperty {
			get { return m_FilePathProperty = m_FilePathProperty ?? serializedObject.FindProperty("FilePath"); }
		}

		private SerializedProperty DefaultScriptableObjectProperty {
			get { return m_DefaultScriptableObjectProperty = m_DefaultScriptableObjectProperty ?? serializedObject.FindProperty("m_DefaultScriptableObject"); }
		}

		private SerializedProperty RuntimeScriptableObjectProperty {
			get { return m_RuntimeScriptableObjectProperty = m_RuntimeScriptableObjectProperty ?? serializedObject.FindProperty("m_RuntimeScriptableObject"); }
		}

		private SerializedProperty AssetsGUIDProperty {
			get { return m_AssetsGUIDProperty = m_AssetsGUIDProperty ?? serializedObject.FindProperty("m_AssetsGUID"); }
		}

		#endregion


		#region Private Methods

		private void DrawScript() {
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(ScriptProperty);
			EditorGUI.EndDisabledGroup();
		}

		private void DrawScriptableObjectHelpBox() {
			if(DefaultScriptableObjectProperty.objectReferenceValue == null) {
				EditorGUILayout.HelpBox("A ScriptableObject must be assigned.", MessageType.Error);
			}
		}

		private void DrawFileHelpBox() {
			if (RuntimeScriptableStorage.FileExists) {
				EditorGUILayout.HelpBox(
					string.Format(
						"File \"{0}\" exists in the specified FilePath.", 
						Path.GetFileName(RuntimeScriptableStorage.FilePath)
					),
					MessageType.Info
				);
			} else {
				EditorGUILayout.HelpBox("No file found at the specified FilePath.", MessageType.Info);
			}
		}

		private void DrawMainButtons() {
			GUILayout.BeginHorizontal();
			{
				GUILayoutOption buttonWidth = GUILayout.Width((EditorGUIUtility.currentViewWidth - 30) / 3);
				EditorGUI.BeginDisabledGroup(!RuntimeScriptableStorage.FileExists);
				if (RuntimeScriptableObjectProperty.objectReferenceValue == null) {
					if (GUILayout.Button("Load", buttonWidth)) {
						// Make the internal field to be assigned by invoking the getter.
						_ = RuntimeScriptableStorage.RuntimeScriptableObject;
					}
				} else {
					if (GUILayout.Button("Unload", buttonWidth)) {
						RuntimeScriptableObjectProperty.objectReferenceValue = null;
					}
				}
				if (GUILayout.Button("Save")) {
					RuntimeScriptableStorage.Save();
				}
				if (GUILayout.Button("Delete")) {
					if (EditorUtility.DisplayDialog(
						"Confirm Delete Player Scriptable Object File",
						string.Format(
							"Are you sure you want to delete the player file of {0}?",
							DefaultScriptableObjectProperty.objectReferenceValue != null ?
							string.Format("\"{0}\"", DefaultScriptableObjectProperty.objectReferenceValue.name) :
							"a missing ScriptableObject"
						),
						"Delete",
						"Cancel"
					)) {
						RuntimeScriptableStorage.Delete();
					}
				}
				EditorGUI.EndDisabledGroup();
			}
			GUILayout.EndHorizontal();
		}

		private void DrawPlayerScriptableObject() {
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(RuntimeScriptableObjectProperty);
			EditorGUI.EndDisabledGroup();
		}

		private void DrawInspectButton() {
			EditorGUI.BeginDisabledGroup((RuntimeScriptableObjectProperty.objectReferenceValue == null));
			if (GUILayout.Button("Inspect Runtime Scriptable Object")) {
				Selection.activeObject = RuntimeScriptableObjectProperty.objectReferenceValue;
			}
			EditorGUI.EndDisabledGroup();
		}

		#endregion


	}
}