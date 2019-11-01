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


	}

}