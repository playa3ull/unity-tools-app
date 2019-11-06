namespace CocodriloDog.App {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Cocodrilo Dog/App/Multiplatform GameObject")]
	public class MultiplatformGameObject : KeyedResource<RuntimePlatform, GameObject, MultiplatformGameObjectEntry> {


		#region Public Properties

		public override RuntimePlatform CurrentKey {
			get { return Application.platform; }
		}

		public override RuntimePlatform FallbackKey {
			get { return RuntimePlatform.WindowsPlayer; }
		}

		#endregion


	}

	[Serializable]
	public class MultiplatformGameObjectEntry: KeyedResourceEntry<RuntimePlatform, GameObject> { }

}