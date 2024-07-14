namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Base class for UIs that react to <see cref="SceneLoader"/>
	/// </summary>
	public abstract class AbstractSceneLoaderUI : MonoBehaviour {


		#region Public Properties

		/// <summary>
		/// The <see cref="CanvasGroup"/> that will be faded in and out
		/// when the loader UI appears. This should be placed in the top-most object
		/// of the UI.
		/// </summary>
		public abstract CanvasGroup CanvasGroup { get; }

		/// <summary>
		/// A button that when clicked will activate the scene if it was loaded by
		/// <c>autoActivate = false</c>. It can be null.
		/// </summary>
		public abstract Button ActivateSceneButton { get; }

		#endregion


		#region Public Methods

		/// <summary>
		/// Called when the loader UI is faded in, but before the load start.
		/// </summary>
		/// <param name="duration">duration of the fade in</param>
		public virtual void OnFadeIn(float duration) { }

		/// <summary>
		/// Called after the fade in completed and the load process starts.
		/// </summary>
		public virtual void OnLoadStart() { }

		/// <summary>
		/// Called when the load progresses.
		/// </summary>
		/// <param name="progress">A number from 0 to 0.9</param>
		public virtual void OnLoadProgress(float progress) { }

		/// <summary>
		/// Called when the scene is loaded, but before being activated.
		/// </summary>
		public virtual void OnLoadComplete() { }

		/// <summary>
		/// Called when the scene is activated and the loader UI starts to fade out.
		/// </summary>
		/// <param name="duration"></param>
		public virtual void OnFadeOut(float duration) { }

		#endregion


	}

}