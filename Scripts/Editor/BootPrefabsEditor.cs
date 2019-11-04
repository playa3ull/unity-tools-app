namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Custom editor for <see cref="BootPrefabs"/>. It instantiates the prefabs
	/// when no <see cref="BootInstantiator"/> is found. This would be the case
	/// when running the project from a scene other than the boot scene and helps
	/// to simulate the runtime state without needing to run the boot scene first.
	/// </summary>
	[CustomEditor(typeof(BootPrefabs))]
	public class BootPrefabsEditor : Editor {


		#region Static Event Handlers

		private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj) {
			switch(obj) {
				case PlayModeStateChange.ExitingEditMode:
					if (FindObjectOfType<BootInstantiator>() == null) {
						// Didn't find a BootInstantiator, look for the BootPrefabs assets.
						foreach (BootPrefabs bootPrefabs in Resources.LoadAll<BootPrefabs>("")) {
							// Iterate through the prefabs
							for (int i = 0; i < bootPrefabs.PrefabsCount; i++) {
								// Instantiate each of the prefabs with PrefabUtility to preserve
								// the connection to the prefab.
								GameObject original = bootPrefabs.GetPrefabAt(i);
								GameObject clone = PrefabUtility.InstantiatePrefab(original) as GameObject;
								Debug.LogFormat("BootPrefabs: Instantiated boot object {0}", clone);
								clone.name = original.name;
							}
						}
					}
					break;
				case PlayModeStateChange.EnteredEditMode:
					if (FindObjectOfType<BootInstantiator>() == null) {
						// Didn't find a BootInstantiator, we need to destroy the created clones
						foreach (BootPrefabs bootPrefabs in Resources.LoadAll<BootPrefabs>("")) {
							// Iterate through the prefabs
							for (int i = 0; i < bootPrefabs.PrefabsCount; i++) {

								GameObject prefab = bootPrefabs.GetPrefabAt(i);
								// Iterate through the game objects in the scene to check if they
								// are derived from the prefab
								foreach (GameObject gameObject in FindObjectsOfType<GameObject>()) {
									if(prefab == PrefabUtility.GetCorrespondingObjectFromSource(gameObject)) {
										// Destroy the instance. It will only be destroyed if it was instantiated
										// with the PrefabUtility (when the objects are blue). This ensures that
										// any other instance created at runtime with the normal Instantiate()
										// methods, won't be destroyed.
										Debug.LogFormat("BootPrefabs: Destroyed boot object {0}", gameObject);
										DestroyImmediate(gameObject);
									}
								}

							}
						}
					}
					break;
			}
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