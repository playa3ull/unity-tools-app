namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using CocodriloDog.Core;
	using UnityEditor;
	using UnityEngine;

	public class AssetGUIDReferencePropertyDrawer : PropertyDrawerBase {


		#region Public Methods

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			if (property.isExpanded) {
				return FieldHeight * 3;
			} else {
				return FieldHeight;
			}
		}

		#endregion


		#region Unity Methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			base.OnGUI(position, property, label);

			EditorGUI.BeginProperty(position, label, property);
			property.isExpanded = EditorGUI.Foldout(GetNextPosition(), property.isExpanded, label);

			if (property.isExpanded) {
				EditorGUI.indentLevel++;
				DrawAsset();
				DrawGUID();
				EditorGUI.indentLevel--;
			}

			EditorGUI.EndProperty();

		}

		#endregion


		#region Private Properties

		private SerializedProperty AssetProperty {
			get {
				return Property.FindPropertyRelative("Asset");
			}
		}

		private SerializedProperty GUIDProperty {
			get {
				return Property.FindPropertyRelative("GUID");
			}
		}
		
		private string AssetGUID {
			get {
				Object obj = AssetProperty.objectReferenceValue;
				if (obj != null) {
					string assetPath = AssetDatabase.GetAssetPath(obj);
					return AssetDatabase.AssetPathToGUID(assetPath);
				}
				return null;
			}
		}

		#endregion


		#region Private Methods

		private void DrawAsset() {
			EditorGUI.PropertyField(GetNextPosition(), AssetProperty);
		}

		private void DrawGUID() {
			EditorGUI.BeginDisabledGroup(true);
			GUIDProperty.stringValue = AssetGUID;
			EditorGUI.PropertyField(GetNextPosition(), GUIDProperty);
			EditorGUI.EndDisabledGroup();
		}

		#endregion


	}

}