namespace CocodriloDog.App {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(MultiplatformGameObject))]
	public class MultiplatformGameObjectEditor : KeyedResourceEditor {


		#region Protected Properties

		protected override float PropertiesMiddle { get { return 0.4f; } }

		protected override string KeyLabel { get { return "Platform"; } }

		protected override float KeyLabelWidth { get { return 55; } }

		protected override string ResourceLabel { get { return "Game Object"; } }

		protected override float ResourceLabelWidth { get { return 80; } }

		protected override string FallbackKeyValue { get { return ((MultiplatformGameObject)target).FallbackKey.ToString(); } }

		#endregion


	}

}