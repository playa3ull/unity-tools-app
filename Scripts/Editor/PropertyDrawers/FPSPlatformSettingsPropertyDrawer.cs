namespace CocodriloDog.App {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(FPSPlatformSettings))]
	public class FPSPlatformSettingsPropertyDrawer : PropertyDrawerBase {


		#region Public Methods

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.GetPropertyHeight(property, label);
			if (Property.isExpanded) { 
				height += EditorGUI.GetPropertyHeight(PlatformProperty) + 2;
				height += EditorGUI.GetPropertyHeight(ModeProperty) + 2;
				height += EditorGUIUtility.singleLineHeight + 2;
			}
			return height;
		}

		#endregion


		#region Unity Methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			
			base.OnGUI(position, property, label);

			Label = EditorGUI.BeginProperty(Position, Label, Property);

			Property.isExpanded = EditorGUI.Foldout(GetNextPosition(), Property.isExpanded, Property.displayName, true);
			EditorGUI.indentLevel++;
			if (Property.isExpanded) {
				EditorGUI.PropertyField(GetNextPosition(PlatformProperty), PlatformProperty);
				EditorGUI.PropertyField(GetNextPosition(ModeProperty), ModeProperty);
				switch((FrameRateMode)ModeProperty.intValue) {
					case FrameRateMode.TargetFrameRate:
						EditorGUI.PropertyField(GetNextPosition(TargetFrameRateProperty), TargetFrameRateProperty);
						break;
					case FrameRateMode.VSyncCount:
						EditorGUI.PropertyField(GetNextPosition(VSyncCountProperty), VSyncCountProperty);
						break;
				}
			}
			EditorGUI.indentLevel--;

			EditorGUI.EndProperty();

		}

		#endregion


		#region Protected Methods

		protected override void InitializePropertiesForGetHeight() {
			base.InitializePropertiesForGetHeight();
			PlatformProperty = Property.FindPropertyRelative("m_Platform");
			ModeProperty = Property.FindPropertyRelative("m_Mode");
			TargetFrameRateProperty = Property.FindPropertyRelative("m_TargetFrameRate");
			VSyncCountProperty = Property.FindPropertyRelative("m_VSyncCount");
		}

		protected override void InitializePropertiesForOnGUI() {
			base.InitializePropertiesForOnGUI();
			PlatformProperty = Property.FindPropertyRelative("m_Platform");
			ModeProperty = Property.FindPropertyRelative("m_Mode");
			TargetFrameRateProperty = Property.FindPropertyRelative("m_TargetFrameRate");
			VSyncCountProperty = Property.FindPropertyRelative("m_VSyncCount");
		}

		#endregion


		#region Private Properties

		private SerializedProperty PlatformProperty { get; set; }

		private SerializedProperty ModeProperty { get; set; }

		private SerializedProperty TargetFrameRateProperty { get; set; }
		
		private SerializedProperty VSyncCountProperty { get; set; }

		#endregion


	}

}
