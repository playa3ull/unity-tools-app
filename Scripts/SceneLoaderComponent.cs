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


		#region Public Properties

		/// <summary>
		/// The name of the scene to load.
		/// </summary>
		public string SceneName {
			get => m_SceneName;
			set => m_SceneName = value;
		}

		/// <summary>
		/// The load scene mode.
		/// </summary>
		public LoadSceneMode LoadSceneMode {
			get => m_LoadSceneMode;
			set => m_LoadSceneMode = value;
		}

		/// <summary>
		/// Will the scene autoactivate?
		/// </summary>
		public bool AutoActivate {
			get => m_AutoActivate;
			set => m_AutoActivate = value;
		}

		/// <summary>
		/// Will the scene autohide?
		/// </summary>
		public bool AutoHideUI {
			get => m_AutoHideUI;
			set => m_AutoHideUI = value;
		}

		#endregion


		#region Public Methods

		public void LoadScene() {
			SceneLoader.LoadScene(SceneName, LoadSceneMode, AutoActivate, AutoHideUI);
		}

		#endregion


		#region Private Fields

		[Tooltip("The name of the scene to load.")]
		[SerializeField]
		private string m_SceneName;

		[Tooltip("The load scene mode.")]
		[SerializeField]
		private LoadSceneMode m_LoadSceneMode;

		[Tooltip("Will the scene autoactivate?")]
		[SerializeField]
		private bool m_AutoActivate = true;

		[Tooltip("Will the scene autohide?")]
		[SerializeField]
		private bool m_AutoHideUI = true;

		#endregion


	}

}