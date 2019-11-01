namespace CocodriloDog.App {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class SceneLoader : MonoSingleton<SceneLoader> {


		#region Unity Methods

		private void Awake() {
			DontDestroyOnLoad(gameObject);
		}

		#endregion


		#region Private Methods

		#endregion


	}
}