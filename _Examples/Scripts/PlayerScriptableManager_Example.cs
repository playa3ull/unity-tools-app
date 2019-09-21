namespace CocodriloDog.App.Examples {

	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class PlayerScriptableManager_Example : MonoBehaviour {


		#region Unity Methods

		private void OnEnable() {
			SaveButton.onClick.AddListener(SaveButton_onClick);
			DeleteButton.onClick.AddListener(DeleteButton_onClick);
			InputField.onEndEdit.AddListener(InputField_onEndEdit);
		}

		private void Update() {
			Text.text = string.Format("SomeProperty: {0}", PlayerScriptableObject.SomeField);
		}

		private void OnDisable() {
			SaveButton.onClick.RemoveListener(SaveButton_onClick);
			DeleteButton.onClick.RemoveListener(DeleteButton_onClick);
			InputField.onEndEdit.AddListener(InputField_onEndEdit);
		}

		#endregion


		#region Event Handlers

		private void SaveButton_onClick() {
			PlayerScriptableManager.Save();
		}

		private void DeleteButton_onClick() {
			PlayerScriptableManager.Delete();
		}

		private void InputField_onEndEdit(string arg0) {
			PlayerScriptableObject.SomeField = arg0;
		}

		#endregion


		#region Private Fields

		[Header("Subcomponents")]

		[SerializeField]
		private Button m_SaveButton;

		[SerializeField]
		private Button m_DeleteButton;

		[SerializeField]
		private Text m_Text;

		[SerializeField]
		private InputField m_InputField;

		private PlayerScriptableStorage m_PlayerScriptableStorage;

		private ExampleScriptableObject m_PlayerScriptableObject;

		#endregion


		#region Private Properties

		private Button SaveButton { get { return m_SaveButton; } }

		private Button DeleteButton { get { return m_DeleteButton; } }

		private Text Text { get { return m_Text; } }

		private InputField InputField { get { return m_InputField; } }

		private PlayerScriptableStorage PlayerScriptableManager {
			get {
				if(m_PlayerScriptableStorage == null) {
					m_PlayerScriptableStorage = GetComponent<PlayerScriptableStorage>();
				}
				return m_PlayerScriptableStorage;
			}
		}

		private ExampleScriptableObject PlayerScriptableObject {
			get {
				if (m_PlayerScriptableObject == null) {
					m_PlayerScriptableObject = PlayerScriptableManager.GetPlayerScriptableObject<ExampleScriptableObject>();
				}
				return m_PlayerScriptableObject;
			}
		}

		#endregion


	}
}