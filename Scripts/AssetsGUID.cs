namespace CocodriloDog.App {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AssetsGUID<T_Asset, T_Reference> : ScriptableObject 
		where T_Reference : AssetGUIDReference<T_Asset> {


		#region Private Fields

		[SerializeField]
		private List<T_Reference> m_AssetReferences;

		#endregion


	}

	[Serializable]
	public class AssetGUIDReference<T_Asset> {


		#region Public Fields

		[SerializeField]
		public T_Asset Asset;

		[SerializeField]
		public string GUID;

		#endregion


	}
}