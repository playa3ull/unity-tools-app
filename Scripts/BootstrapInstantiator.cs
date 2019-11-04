namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class BootstrapInstantiator : MonoBehaviour {


		#region Public Fields

		[SerializeField]
		public BootstrapPrefabs BootstrapPrefabs;

		#endregion


		#region Unity Methods

		private void Awake() {
			for(int i = 0; i < BootstrapPrefabs.PrefabsCount; i++) {
				GameObject original = BootstrapPrefabs.GetPrefabAt(i);
				GameObject clone = Instantiate(original);
				clone.name = original.name;
			}
		}

		#endregion


	}
}