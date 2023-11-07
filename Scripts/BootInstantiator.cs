namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Instantiates the boot prefabs referenced in the <see cref="BootPrefabs"/> assets.
	/// </summary>
	public class BootInstantiator : MonoBehaviour {


		#region Public Fields

		/// <summary>
		/// A <see cref="BootPrefabs"/> which prefabs will be instantiated.
		/// </summary>
		[SerializeField]
		public BootPrefabs BootPrefabs;

		#endregion


		#region Unity Methods

		private void Awake() {
			BootPrefabs.InstantiatePrefabs();
		}

		#endregion


	}
}