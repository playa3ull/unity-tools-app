namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	public class DefaultSceneLoader : MonoBehaviour {


		#region Public Fields

		[SerializeField]
		public string DefaultSceneName;

		[SerializeField]
		public LoadSceneMode LoadSceneMode;

		[SerializeField]
		public bool AutoActivate = true;

		#endregion


		#region Unity Methods

		private void Start() {
			SceneLoader.LoadScene(DefaultSceneName, LoadSceneMode, AutoActivate);
		}

		#endregion


	}
}