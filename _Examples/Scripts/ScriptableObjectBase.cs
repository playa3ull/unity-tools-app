namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class ScriptableObjectBase : ScriptableObject {


		#region Public Properties

		public List<ExampleScriptableObject> SubScriptableObjectsList {
			get { return m_SubScriptableObjectsList = m_SubScriptableObjectsList ?? new List<ExampleScriptableObject>(); }
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private List<ExampleScriptableObject> m_SubScriptableObjectsList;

		#endregion


	}
}