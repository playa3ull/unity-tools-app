namespace CocodriloDog.App {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	public class AssetsGUID<T_Asset, T_Reference> : ScriptableObject 
		where T_Asset : UnityEngine.Object
		where T_Reference : AssetGUIDReference<T_Asset> {


		#region Public Methods

		public string GUIDFromAsset(T_Asset obj) {
			T_Reference assetReference = AssetReferences.FirstOrDefault(ar => ar.Asset == obj);
			return assetReference?.GUID;
		}

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