namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(PlayerScriptableStorage))]
	public class PlayerScriptableStorageEditor : Editor {


		#region Unity Methods

		public override void OnInspectorGUI() {
			serializedObject.Update();
			DrawScript();
			EditorGUILayout.PropertyField(FilePathProperty); 
			EditorGUILayout.PropertyField(ScriptableObjectProperty);
			DrawScriptableObjectHelpBox();
			DrawPlayerScriptableObject();
			DrawFileHelpBox();
			DrawDelete();
			serializedObject.ApplyModifiedProperties();
			Repaint();
		}

		#endregion


		#region Private Fields

		private PlayerScriptableStorage m_PlayerScriptableStorage;

		private SerializedProperty m_ScriptProperty;

		private SerializedProperty m_FilePathProperty;

		private SerializedProperty m_ScriptableObjectProperty;

		private SerializedProperty m_PlayerScriptableObjectProperty;

		#endregion


		#region Private Properties

		private PlayerScriptableStorage PlayerScriptableStorage {
			get {
				if(m_PlayerScriptableStorage == null) {
					m_PlayerScriptableStorage = (PlayerScriptableStorage)target;
				}
				return m_PlayerScriptableStorage;
			}
		}

		private SerializedProperty ScriptProperty {
			get { return m_ScriptProperty = m_ScriptProperty ?? serializedObject.FindProperty("m_Script"); }
		}

		private SerializedProperty FilePathProperty {
			get { return m_FilePathProperty = m_FilePathProperty ?? serializedObject.FindProperty("FilePath"); }
		}

		private SerializedProperty ScriptableObjectProperty {
			get { return m_ScriptableObjectProperty = m_ScriptableObjectProperty ?? serializedObject.FindProperty("m_ScriptableObject"); }
		}

		private SerializedProperty PlayerScriptableObjectProperty {
			get { return m_PlayerScriptableObjectProperty = m_PlayerScriptableObjectProperty ?? serializedObject.FindProperty("m_PlayerScriptableObject"); }
		}

		#endregion


		#region Private Methods

		private void DrawScript() {
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(ScriptProperty);
			EditorGUI.EndDisabledGroup();
		}

		private void DrawPlayerScriptableObject() {
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(PlayerScriptableObjectProperty);
			EditorGUI.EndDisabledGroup();
		}

		private void DrawScriptableObjectHelpBox() {
			if(ScriptableObjectProperty.objectReferenceValue == null) {
				EditorGUILayout.HelpBox("A ScriptableObject must be assigned.", MessageType.Error);
			}
		}

		private void DrawFileHelpBox() {
			if (PlayerScriptableStorage.FileExists) {
				EditorGUILayout.HelpBox(
					string.Format(
						"File \"{0}\" exists in the specified FilePath.", 
						Path.GetFileName(PlayerScriptableStorage.FilePath)
					),
					MessageType.Info
				);
			} else {
				EditorGUILayout.HelpBox("No file found at the specified FilePath.", MessageType.Info);
			}
		}

		private void DrawDelete() {
			GUILayout.BeginHorizontal();
			{
				EditorGUI.BeginDisabledGroup(!PlayerScriptableStorage.FileExists);
				if (GUILayout.Button("Load")) {
					PlayerScriptableStorage.GetPlayerScriptableObject();
				}
				if (GUILayout.Button("Unload")) {
					PlayerScriptableObjectProperty.objectReferenceValue = null;
				}
				if (GUILayout.Button("Delete")) {
					if (EditorUtility.DisplayDialog(
						"Confirm Delete Player Scriptable Object File",
						string.Format(
							"Are you sure you want to delete the player file of {0}?",
							ScriptableObjectProperty.objectReferenceValue != null ?
							string.Format("\"{0}\"", ScriptableObjectProperty.objectReferenceValue.name) :
							"a missing ScriptableObject"
						),
						"Delete",
						"Cancel"
					)) {
						PlayerScriptableStorage.Delete();
					}
				}
				EditorGUI.EndDisabledGroup();
			}
			GUILayout.EndHorizontal();
		}

		#endregion


	}
}