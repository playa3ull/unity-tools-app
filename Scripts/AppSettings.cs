namespace CocodriloDog.App {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AppSettings : MonoSingleton<AppSettings> {


		#region Public Fields

		[SerializeField]
		public int TargetFrameRate = 60;

		[SerializeField]
		public bool HalveMacRetina = true;

		#endregion


		#region Unity Methods

		protected override void Awake() {
			base.Awake();
			DontDestroyOnLoad(gameObject);
		}

		private void Start() {

			Application.targetFrameRate = TargetFrameRate;

			if (Application.platform == RuntimePlatform.OSXPlayer) {
				// TODO: This should be the default behavior, but once the user has chosen
				// a resolution, this code should be executed
				if (HalveMacRetina) {
					Resolution highestResolution = Screen.resolutions[Screen.resolutions.Length - 1];
					if (Screen.dpi > 200) { // Is retina (221)
						if (Screen.currentResolution.width == highestResolution.width &&
							Screen.currentResolution.height == highestResolution.height) {
							Screen.SetResolution(
								highestResolution.width / 2,
								highestResolution.height / 2,
								Screen.fullScreen
							);
						}
					}
				}
			}

		}

		#endregion


	}

}