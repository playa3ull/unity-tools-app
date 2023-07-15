namespace CocodriloDog.App {


	using CocodriloDog.CD_JSON;
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
		/// The <see cref="ScriptableObject"/> that is a starting point for the data
		/// that will be modified at runtime and then saved into disk.
		/// </summary>
		public ScriptableObject DefaultScriptableObject => m_DefaultScriptableObject;

		/// <summary>
		/// Is there a stored file?
		/// </summary>
		public bool FileExists => File.Exists(CrossPlatformFilePath);

		/// <summary>
		/// The full path of the file.
		/// </summary>
		public string CrossPlatformFilePath {
			get {
				string path = null;
				if (!string.IsNullOrEmpty(FilePath)) {
					path = Application.persistentDataPath;
					// Allow the developer to type the path separated with "/"
					string[] pathSteps = FilePath.Split('/');
					// Create a cross-platform path with the Path class
					for (int i = 0; i < pathSteps.Length; i++) {
						path = Path.Combine(path, pathSteps[i]);
					}
					return path;
				}
				return path;
			}
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
					// TODO: Handle the case where the loaded file can not be read.
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
			if (RuntimeScriptableObject != null) {
				if(Application.isPlaying) {
					Destroy(RuntimeScriptableObject);
				}
				RuntimeScriptableObject = null;
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
		private ScriptableObject m_RuntimeScriptableObject;

		#endregion


		#region Private Properties

		private string RuntimeScriptableObjectName => string.Format("{0} (From file)", DefaultScriptableObject.name);

		#endregion


		#region Private Methods

		private T Load<T>() where T : ScriptableObject => Load(typeof(T)) as T;

		private ScriptableObject Load(Type type) {
			if (File.Exists(CrossPlatformFilePath)) {
				string jsonString = File.ReadAllText(CrossPlatformFilePath);
				try {
					return CD_JSON.Deserialize(type, jsonString) as ScriptableObject;
				} catch(Exception e) {
					Debug.LogWarning($"File has no proper format: {e}");
				}
			}
			return null;
		}

		private void Save(ScriptableObject runtimeScriptableObject, string path) {

			// Create the directory if it doesn't exists
			Directory.CreateDirectory(Path.GetDirectoryName(path));

			// Save the data
			var serialized = CD_JSON.Serialize(runtimeScriptableObject, true);
			File.WriteAllText(path, serialized);

		}

		#endregion


	}

}