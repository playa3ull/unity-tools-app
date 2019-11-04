﻿namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	/// <summary>
	/// This asset is used to load a bootstraping scene in edit mode if it is not already
	/// loaded.
	/// </summary>
	/// 
	/// <remarks>
	/// Normally, the first scene of a unity project should contain all the objects that will 
	/// live for all the lifecycle of the app, so this asset loads that scene additively when the
	/// developer starts the project from a scene other than it. All the loading/unloading is 
	/// carried out by the <c>BootstrapLoaderEditor</c>.
	/// </remarks>
	[CreateAssetMenu(menuName = "Cocodrilo Dog/App/Bootstrap Prefabs")]
	public class BootstrapPrefabs : ScriptableObject {


		#region Public Properties

		public int PrefabsCount {
			get { return m_Prefabs.Count; }
		}

		#endregion


		#region Public Methods

		public GameObject GetPrefabAt(int index) {
			return m_Prefabs[index];
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private List<GameObject> m_Prefabs;

		#endregion


	}

}