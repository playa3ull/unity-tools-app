namespace CocodriloDog.App {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class DefaultAppSettings : MonoSingleton<DefaultAppSettings> {


		#region Public Fields

		[SerializeField]
		public int TargetFrameRate = 60;

		#endregion


		#region Unity Methods

		protected override void Awake() {
			base.Awake();
			DontDestroyOnLoad(gameObject);
		}

		private void Start() {
			Application.targetFrameRate = TargetFrameRate;
		}

		#endregion


	}

}