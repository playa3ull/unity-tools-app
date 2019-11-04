namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(BootstrapPrefabs))]
	public class BootstrapPrefabsEditor : Editor {


		#region Static Event Handlers

		private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj) {
			switch(obj) {
				case PlayModeStateChange.EnteredPlayMode:
					if (FindObjectOfType<BootstrapInstantiator>() == null) {
						foreach (BootstrapPrefabs bootstrapPrefabs in Resources.LoadAll<BootstrapPrefabs>("")) {
							for (int i = 0; i < bootstrapPrefabs.PrefabsCount; i++) {
								GameObject original = bootstrapPrefabs.GetPrefabAt(i);
								GameObject clone = Instantiate(original);
								clone.name = original.name;
								Clones.Add(clone);
							}
						}
					}
					break;
				case PlayModeStateChange.ExitingPlayMode:
					foreach (GameObject clone in Clones) {
						Destroy(clone);
					}
					break;
			}
		}

		#endregion


		#region Private Static Fields

		private static List<GameObject> s_Clones;

		#endregion


		#region Private Static Properties

		private static List<GameObject> Clones {
			get { return s_Clones = s_Clones ?? new List<GameObject>(); }
		}

		#endregion


		#region Private Static Methods

		[InitializeOnLoadMethod]
		private static void InitializeOnLoad() {
			EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
		}

		#endregion


	}
}