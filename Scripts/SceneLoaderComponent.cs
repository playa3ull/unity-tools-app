namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	/// <summary>
	/// Utility class that allows an easy way to use the <see cref="SceneLoader"/> from a component
	/// without additional scripting.
	/// </summary>
	public class SceneLoaderComponent : MonoBehaviour {


		#region Public Fields

		/// <summary>
		/// The name of the scene to load.
		/// </summary>
		[Tooltip("The name of the scene to load.")]
		[SerializeField]
		public string SceneName;

		/// <summary>
		/// The load scene mode.
		/// </summary>
		[Tooltip("The load scene mode.")]
		[SerializeField]
		public LoadSceneMode LoadSceneMode;

		/// <summary>
		/// Will the scene autoactivate?
		/// </summary>
		[Tooltip("Will the scene autoactivate?")]
		[SerializeField]
		public bool AutoActivate = true;

		/// <summary>
		/// Will the scene autohide?
		/// </summary>
		[Tooltip("Will the scene autohide?")]
		[SerializeField]
		public bool AutoHideUI = true;

		#endregion


		#region Public Methods

		public void LoadScene() {
			SceneLoader.LoadScene(SceneName, LoadSceneMode, AutoActivate, AutoHideUI);
		}

		#endregion


	}

}