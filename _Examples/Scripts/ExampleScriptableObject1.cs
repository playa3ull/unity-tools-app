namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Cocodrilo Dog/App/Examples/Example Scriptable Object")]
	public class ExampleScriptableObject1 : ScriptableObject {


		#region Public Fields

		[SerializeField]
		public string SomeField;

		[SerializeField]
		public string[] SomeFields;

		[SerializeField]
		public ExampleScriptableObject2 OtherScriptableObject;

		[SerializeField]
		public ExampleScriptableObject1 SubScriptableObject;

		[SerializeField]
		public ExampleScriptableObject1[] SubScriptableObjectsArray;

		[SerializeField]
		public List<ExampleScriptableObject1> SubScriptableObjectsList;

		#endregion


	}

}