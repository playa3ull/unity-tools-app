namespace CocodriloDog.App {

	using Leguar.TotalJSON;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using UnityEngine;

	/// <summary>
	/// Stores runtime data from scriptable objects into disk.
	/// </summary>
	/// 
	/// <remarks>
	/// When there is not file in disk, this creates a runtime clone of the provided
	/// <see cref="DefaultScriptableObject"/>. The clone can be modified at runtime
	/// and the changes can be saved in disk. If there is a file in disk, it creates 
	/// a scriptable object with that data.
	/// </remarks>
	[CreateAssetMenu(menuName = "Cocodrilo Dog/App/Runtime Scriptable Storage")]
	public class RuntimeScriptableStorage : ScriptableObject {


		#region Public Fields

		/// <summary>
		/// A path to store the file. It is relative to <see cref="Application.persistentDataPath"/>
		/// </summary>
		public string FilePath;

		#endregion


		#region Public Properties

		/// <summary>
		/// Is there a stored file?
		/// </summary>
		public bool FileExists {
			get { return File.Exists(CrossPlatformFilePath); }
		}

		/// <summary>
		/// The runtime version of the scriptable object, either loaded from disk or
		/// copied from the <see cref="DefaultScriptableObject"/>.
		/// </summary>
		public ScriptableObject RuntimeScriptableObject {
			get {

				if (DefaultScriptableObject == null) {
					throw new InvalidOperationException(
						string.Format("{0} can not be null", nameof(DefaultScriptableObject))
					);
				}

				// Try to load it first
				if (m_RuntimeScriptableObject == null) {
					m_RuntimeScriptableObject = Load(DefaultScriptableObject.GetType());
					if (m_RuntimeScriptableObject != null) {
						m_RuntimeScriptableObject.name = RuntimeScriptableObjectName;
					}
				}

				// If file doesn't exist, then create a clone of the default scriptable object.
				if (m_RuntimeScriptableObject == null) {
					m_RuntimeScriptableObject = Instantiate(DefaultScriptableObject);
				}

				return m_RuntimeScriptableObject;

			}
			private set { m_RuntimeScriptableObject = value; }
		}

		/// <summary>
		/// The <see cref="ScriptableObject"/> that is a starting point for the data
		/// that will be modified at runtime and then saved into disk.
		/// </summary>
		private ScriptableObject DefaultScriptableObject {
			get { return m_DefaultScriptableObject; }
		}

		/// <summary>
		/// An optional <see cref="App.AssetsGUID"/> that must be provided when references to
		/// assets such as <see cref="AudioClip"/> will be saved in the data file.
		/// </summary>
		public AssetsGUID AssetsGUID {
			get { return m_AssetsGUID; }
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Gets the <see cref="RuntimeScriptableObject"/> of the <typeparamref name="T"/> type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>
		/// The <see cref="RuntimeScriptableObject"/> of the <typeparamref name="T"/> type.
		/// </returns>
		public T GetRuntimeScriptableObject<T>() where T: ScriptableObject {

			if (DefaultScriptableObject == null) {
				throw new InvalidOperationException(
					string.Format("{0} can not be null", nameof(DefaultScriptableObject))
				);
			}

			// Try to load it first
			if (m_RuntimeScriptableObject == null) {
				m_RuntimeScriptableObject = Load<T>();
				if (m_RuntimeScriptableObject != null) {
					m_RuntimeScriptableObject.name = RuntimeScriptableObjectName;
				}
			} 

			// If file doesn't exist, then create a clone of the default scriptable object.
			if (m_RuntimeScriptableObject == null) {
				m_RuntimeScriptableObject = Instantiate(DefaultScriptableObject);
			}

			return m_RuntimeScriptableObject as T;

		}

		/// <summary>
		/// Saves the <see cref="RuntimeScriptableObject"/> in its current state in the
		/// specified <see cref="FilePath"/>.
		/// </summary>
		public void Save() {
			if (DefaultScriptableObject == null) {
				throw new InvalidOperationException(
					string.Format("{0} can not be null", nameof(DefaultScriptableObject))
				);
			}
			Save(RuntimeScriptableObject, CrossPlatformFilePath);
		}

		/// <summary>
		/// Deletes the file at <see cref="FilePath"/>, if any.
		/// </summary>
		public void Delete() {
			string fullPath = Path.Combine(Application.persistentDataPath, FilePath);
			if (File.Exists(fullPath)) {
				File.Delete(fullPath);
			}
			if (RuntimeScriptableObject != null) {
				if(Application.isPlaying) {
					Destroy(RuntimeScriptableObject);
				}
				RuntimeScriptableObject = null;
			}
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private ScriptableObject m_DefaultScriptableObject;

		[SerializeField]
		private AssetsGUID m_AssetsGUID;
		
		[SerializeField]
		private ScriptableObject m_RuntimeScriptableObject;

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

		private string RuntimeScriptableObjectName { 
			get { return string.Format("{0} (From file)", DefaultScriptableObject.name); } 
		}

		#endregion


		#region Private Methods

		private T Load<T>() where T : ScriptableObject {
			return Load(typeof(T)) as T;
		}

		private ScriptableObject Load(Type type) {
			if (File.Exists(CrossPlatformFilePath)) {
				string jsonString = File.ReadAllText(CrossPlatformFilePath);
				JSON json = JSON.ParseString(jsonString);
				return (ScriptableObject)json.Deserialize(type, AssetsGUID);
			}
			return null;
		}

		private void Save(ScriptableObject runtimeScriptableObject, string path) {

			// Create the directory if it doesn't exists
			Directory.CreateDirectory(Path.GetDirectoryName(path));

			// Save the data
			JSON json = JSON.Serialize(runtimeScriptableObject, AssetsGUID);
			File.WriteAllText(path, json.CreatePrettyString());

		}

		#endregion


	}

}