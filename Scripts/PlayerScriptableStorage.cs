namespace CocodriloDog.App {

	using Leguar.TotalJSON;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using UnityEngine;

	/// <summary>
	/// Stores player data from scriptable objects into disk.
	/// </summary>
	/// 
	/// <remarks>
	/// When there is not file in disk, this creates a runtime clone of the provided
	/// <see cref="m_DefaultScriptableObject"/> and modifies the clone in order to preserve the 
	/// <see cref="m_DefaultScriptableObject"/> with its values. If there is a file in disk, 
	/// it creates a scriptable object with that data.
	/// </remarks>
	public class PlayerScriptableStorage : MonoBehaviour {


		#region Public Fields

		public string FilePath;

		#endregion


		#region Public Properties

		public bool FileExists {
			get { return File.Exists(Path.Combine(Application.persistentDataPath, FilePath)); }
		}

		#endregion


		#region Public Methods

		public ScriptableObject PlayerScriptableObject {
			get {

				if (m_DefaultScriptableObject == null) {
					throw new InvalidOperationException(
						string.Format("{0} can not be null", nameof(m_DefaultScriptableObject))
					);
				}

				// Try to load it first
				if (m_PlayerScriptableObject == null) {
					Load(m_DefaultScriptableObject.GetType());
				}
				// If file doesn't exist, then create a clone of the default scriptable object.
				if (m_PlayerScriptableObject == null) {
					m_PlayerScriptableObject = Instantiate(m_DefaultScriptableObject);
				}

				return m_PlayerScriptableObject;

			}
		}

		public T GetPlayerScriptableObject<T>() where T: ScriptableObject {

			if (m_DefaultScriptableObject == null) {
				throw new InvalidOperationException(
					string.Format("{0} can not be null", nameof(m_DefaultScriptableObject))
				);
			}

			// Try to load it first
			if (m_PlayerScriptableObject == null) {
				Load<T>();
			} 
			// If file doesn't exist, then create a clone of the default scriptable object.
			if (m_PlayerScriptableObject == null) {
				m_PlayerScriptableObject = Instantiate(m_DefaultScriptableObject);
			}
			return m_PlayerScriptableObject as T;
		}

		public void Save() {
			Save(PlayerScriptableObject, CrossPlatformFilePath);
		}

		public void Delete() {
			if (m_PlayerScriptableObject != null) {
				if(Application.isPlaying) {
					Destroy(m_PlayerScriptableObject);
				}
				m_PlayerScriptableObject = null;
			}
			string fullPath = Path.Combine(Application.persistentDataPath, FilePath);
			if (File.Exists(fullPath)) {
				File.Delete(fullPath);
			}
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private ScriptableObject m_DefaultScriptableObject;

		[SerializeField]
		private ScriptableObject m_PlayerScriptableObject;

		#endregion


		#region Private Properties

		private string CrossPlatformFilePath {
			get {
				// Allow the developer to type the path separated with "/"
				string[] pathSteps = FilePath.Split('/');
				string path = Application.persistentDataPath;

				// Create a cross-platform path with the Path class
				for (int i = 0; i < pathSteps.Length; i++) {
					path = Path.Combine(path, pathSteps[i]);
				}
				return path;
			}
		}

		private string PlayerScriptableObjectName { 
			get { return string.Format("{0} (From file)", m_DefaultScriptableObject.name); } 
		}

		#endregion


		#region Private Methods

		private void Load<T>() where T : ScriptableObject {
			Load(typeof(T));
		}

		private void Load(Type typ) {

			if (File.Exists(CrossPlatformFilePath)) {

				string jsonString = File.ReadAllText(CrossPlatformFilePath);

				JSON json = JSON.ParseString(jsonString);
				m_PlayerScriptableObject = (ScriptableObject)json.zDeserialize(typ, null);

			}
		}

		private void Save(ScriptableObject playerScriptableObject, string path) {

			if (m_DefaultScriptableObject == null) {
				throw new InvalidOperationException(
					string.Format("{0} can not be null", nameof(m_DefaultScriptableObject))
				);
			}

			// Create the directory if it doesn't exists
			Directory.CreateDirectory(Path.GetDirectoryName(path));

			// Save the data
			//string playerScriptableJson = JsonUtility.ToJson(playerScriptableObject, true);
			JSON json = JSON.Serialize(playerScriptableObject);
			//string playerScriptableJson = json.CreatePrettyString();
			File.WriteAllText(path, json.CreatePrettyString());

			//SavePlayerScriptableFields(PlayerScriptableObject);

		}

		//private FieldInfo[] SavePlayerScriptableFields(PlayerScriptableObject playerScriptableObject) {

		//	BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		//	Type type = playerScriptableObject.GetType();

 	//		FieldInfo[] fieldInfos = null;
		//	while (type.BaseType != null) {

		//		fieldInfos = type.GetFields(bindingFlags);

		//		// Look all fields
		//		foreach (FieldInfo fieldInfo in fieldInfos) {

		//			// Just a reference
		//			if(typeof(PlayerScriptableObject).IsAssignableFrom(fieldInfo.FieldType)) {

		//				Debug.LogFormat("Is reference = {0}", fieldInfo.Name);
		//				PlayerScriptableObject subPlayerScriptableObject = (PlayerScriptableObject)fieldInfo.GetValue(playerScriptableObject);

		//				if (subPlayerScriptableObject != null) {

		//					Debug.LogFormat("\tValue = {0}", subPlayerScriptableObject);
		//					Debug.LogFormat("\tInstance ID = {0}", subPlayerScriptableObject.GetInstanceID());

		//					string path = CrossPlatformFilePath;
		//					string directoryName = Path.GetDirectoryName(path);
		//					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
		//					string extension = Path.GetExtension(path);
		//					string subfileName = string.Format(
		//						"{0}_{1}{2}",
		//						fileNameWithoutExtension, 
		//						subPlayerScriptableObject.GetInstanceID(), 
		//						extension
		//					);
		//					string subPath = Path.Combine(directoryName, subfileName);

		//					Debug.LogFormat("subPath: {0}", subPath);

		//					//Save(subPlayerScriptableObject, subPath);
		//				}

		//			}

		//			// Is of list type
		//			if (typeof(IList).IsAssignableFrom(fieldInfo.FieldType)) {

		//				// Array
		//				if (fieldInfo.FieldType.IsArray) {
		//					if (typeof(PlayerScriptableObject).IsAssignableFrom(fieldInfo.FieldType.GetElementType())) {
		//						Debug.LogFormat("Is array = {0}", fieldInfo.Name);
		//					}
		//				}

		//				// List
		//				if (fieldInfo.FieldType.IsGenericType) {
		//					if (typeof(PlayerScriptableObject).IsAssignableFrom(fieldInfo.FieldType.GenericTypeArguments[0])) {
		//						Debug.LogFormat("Is list = {0}", fieldInfo.Name);
		//					}
		//				}

		//			}

		//		}
		//		type = type.BaseType;
		//	}

		//	return fieldInfos;

		//}

		#endregion


	}

}