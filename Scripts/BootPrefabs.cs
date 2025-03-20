namespace CocodriloDog.App {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using System;
	using CocodriloDog.Core;


	#region Small Types

	public enum BootPrefabsEditorInstantiationMode {
		InstantiatePrefabsInAllScenes,
		InstantiatePrefabsOnlyInSpecificScenes,
		InstantiatePrefabsInAllScenesExcept,
	}

	#endregion


	/// <summary>
	/// Stores prefabs that will be instantiated when the app boots. This scriptable object must reside
	/// in a Resources folder.
	/// </summary>
	/// 
	/// <remarks>
	/// Normally, the first scene of a Unity project should contain all the objects that will 
	/// live for all the lifecycle of the app. This asset has a reference to those objects and
	/// It can be assigned to a <see cref="BootInstantiator"/> in the first scene of the
	/// Unity project to be instantiated when tha app boots, but it will also instantiate the
	/// prefabs automatically if the developer runs the project from other scenes. This automation
	/// is performed in the <c>BootPrefabsEditor</c>.
	/// </remarks>
	[CreateAssetMenu(menuName = "Cocodrilo Dog/App/Boot Prefabs")]
	public class BootPrefabs : ScriptableObject {


		#region Public Properties

		/// <summary>
		/// This flags register whether the boot prefabs have been instantiated or not.
		/// </summary>
		public bool HasInstantiatedPrefabs {
			get => m_HasInstantiatedPrefabs;
#if UNITY_EDITOR
			set {
				m_HasInstantiatedPrefabs = value;
			}
#endif
		}

		/// <summary>
		/// How many prefabs are referenced?
		/// </summary>
		public int PrefabsCount => m_Prefabs.Count;

		#endregion


		#region Public Methods

		/// <summary>
		/// Instantiates the prefabs.
		/// </summary>
		/// <returns>An array with the clones.</returns>
		public GameObject[] InstantiatePrefabs() {
			// They may have been instantiated by the editor
			if (!HasInstantiatedPrefabs) {
				List<GameObject> clones = new List<GameObject>(PrefabsCount);
				for (int i = 0; i < PrefabsCount; i++) {
					clones.Add(InstantiatePrefabAt(i));
				}
				m_HasInstantiatedPrefabs = true;
				return clones.ToArray();
			} else {
				return null;
			}
		}

		/// <summary>
		/// Gets the prefab at the <paramref name="index"/>
		/// </summary>
		/// <param name="index"></param>
		/// <returns>The prefab.</returns>
		public GameObject GetPrefabAt(int index) => m_Prefabs[index];

		#endregion


		#region Private Fields - Serialized

		[HorizontalLine]
		[Header("RUNTIME")]
		[SerializeField]
		private List<GameObject> m_Prefabs;

		[HorizontalLine]
		[Header("EDITOR")]
		[SerializeField]
		private BootPrefabsEditorInstantiationMode m_EditorInstantiationMode;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private bool m_HasInstantiatedPrefabs;

		#endregion


		#region Private Methods

		/// <summary>
		/// Instantiates the prefab at the <paramref name="index"/>
		/// </summary>
		/// <param name="index"></param>
		/// <returns>The prefab.</returns>
		private GameObject InstantiatePrefabAt(int index) {
			GameObject original = m_Prefabs[index];
			GameObject clone = Instantiate(original);
			clone.name = original.name;
			return clone;
		}

		#endregion


#if UNITY_EDITOR


		#region Public Properties

		public BootPrefabsEditorInstantiationMode EditorInstantiationMode => m_EditorInstantiationMode;

		public int SpecificScenesCount => m_SpecificScenes.Count;

		public int ExceptionScenesCount => m_ExceptionScenes.Count;

		#endregion


		#region Public Methods

		public SceneAsset GetSpecificSceneAt(int index) => m_SpecificScenes[index];

		public SceneAsset GetExceptionSceneAt(int index) => m_ExceptionScenes[index];

		#endregion


		#region Private Fields

		[Tooltip(
			"When running in the editor, the prefabs will be instantiated only when any of these scenes is " +
			"the active one."
		)]
		[SerializeField]
		private List<SceneAsset> m_SpecificScenes;

		[Tooltip(
			"When running in the editor, the prefabs will be instantiated if any scene is the active one, except " +
			"for the scenes in this list."
		)]
		[SerializeField]
		private List<SceneAsset> m_ExceptionScenes;

		#endregion


#endif


	}

}