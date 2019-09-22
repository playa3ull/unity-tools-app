namespace CocodriloDog.App.Examples {

	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class PlayerScriptableStorage_Example : MonoBehaviour {


		#region Unity Methods

		private void OnEnable() {
			InputField.onEndEdit.AddListener(InputField_onEndEdit);
			AddObjectButton.onClick.AddListener(AddObjectButton_onClick);
			SaveButton.onClick.AddListener(SaveButton_onClick);
			DeleteButton.onClick.AddListener(DeleteButton_onClick);
		}

		private void Update() {
			Text.text = string.Format("SomeProperty: {0}", PlayerScriptableObject.SomeField);
		}

		private void OnDisable() {
			InputField.onEndEdit.AddListener(InputField_onEndEdit);
			AddObjectButton.onClick.RemoveListener(AddObjectButton_onClick);
			SaveButton.onClick.RemoveListener(SaveButton_onClick);
			DeleteButton.onClick.RemoveListener(DeleteButton_onClick);
		}

		#endregion


		#region Event Handlers
		
		private void InputField_onEndEdit(string arg0) {
			PlayerScriptableObject.SomeField = arg0;
		}

		private void AddObjectButton_onClick() {
			ExampleScriptableObject1 subobject = new ExampleScriptableObject1();
			subobject.SomeField = "Subobject 3";
			PlayerScriptableObject.SubScriptableObjectsList.Add(subobject);
		}

		private void SaveButton_onClick() {
			PlayerScriptableManager.Save();
		}

		private void DeleteButton_onClick() {
			PlayerScriptableManager.Delete();
		}

		#endregion


		#region Private Fields - Serialized

		[Header("Subcomponents")]

		[SerializeField]
		private Text m_Text;

		[SerializeField]
		private InputField m_InputField;

		[SerializeField]
		private Button m_AddObjectButton;

		[SerializeField]
		private Button m_SaveButton;

		[SerializeField]
		private Button m_DeleteButton;

		#endregion


		#region Private Fields - Non Serialized

		private PlayerScriptableStorage m_PlayerScriptableStorage;

		private ExampleScriptableObject1 m_PlayerScriptableObject;

		#endregion


		#region Private Properties

		private Text Text { get { return m_Text; } }

		private InputField InputField { get { return m_InputField; } }

		private Button AddObjectButton { get { return m_AddObjectButton; } }

		private Button SaveButton { get { return m_SaveButton; } }

		private Button DeleteButton { get { return m_DeleteButton; } }

		private PlayerScriptableStorage PlayerScriptableManager {
			get {
				if(m_PlayerScriptableStorage == null) {
					m_PlayerScriptableStorage = GetComponent<PlayerScriptableStorage>();
				}
				return m_PlayerScriptableStorage;
			}
		}

		private ExampleScriptableObject1 PlayerScriptableObject {
			get {
				if (m_PlayerScriptableObject == null) {
					m_PlayerScriptableObject = PlayerScriptableManager.GetPlayerScriptableObject<ExampleScriptableObject1>();
				}
				return m_PlayerScriptableObject;
			}
		}

		#endregion


	}
}