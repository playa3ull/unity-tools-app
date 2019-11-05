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
	/// In needs a reference to a <see cref="AbstractSceneLoaderUI"/> in order to display
	/// the download progress.
	/// </remarks>
	public class SceneLoader : MonoSingleton<SceneLoader> {


		#region Public Static Methods

		/// <summary>
		/// Load a scene.
		/// </summary>
		/// <param name="sceneName">The name of the scene</param>
		/// <param name="loadSceneMode">The <see cref="LoadSceneMode"/></param>
		/// <param name="autoActivate">
		/// Will the scene activate immediatly after it loads? If false, a call
		/// to <see cref="ActivateScene"/> is required to activate the scene.
		/// </param>
		public static void LoadScene(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true) {
			if (Instance != null) {
				Instance._LoadScene(sceneName, loadSceneMode, autoActivate);
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

		protected override void Awake() {
			base.Awake();
			DontDestroyOnLoad(gameObject);
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
			Animate.ClearMotions(this);
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
		private AsyncOperation m_AsyncOperation;

		[NonSerialized]
		private bool m_IsSceneLoaded;

		#endregion


		#region Private Methods - Loading

		private void _LoadScene(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true) {
			// Reset to before the fade in so that no progress bars are shown filled
			UI.OnLoadProgress(0);
			ShowUI(true, () => LoadSceneAsync(sceneName, loadSceneMode, autoActivate));
		}

		private void _ActivateScene() {
			m_AsyncOperation.allowSceneActivation = true;
		}

		private void LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true) {
			UI.OnLoadStart();
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
				UI.OnLoadProgress(m_AsyncOperation.progress);
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
					UI.OnLoadComplete();
				}
			}
		}

		#endregion


		#region Private Methods - UI

		private void ShowUI(bool animated, Action onComplete = null) {
			UI.gameObject.SetActive(true);
			if(animated) {
				UI.OnFadeIn(FadeInTime);
				Animate.GetMotion(this, AlphaKey, v => UI.CanvasGroup.alpha = v)
					.SetEasing(AnimateEasing.QuadInOut)
					.SetOnComplete(() => { onComplete?.Invoke();})
					.Play(0, 1, FadeInTime);
			} else {
				UI.CanvasGroup.alpha = 1;
				onComplete?.Invoke();
			}
		}

		private void HideUI(bool animated) {
			if (animated) {
				UI.OnFadeOut(FadeOutTime);
				Animate.GetMotion(this, AlphaKey, v => UI.CanvasGroup.alpha = v)
					.SetEasing(AnimateEasing.QuadInOut)
					.SetOnComplete(() => UI.gameObject.SetActive(false))
					.Play(1, 0, FadeOutTime);
			} else {
				UI.CanvasGroup.alpha = 0;
				UI.gameObject.SetActive(false);
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