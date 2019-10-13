namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Cocodrilo Dog/App/Examples/Example Scriptable Object")]
	public class ExampleScriptableObject : ExampleScriptableObjectBase {


		#region Public Fields

		[SerializeField]
		public AudioClip AudioClip;

		[SerializeField]
		public string SomeField;

		#endregion


	}

}