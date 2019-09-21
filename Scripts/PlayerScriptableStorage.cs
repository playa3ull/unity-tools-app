namespace CocodriloDog.App {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using UnityEngine;

	/// <summary>
	/// Stores player data from scriptable objects into disk.
	/// </summary>
	/// 
	/// <remarks>
	/// When there is not file in disk, this creates a runtime clone of the provided
	/// <see cref="m_ScriptableObject"/> and modifies the clone in order to preserve the 
	/// <see cref="m_ScriptableObject"/> with its values. If there is a file in disk, 
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

		public ScriptableObject GetPlayerScriptableObject() {
			// Try to load it first
			if (m_PlayerScriptableObject == null) {
				Load(m_ScriptableObject.GetType());
			}
			// If file doesn't exist, then create a clone of the default scriptable object.
			if (m_PlayerScriptableObject == null) {
				m_PlayerScriptableObject = Instantiate(m_ScriptableObject);
			}
			return m_PlayerScriptableObject;
		}

		public T GetPlayerScriptableObject<T>() where T: ScriptableObject {
			// Try to load it first
			if (m_PlayerScriptableObject == null) {
				Load<T>();
			} 
			// If file doesn't exist, then create a clone of the default scriptable object.
			if (m_PlayerScriptableObject == null) {
				m_PlayerScriptableObject = Instantiate(m_ScriptableObject);
			}
			return m_PlayerScriptableObject as T;
		}

		public void Save<T>() where T : ScriptableObject {

			// Allow the developer to type the path separated with "/"
			string[] pathSteps = FilePath.Split('/');
			string fullPath = Application.persistentDataPath;

			// Create a cross-platform path with the Path class
			for(int i = 0; i < pathSteps.Length; i++) {
				fullPath = Path.Combine(fullPath, pathSteps[i]);
			}

			// Create the directory if it doesn't exists
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

			// Save the data
			string scriptableDataJSON = JsonUtility.ToJson(GetPlayerScriptableObject<T>(), true);
			File.WriteAllText(fullPath, scriptableDataJSON);

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
		private ScriptableObject m_ScriptableObject;

		[SerializeField]
		private ScriptableObject m_PlayerScriptableObject;

		#endregion


		#region Private Properties

		private string PlayerScriptableObjectName { 
			get { return string.Format("{0} (From file)", m_ScriptableObject.name); } 
		}

		#endregion


		#region Private Methods

		private void Load<T>() where T : ScriptableObject {
			string fullPath = Path.Combine(Application.persistentDataPath, FilePath);
			if (File.Exists(fullPath)) {
				string json = File.ReadAllText(fullPath);
				m_PlayerScriptableObject = (T)Activator.CreateInstance(typeof(T), null);
				m_PlayerScriptableObject.name = PlayerScriptableObjectName;
				JsonUtility.FromJsonOverwrite(json, m_PlayerScriptableObject);
			}
		}

		private void Load(Type type) { 
			string fullPath = Path.Combine(Application.persistentDataPath, FilePath);
			if (File.Exists(fullPath)) {
				string json = File.ReadAllText(fullPath);
				m_PlayerScriptableObject = (ScriptableObject)Activator.CreateInstance(type, null);
				m_PlayerScriptableObject.name = PlayerScriptableObjectName;
				JsonUtility.FromJsonOverwrite(json, m_PlayerScriptableObject);
			}
		}

		#endregion


	}

}