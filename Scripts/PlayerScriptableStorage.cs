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
	/// <see cref="m_DefaultScriptableObject"/>. The clone can be modified at runtime
	/// and the changes can be saved in disk. If there is a file in disk, it creates 
	/// a scriptable object with that data.
	/// </remarks>
	[CreateAssetMenu(menuName = "Cocodrilo Dog/App/Player Scriptable Storage")]
	public class PlayerScriptableStorage : ScriptableObject {


		#region Public Fields

		public string FilePath;

		#endregion


		#region Public Properties

		public bool FileExists {
			get { return File.Exists(CrossPlatformFilePath); }
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
				string path = "";
				if (!string.IsNullOrEmpty(FilePath)) {
					// Allow the developer to type the path separated with "/"
					string[] pathSteps = FilePath.Split('/');
					path = Application.persistentDataPath;

					// Create a cross-platform path with the Path class
					for (int i = 0; i < pathSteps.Length; i++) {
						path = Path.Combine(path, pathSteps[i]);
					}
					return path;
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
			JSON json = JSON.Serialize(playerScriptableObject);
			File.WriteAllText(path, json.CreatePrettyString());

		}

		#endregion


	}

}