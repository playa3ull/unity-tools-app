namespace CocodriloDog.App {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(MultiplatformScriptableObject))]
	public class MultiplatformScriptableObjectEditor : KeyedResourceEditor {


		#region Protected Properties

		protected override float PropertiesMiddle { get { return 0.4f; } }

		protected override string KeyLabel { get { return "Platform"; } }

		protected override float KeyLabelWidth { get { return 55; } }

		protected override string ResourceLabel { get { return "Scriptable Object"; } }

		protected override float ResourceLabelWidth { get { return 110; } }

		protected override string FallbackKeyValue { get { return ((MultiplatformScriptableObject)target).FallbackKey.ToString(); } }

		#endregion


	}

}