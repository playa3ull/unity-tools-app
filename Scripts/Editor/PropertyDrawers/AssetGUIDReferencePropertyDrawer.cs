namespace CocodriloDog.App {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using CocodriloDog.Core;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Property drawer for <see cref="AssetGUIDReference"/>.
	/// </summary>
	/// <remarks>
	/// This looks for the GUID of the assigned assets and displays them as read-only.
	/// </remarks>
	[CustomPropertyDrawer(typeof(AssetGUIDReference))]
	public class AssetGUIDReferencePropertyDrawer : PropertyDrawerBase {


		#region Public Methods

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			base.GetPropertyHeight(property, label);
			if (Property.isExpanded) {
				return FieldHeight * 3;
			} else {
				return FieldHeight;
			}
		}

		#endregion


		#region Unity Methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			base.OnGUI(position, property, label);

			EditorGUI.BeginProperty(Position, Label, Property);
			Property.isExpanded = EditorGUI.Foldout(GetNextPosition(), Property.isExpanded, Label);

			if (Property.isExpanded) {
				EditorGUI.indentLevel++;
				DrawAsset();
				DrawGUID();
				EditorGUI.indentLevel--;
			}

			EditorGUI.EndProperty();

		}

		#endregion


		#region Protected Methods

		protected override void InitializePropertiesForOnGUI() {
			base.InitializePropertiesForOnGUI();
			AssetProperty = Property.FindPropertyRelative("Asset");
			GUIDProperty = Property.FindPropertyRelative("GUID");
			AssignAssetGUI();
		}

		#endregion


		#region Private Properties

		private SerializedProperty AssetProperty { get; set; }

		private SerializedProperty GUIDProperty { get; set; }
		
		private string AssetGUID { get; set; }

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

		private void AssignAssetGUI() {
			UnityEngine.Object obj = AssetProperty.objectReferenceValue;
			if (obj != null) {
				string assetPath = AssetDatabase.GetAssetPath(obj);
				AssetGUID = AssetDatabase.AssetPathToGUID(assetPath);
			} else {
				AssetGUID = null;
			}
		}

		#endregion


	}

}