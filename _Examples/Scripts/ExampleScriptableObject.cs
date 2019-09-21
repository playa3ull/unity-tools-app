namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Cocodrilo Dog/App/Examples/Example Scriptable Object")]
	public class ExampleScriptableObject : ScriptableObject {


		#region Public Fields

		[SerializeField]
		public string SomeField;

		#endregion


	}

}