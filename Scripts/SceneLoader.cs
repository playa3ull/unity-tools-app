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
			Instance._LoadScene(sceneName, loadSceneMode, autoActivate);
		}

		public static void ActivateScene() {
			Instance._ActivateScene();
		}

		#endregion


		#region Public Fields

		[SerializeField]
		public string DefaultSceneName;

		[SerializeField]
		public LoadSceneMode DefaultSceneLoadMode;

		#endregion


		#region Public Properties

		AbstractSceneLoaderUI UI {
			get { return m_UI; }
			set {
				if(m_UI != null) {
					SubscribeToUI();
				}
				m_UI = value;
				if (m_UI != null) {
					UnsubscribeFromUI();
				}
			}
		}

		#endregion


		#region Unity Methods

		private void Awake() {
			DontDestroyOnLoad(gameObject);
		}

		private void Start() {
			LoadScene(DefaultSceneName, DefaultSceneLoadMode);
			HideCanvasGroup(false);
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


		#region Private Fields

		[SerializeField]
		private AbstractSceneLoaderUI m_UI;

		#endregion


		#region Private Fields

		[NonSerialized]
		private AsyncOperation m_AsyncOperation;

		[NonSerialized]
		private bool m_IsSceneLoaded;

		#endregion


		#region Private Properties

		private bool IsSceneLoaded {
			get { return m_IsSceneLoaded; }
			set {
				if (value != m_IsSceneLoaded) {
					m_IsSceneLoaded = value;
					if(m_IsSceneLoaded) {
						UI.DisplayLoadComplete();
					}
				}
			}
		}

		#endregion


		#region Private Methods - Loading

		private void _LoadScene(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true) {
			UI.DisplayLoadStart();
			ShowCanvasGroup(true, () => LoadSceneAsync(sceneName, loadSceneMode, autoActivate));
		}

		private void _ActivateScene() {
			m_AsyncOperation.allowSceneActivation = true;
			Debug.LogFormat("Activate scene");
		}

		private void LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true) {
			gameObject.SetActive(true);
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
				UI.DisplayLoadProgress(m_AsyncOperation.progress);
				yield return null;
			}

			HideCanvasGroup(true);

			// Reset fields
			IsSceneLoaded = false;
			m_AsyncOperation = null;
		}

		#endregion


		#region Private Methods - UI

		private void ShowCanvasGroup(bool animated, Action onComplete = null) {
			gameObject.SetActive(true);
			if(animated) {
				Animate.GetMotion(this, AlphaKey, v => UI.CanvasGroup.alpha = v)
					.SetEasing(AnimateEasing.QuadInOut)
					.SetOnComplete(() => { onComplete?.Invoke();})
					.Play(0, 1, 0.2f);
			} else {
				UI.CanvasGroup.alpha = 1;
				onComplete?.Invoke();
			}
		}

		private void HideCanvasGroup(bool animated) {
			if (animated) {
				Animate.GetMotion(this, AlphaKey, v => UI.CanvasGroup.alpha = v)
					.SetEasing(AnimateEasing.QuadInOut)
					.SetOnComplete(() => gameObject.SetActive(false))
					.Play(1, 0, 0.2f);
			} else {
				UI.CanvasGroup.alpha = 0;
				gameObject.SetActive(false);
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