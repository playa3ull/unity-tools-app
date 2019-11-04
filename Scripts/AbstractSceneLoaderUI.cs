namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public abstract class AbstractSceneLoaderUI : MonoBehaviour {


		#region Public Properties

		public abstract CanvasGroup CanvasGroup { get; }

		public abstract Button ActivateSceneButton { get; }

		#endregion


		#region Public Methods

		public virtual void OnFadeIn(float duration) { }

		public virtual void OnLoadStart() { }

		public virtual void OnLoadProgress(float progress) { }

		public virtual void OnLoadComplete() { }

		public virtual void OnFadeOut(float duration) { }

		#endregion


	}

}