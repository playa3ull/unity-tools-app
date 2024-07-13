namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class ExampleScriptableObjectBase : ScriptableObject {


		#region Public Properties

		public List<ExampleScriptableObject> SubScriptableObjectsList {
			get { return m_SubScriptableObjectsList = m_SubScriptableObjectsList ?? new List<ExampleScriptableObject>(); }
		}

		#endregion


		#region Private Fields

		/// <summary>
		/// We use a base class to demonstrate that private serialized fields in 
		/// base classes are stored.
		/// </summary>
		[SerializeField]
		private List<ExampleScriptableObject> m_SubScriptableObjectsList;

		#endregion


	}
}