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
		public Vector2Int ScreenResolution = new Vector2Int(1920, 1080);

		#endregion


		#region Unity Methods

		protected override void Awake() {
			base.Awake();
			DontDestroyOnLoad(gameObject);
		}

		private void Start() {
			Application.targetFrameRate = TargetFrameRate;
			//Screen.SetResolution(ScreenResolution.x, ScreenResolution.y, Screen.fullScreen);
		}

		#endregion


	}

}