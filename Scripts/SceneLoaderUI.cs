namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Implementation of <see cref="AbstractSceneLoaderUI"/>
	/// </summary>
	public class SceneLoaderUI : AbstractSceneLoaderUI {


		#region Public Properties

		public override CanvasGroup CanvasGroup { get { return m_CanvasGroup; } }

		public override Button ActivateSceneButton { get { return m_ActivateSceneButton; } }

		#endregion


		#region Public Methods

		public override void OnFadeIn(float duration) {
			base.OnFadeIn(duration);
			if (ActivateSceneButton != null) {
				ActivateSceneButton.gameObject.SetActive(false);
			}
		}

		public override void OnLoadProgress(float progress) {
			base.OnLoadProgress(progress);
			if (ProgressImage != null) {
				ProgressImage.fillAmount = progress / 0.9f;
			}
		}

		public override void OnLoadComplete() {
			base.OnLoadComplete();
			if (ActivateSceneButton != null) {
				ActivateSceneButton.gameObject.SetActive(true);
			}
		}

		#endregion


		#region Unity Methods

		private void Start() {
			if (ActivateSceneButton != null) {
				ActivateSceneButton.gameObject.SetActive(false);
			}
		}

		#endregion


		#region Private Fields

		[Header("Subcomponents")]

		[SerializeField]
		private CanvasGroup m_CanvasGroup;

		[SerializeField]
		private Button m_ActivateSceneButton;

		[SerializeField]
		private Image m_ProgressImage;

		#endregion


		#region Public Properties

		private Image ProgressImage { get { return m_ProgressImage; } }

		#endregion


	}
}