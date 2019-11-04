namespace CocodriloDog.App {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// This will collect a list of assets and serialize their respective GUIDs.
	/// </summary>
	/// <remarks>
	///	This was created to work in conjunction with <see cref="PlayerScriptableStorage"/>
	///	to store the reference to assets as GUIDs.
	/// </remarks>
	[CreateAssetMenu(menuName = "Cocodrilo Dog/App/Assets GUID")]
	public class AssetsGUID : ScriptableObject {


		#region Public Methods

		/// <summary>
		/// Get the GUID of the specified <paramref name="obj"/>.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>The GUID string o null if not found.</returns>
		public string GUIDFromAsset(UnityEngine.Object obj) {
			AssetGUIDReference assetReference = AssetReferences.FirstOrDefault(ar => ar.Asset == obj);
			return assetReference?.GUID;
		}

		/// <summary>
		/// Get the asset that matches the provided <paramref name="guid"/>
		/// </summary>
		/// <param name="guid"></param>
		/// <returns>
		/// The asset that has the <paramref name="guid"/> or null if not found.
		/// </returns>
		public UnityEngine.Object AssetFromGUID(string guid) {
			AssetGUIDReference assetReference = AssetReferences.FirstOrDefault(ar => ar.GUID == guid);
			return assetReference?.Asset;
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private List<AssetGUIDReference> m_AssetReferences;

		#endregion


		#region Private Properties

		private List<AssetGUIDReference> AssetReferences {
			get { return m_AssetReferences = m_AssetReferences ?? new List<AssetGUIDReference>(); }
		}

		#endregion


	}

	/// <summary>
	/// A class pairs an asset with its corresponding GUID.
	/// </summary>
	[Serializable]
	public class AssetGUIDReference {


		#region Public Fields

		[SerializeField]
		public UnityEngine.Object Asset;

		[SerializeField]
		public string GUID;

		#endregion


	}
}