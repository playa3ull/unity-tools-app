namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Cocodrilo Dog/App/Bootstrap Prefabs")]
	public class BootstrapPrefabs : ScriptableObject {


		#region Private Fields

		[SerializeField]
		private List<GameObject> m_Prefabs;

		#endregion


	}

}