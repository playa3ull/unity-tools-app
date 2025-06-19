namespace CocodriloDog.App {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	/// <summary>
	/// Utility class that allows an easy way to use the <see cref="SceneLoader"/> from a component
	/// without additional scripting.
	/// </summary>
	public class SceneLoaderInvoker : MonoBehaviour {


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

		/// <summary>
		/// Will the scene load on start?
		/// </summary>
		public bool LoadSceneOnStart {
			get => m_LoadSceneOnStart;
			set => m_LoadSceneOnStart = value;
		}

		#endregion


		#region Public Methods

		public void LoadScene() {
			m_SceneLoader.Value.LoadScene(SceneName, LoadSceneMode, AutoActivate, AutoHideUI);
		}

		#endregion


		#region Unity Methods

		private void Start() {
			if (LoadSceneOnStart) {
				LoadScene();
			}
		}

		#endregion


		#region Private Fields

		[Tooltip("A reference to the SceneLoader that this trigger will use.")]
		[SerializeField]
		private ScriptableReferenceField<SceneLoader> m_SceneLoader;

		[Tooltip("The name of the scene to load.")]
		[SerializeField]
		private string m_SceneName;

		[Tooltip("The load scene mode.")]
		[SerializeField]
		private LoadSceneMode m_LoadSceneMode;

		[Tooltip("Will the scene autoactivate?")]
		[SerializeField]
		private bool m_AutoActivate = true;

		[Tooltip("Will the loading UI autohide?")]
		[SerializeField]
		private bool m_AutoHideUI = true;

		[Tooltip("Will the scene load on start?")]
		[SerializeField]
		private bool m_LoadSceneOnStart;

		#endregion


	}

}