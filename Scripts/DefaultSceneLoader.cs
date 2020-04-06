namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	public class DefaultSceneLoader : MonoBehaviour {


		#region Public Fields

		public string DefaultSceneName;

		#endregion


		#region Unity Methods

		private void Start() {
			SceneLoader.LoadScene(DefaultSceneName, LoadSceneMode.Single);
		}

		#endregion


	}
}