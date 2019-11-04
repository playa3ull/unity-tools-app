namespace CocodriloDog.App {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// Base class for assets that will collect a list of other assets and serialize
	/// their GUID.
	/// </summary>
	/// <typeparam name="T_Asset"></typeparam>
	/// <typeparam name="T_Reference"></typeparam>
	public class AssetsGUID<T_Asset, T_Reference> : ScriptableObject 
		where T_Asset : UnityEngine.Object
		where T_Reference : AssetGUIDReference<T_Asset> {


		#region Public Methods

		/// <summary>
		/// Get the GUID of the specified <paramref name="obj"/>.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>The GUID string o null if not found.</returns>
		public string GUIDFromAsset(T_Asset obj) {
			T_Reference assetReference = AssetReferences.FirstOrDefault(ar => ar.Asset == obj);
			return assetReference?.GUID;
		}

		/// <summary>
		/// Get the asset that matches the provided <paramref name="guid"/>
		/// </summary>
		/// <param name="guid"></param>
		/// <returns>
		/// The asset that has the <paramref name="guid"/> or null if not found.
		/// </returns>
		public T_Asset AssetFromGUID(string guid) {
			T_Reference assetReference = AssetReferences.FirstOrDefault(ar => ar.GUID == guid);
			return assetReference?.Asset;
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private List<T_Reference> m_AssetReferences;

		#endregion


		#region Private Properties

		private List<T_Reference> AssetReferences {
			get { return m_AssetReferences = m_AssetReferences ?? new List<T_Reference>(); }
		}

		#endregion


	}

	/// <summary>
	/// Base class that pairs an asset with its corresponding GUID.
	/// </summary>
	/// <typeparam name="T_Asset"></typeparam>
	[Serializable]
	public class AssetGUIDReference<T_Asset>
		where T_Asset : UnityEngine.Object {


		#region Public Fields

		[SerializeField]
		public T_Asset Asset;

		[SerializeField]
		public string GUID;

		#endregion


	}
}