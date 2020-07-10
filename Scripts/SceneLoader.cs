namespace CocodriloDog.App {

	using CocodriloDog.Animation;
	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.UI;

	/// <summary>
	/// A singleton that can load scenes asynchronously and display the load progress.
	/// </summary>
	/// <remarks>
	/// In needs a reference to a <see cref="AbstractSceneLoaderUIController"/> in order to display
	/// the download progress.
	/// </remarks>
	public class SceneLoader : MonoSingleton<SceneLoader> {


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
		public static void LoadScene(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true) {
			if (Instance != null) {
				Instance._LoadScene(sceneName, loadSceneMode, autoActivate);
			}
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
		/// <param name="beforeLoadWaitForSeconds">
		/// After performing the <paramref name="beforeLoadAction"/>, it will wait
		/// this time before loading the new scene.
		/// </param>
		/// 
		/// <param name="autoActivate">
		/// Will the scene activate immediately after it loads? If false, a call
		/// to <see cref="ActivateScene"/> is required to activate the scene.
		/// </param>
		public static void LoadScene(
			string sceneName,
			LoadSceneMode loadSceneMode,
			Action beforeLoadAction,
			float beforeLoadWaitForSeconds = 0,
			bool autoActivate = true) {

			if (Instance != null) {
				Instance._LoadScene(sceneName, loadSceneMode, beforeLoadAction, beforeLoadWaitForSeconds, autoActivate);
			}

		}

		/// <summary>
		/// Activates the scene.
		/// </summary>
		public static void ActivateScene() {
			if (Instance != null) {
				Instance._ActivateScene();
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
		public AbstractSceneLoaderUIController UIController {
			get { return m_UIController; }
			set {
				if (m_UIController != null) {
					UnsubscribeFromUI();
				}
				m_UIController = value ?? throw new ArgumentNullException(string.Format("{0} can not be null", nameof(UIController)));
				if (enabled) {
					SubscribeToUI();
				}
				HideUI(false);
			}
		}

		#endregion


		#region Unity Methods

		protected override void Awake() {
			base.Awake();
			DontDestroyOnLoad(gameObject);
			DisableCanvases();
			HideUI(false);
		}

		private void OnEnable() {
			SubscribeToUI();
		}

		private void Start() {
			// Initialize so that it is not created OnDestroy()
			Animate animate = Animate.Instance;
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

		protected override void OnDestroy() {
			base.OnDestroy();
			Animate.ClearPlaybacks(this);
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
		private AbstractSceneLoaderUIController m_UIController;

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

		private void _LoadScene(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true) {
			// Reset before the fade in so that no progress bars are shown filled
			UIController.OnLoadProgress(0);
			EnableCanvases();
			ShowUI(true, () => LoadSceneAsync(sceneName, loadSceneMode, autoActivate));
		}

		private void _LoadScene(
			string sceneName,
			LoadSceneMode loadSceneMode,
			Action beforeLoadAction,
			float beforeLoadWaitForSeconds = 0,
			bool autoActivate = true
		) {

			// Reset before the fade in so that no progress bars are shown filled
			UIController.OnLoadProgress(0);
			EnableCanvases();
			ShowUI(true, () => {
				// Perform the action
				beforeLoadAction?.Invoke();
				if(Mathf.Approximately(beforeLoadWaitForSeconds, 0)) {
					// Loads the new scene immediately
					LoadSceneAsync(sceneName, loadSceneMode, autoActivate);
				} else {
					// Wait some time before loading the new scene
					Animate.GetTimer().Play(beforeLoadWaitForSeconds)
						.SetOnComplete(() => LoadSceneAsync(sceneName, loadSceneMode, autoActivate));
				}
			});

		}

		private void _ActivateScene() {
			m_AsyncOperation.allowSceneActivation = true;
		}

		private void LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true) {
			UIController.OnLoadStart();
			StartCoroutine(_LoadSceneAsync(sceneName, loadSceneMode, autoActivate));
		}

		private IEnumerator _LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true) {

			m_AsyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
			m_AsyncOperation.allowSceneActivation = autoActivate;

			while (!m_AsyncOperation.isDone) {
				if (m_AsyncOperation.progress < 0.9f) {
					UpdateIsSceneLoaded(false);
				} else {
					UpdateIsSceneLoaded(true);
				}
				// Update the progress always, in case it reached 0.9 very fast
				UIController.OnLoadProgress(m_AsyncOperation.progress);
				yield return null;
			}

			HideUI(true);

			// Reset fields
			UpdateIsSceneLoaded(false);
			m_AsyncOperation = null;
		}

		private void UpdateIsSceneLoaded(bool value) {
			if (value != m_IsSceneLoaded) {
				m_IsSceneLoaded = value;
				if (m_IsSceneLoaded) {
					UIController.OnLoadComplete();
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
			UIController.gameObject.SetActive(true);
			if (animated) {
				UIController.OnFadeIn(FadeInTime);
				Animate.GetMotion(this, AlphaKey, v => UIController.CanvasGroup.alpha = v)
					.SetTimeMode(TimeMode.Unscaled)
					.SetEasing(AnimateEasing.QuadInOut)
					.SetOnComplete(() => onComplete?.Invoke())
					.Play(0, 1, FadeInTime);
			} else {
				UIController.CanvasGroup.alpha = 1;
				onComplete?.Invoke();
			}
		}

		private void HideUI(bool animated, Action onComplete = null) {
			if (animated) {
				UIController.OnFadeOut(FadeOutTime);
				Animate.GetMotion(this, AlphaKey, v => UIController.CanvasGroup.alpha = v)
					.SetTimeMode(TimeMode.Unscaled)
					.SetEasing(AnimateEasing.QuadInOut)
					.SetOnComplete(() => {
						UIController.gameObject.SetActive(false);
						DisableCanvases();
					})
					.Play(1, 0, FadeOutTime);
			} else {
				UIController.CanvasGroup.alpha = 0;
				UIController.gameObject.SetActive(false);
			}
		}

		private void SubscribeToUI() {
			if (UIController.ActivateSceneButton != null) {
				UIController.ActivateSceneButton.onClick.AddListener(ActivateSceneButton_onClick);
			}
		}

		private void UnsubscribeFromUI() {
			if (UIController.ActivateSceneButton != null) {
				UIController.ActivateSceneButton.onClick.RemoveListener(ActivateSceneButton_onClick);
			}
		}

		#endregion


	}
}