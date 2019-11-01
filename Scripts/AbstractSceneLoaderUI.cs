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

		public virtual void DisplayLoadStart() { }

		public virtual void DisplayLoadProgress(float progress) { }

		public virtual void DisplayLoadComplete() { }

		#endregion


	}

}