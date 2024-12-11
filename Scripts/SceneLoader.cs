namespace CocodriloDog.App {

	using CocodriloDog.MotionKit;
	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.UI;

	/// <summary>
	/// A MonoBehaviour that can load scenes asynchronously and display the load progress.
	/// </summary>
	/// 
	/// <remarks>
	/// In needs a reference to an <see cref="AbstractSceneLoaderUI"/> in order to display
	/// the download progress.
	/// </remarks>
	public class SceneLoader : MonoBehaviour {


		#region Public Static Methods

		/// <summary>
		/// Loads a scene.
		/// </summary>
		/// 
		/// <param name="sceneName">The name of the scene</param>
		/// <param name="loadSceneMode">The <see cref="LoadSceneMode"/></param>
		/// 
		/// <param name="autoActivate">
		/// Will the scene activate immediately after it loads? If false, a call
		/// to <see cref="ActivateScene"/> is required to activate the scene.
		/// </param>
		/// 
		///	<param name="autoHideUI">
		/// Will the UI hide immediately after it loads? If false, a call
		/// to <see cref="HideUI(bool, Action)"/> is required to hide the scene.
		/// </param>
		public void LoadScene(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true, bool autoHideUI = true) {
			// Reset before the fade in so that no progress bars are shown filled
			UI.OnLoadProgress(0);
			EnableCanvases();
			ShowUI(true, () => LoadSceneAsync(sceneName, loadSceneMode, autoActivate, autoHideUI));
		}

		/// <summary>
		/// Loads a scene with the option of performing an action just before loading the
		/// new scene (after the scene loader has faded-in).
		/// </summary>
		/// 
		/// <param name="sceneName">The name of the scene</param>
		/// <param name="loadSceneMode">The <see cref="LoadSceneMode"/></param>
		/// 
		/// <param name="beforeLoadAction">
		/// Any action to be performed just before start the loading of the new scene
		/// (after the scene loader has faded-in).
		/// </param>
		/// 
		/// <param name="waitForSecondsToLoad">
		/// After performing the <paramref name="beforeLoadAction"/>, it will wait
		/// this time before loading the new scene.
		/// </param>
		/// 
		/// <param name="autoActivate">
		/// Will the scene activate immediately after it loads? If false, a call
		/// to <see cref="ActivateScene"/> is required to activate the scene.
		/// </param>
		/// 
		///	<param name="autoHideUI">
		/// Will the UI hide immediately after it loads? If false, a call
		/// to <see cref="HideUI(bool, Action)"/> is required to hide the scene.
		/// </param>
		public void LoadScene(
			string sceneName,
			LoadSceneMode loadSceneMode,
			Action beforeLoadAction,
			float waitForSecondsToLoad = 0,
			bool autoActivate = true, 
			bool autoHideUI = true) {

			// Reset before the fade in so that no progress bars are shown filled
			UI.OnLoadProgress(0);
			EnableCanvases();
			ShowUI(true, () => {
				// Perform the action
				beforeLoadAction?.Invoke();
				if (Mathf.Approximately(waitForSecondsToLoad, 0)) {
					// Loads the new scene immediately
					LoadSceneAsync(sceneName, loadSceneMode, autoActivate, autoHideUI);
				} else {
					// Wait some time before loading the new scene
					MotionKit.GetTimer().Play(waitForSecondsToLoad)
						.SetOnComplete(() => LoadSceneAsync(sceneName, loadSceneMode, autoActivate, autoHideUI));
				}
			});

		}

		/// <summary>
		/// Activates the scene.
		/// </summary>
		public void ActivateScene() {
			m_AsyncOperation.allowSceneActivation = true;
		}

		/// <summary>
		/// Hides the loader UI.
		/// </summary>
		/// <param name="animated">hide will be animated</param>
		/// <param name="onComplete">An action to invoke on complete</param>
		public void HideUI(bool animated, Action onComplete = null) {
			if (animated) {
				UI.OnFadeOut(FadeOutTime);
				MotionKit.GetMotion(this, AlphaKey, v => UI.CanvasGroup.alpha = v)
					.SetTimeMode(TimeMode.Unscaled)
					.SetEasing(MotionKitEasing.QuadInOut)
					.SetOnComplete(() => {
						UI.gameObject.SetActive(false);
						DisableCanvases();
					})
					.Play(1, 0, FadeOutTime);
			} else {
				UI.CanvasGroup.alpha = 0;
				UI.gameObject.SetActive(false);
			}
		}

		#endregion


		#region Public Fields

		/// <summary>
		/// The duration of the fade in effect.
		/// </summary>
		[SerializeField]
		public float FadeInTime = 0.2f;

		/// <summary>
		/// The duration of the fade out effect.
		/// </summary>
		[SerializeField]
		public float FadeOutTime = 0.2f;

		#endregion


		#region Public Properties

		/// <summary>
		/// The UI.
		/// </summary>
		public AbstractSceneLoaderUI UI {
			get { return m_UI; }
			set {
				if (m_UI != null) {
					UnsubscribeFromUI();
				}
				m_UI = value ?? throw new ArgumentNullException(string.Format("{0} can not be null", nameof(UI)));
				if (enabled) {
					SubscribeToUI();
				}
				HideUI(false);
			}
		}

		#endregion


		#region Unity Methods

		protected void Awake() {
			DontDestroyOnLoad(gameObject);
			DisableCanvases();
			HideUI(false);
		}

		private void OnEnable() {
			SubscribeToUI();
		}

		private void Start() {
			// Initialize so that it is not created OnDestroy()
			_ = MotionKit.Instance;
		}

		private void OnValidate() {
			FadeInTime = Mathf.Clamp(FadeInTime, 0, float.MaxValue);
			FadeOutTime = Mathf.Clamp(FadeOutTime, 0, float.MaxValue);
		}

		private void Reset() {
			FadeInTime = 0.2f;
			FadeOutTime = 0.2f;
		}

		private void OnDisable() {
			UnsubscribeFromUI();
		}

		protected void OnDestroy() {
			MotionKit.ClearPlaybacks(this);
		}

		#endregion


		#region Event Handlers

		private void ActivateSceneButton_onClick() {
			ActivateScene();
		}

		#endregion


		#region Private Fields

		private const string AlphaKey = "Alpha";

		#endregion


		#region Private Fields - Serialized

		[SerializeField]
		private AbstractSceneLoaderUI m_UI;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private Canvas[] m_Canvases;

		[NonSerialized]
		private AsyncOperation m_AsyncOperation;

		[NonSerialized]
		private bool m_IsSceneLoaded;

		#endregion


		#region Private Fields - Non Serialized

		private Canvas[] Canvases {
			get {
				if(m_Canvases == null) {
					m_Canvases = GetComponentsInChildren<Canvas>();
				}
				return m_Canvases;
			}
		}

		#endregion


		#region Private Methods - Loading


		private void LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true, bool autoHideUI = true) {
			UI.OnLoadStart();
			StartCoroutine(_LoadSceneAsync(sceneName, loadSceneMode, autoActivate, autoHideUI));
		}

		private IEnumerator _LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true, bool autoHideUI = true) {

			m_AsyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
			m_AsyncOperation.allowSceneActivation = autoActivate;

			while (!m_AsyncOperation.isDone) {
				if (m_AsyncOperation.progress < 0.9f) {
					UpdateIsSceneLoaded(false);
				} else {
					UpdateIsSceneLoaded(true);
				}
				// Update the progress always, in case it reached 0.9 very fast
				UI.OnLoadProgress(m_AsyncOperation.progress);
				yield return null;
			}

			if (autoHideUI) {
				HideUI(true);
			}

			// Reset fields
			UpdateIsSceneLoaded(false);
			m_AsyncOperation = null;
		}

		private void UpdateIsSceneLoaded(bool value) {
			if (value != m_IsSceneLoaded) {
				m_IsSceneLoaded = value;
				if (m_IsSceneLoaded) {
					UI.OnLoadComplete();
				}
			}
		}

		#endregion


		#region Private Methods - UI

		private void EnableCanvases() {
			foreach (Canvas canvas in Canvases) {
				canvas.enabled = true;
			}
		}

		private void DisableCanvases() {
			foreach (Canvas canvas in Canvases) {
				canvas.enabled = false;
			}
		}

		private void ShowUI(bool animated, Action onComplete = null) {
			UI.gameObject.SetActive(true);
			if (animated) {
				UI.OnFadeIn(FadeInTime);
				MotionKit.GetMotion(this, AlphaKey, v => UI.CanvasGroup.alpha = v)
					.SetTimeMode(TimeMode.Unscaled)
					.SetEasing(MotionKitEasing.QuadInOut)
					.SetOnComplete(() => onComplete?.Invoke())
					.Play(0, 1, FadeInTime);
			} else {
				UI.CanvasGroup.alpha = 1;
				onComplete?.Invoke();
			}
		}

		private void SubscribeToUI() {
			if (UI.ActivateSceneButton != null) {
				UI.ActivateSceneButton.onClick.AddListener(ActivateSceneButton_onClick);
			}
		}

		private void UnsubscribeFromUI() {
			if (UI.ActivateSceneButton != null) {
				UI.ActivateSceneButton.onClick.RemoveListener(ActivateSceneButton_onClick);
			}
		}

		#endregion


	}
}