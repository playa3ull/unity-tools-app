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


		#region Public Methods

		public event Action<float> OnLoadProgress;

		public event Action OnLoadComplete;

		public event Action OnSceneActivated;

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
						Debug.LogFormat("Loaded {0}", m_AsyncOperation.progress);
						OnLoadComplete?.Invoke();
					}
				}
			}
		}

		#endregion


		#region Private Methods - Loading

		private void _LoadScene(string sceneName, LoadSceneMode loadSceneMode, bool autoActivate = true) {
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
					Debug.LogFormat("Loading {0}", m_AsyncOperation.progress);
					OnLoadProgress?.Invoke(m_AsyncOperation.progress);
					IsSceneLoaded = false;
				} else {
					IsSceneLoaded = true;
				}

				yield return null;
			}

			HideCanvasGroup(true);

			Debug.LogFormat("Activated {0}", m_AsyncOperation.progress);
			OnSceneActivated?.Invoke();

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