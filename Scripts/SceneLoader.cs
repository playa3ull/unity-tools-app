namespace CocodriloDog.App {

	using CocodriloDog.Animation;
	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.UI;

	public class SceneLoader : MonoSingleton<SceneLoader> {


		#region Public Static Methods

		public static void LoadScene(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true) {
			if (Instance != null) {
				Instance._LoadScene(sceneName, loadSceneMode, autoActivate);
			}
		}

		public static void ActivateScene() {
			if (Instance != null) {
				Instance._ActivateScene();
			}
		}

		#endregion


		#region Public Properties

		AbstractSceneLoaderUI UI {
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

		[NonSerialized]
		private float m_FadeInTime = 0.2f;

		[NonSerialized]
		private float m_FadeOutTime = 0.2f;

		#endregion


		#region Private Properties

		private bool IsSceneLoaded {
			get { return m_IsSceneLoaded; }
			set {
				if (value != m_IsSceneLoaded) {
					m_IsSceneLoaded = value;
					if(m_IsSceneLoaded) {
						UI.OnLoadComplete();
					}
				}
			}
		}

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
					IsSceneLoaded = false;
				} else {
					IsSceneLoaded = true;
				}
				// Update the progress always, in case it reached 0.9 very fast
				UI.OnLoadProgress(m_AsyncOperation.progress);
				yield return null;
			}

			HideUI(true);

			// Reset fields
			IsSceneLoaded = false;
			m_AsyncOperation = null;
		}

		#endregion


		#region Private Methods - UI

		private void ShowUI(bool animated, Action onComplete = null) {
			UI.gameObject.SetActive(true);
			if(animated) {
				UI.OnFadeIn(m_FadeInTime);
				Animate.GetMotion(this, AlphaKey, v => UI.CanvasGroup.alpha = v)
					.SetEasing(AnimateEasing.QuadInOut)
					.SetOnComplete(() => { onComplete?.Invoke();})
					.Play(0, 1, m_FadeInTime);
			} else {
				UI.CanvasGroup.alpha = 1;
				onComplete?.Invoke();
			}
		}

		private void HideUI(bool animated) {
			if (animated) {
				UI.OnFadeOut(m_FadeOutTime);
				Animate.GetMotion(this, AlphaKey, v => UI.CanvasGroup.alpha = v)
					.SetEasing(AnimateEasing.QuadInOut)
					.SetOnComplete(() => UI.gameObject.SetActive(false))
					.Play(1, 0, m_FadeOutTime);
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